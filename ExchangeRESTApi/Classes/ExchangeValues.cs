using ExchangeRESTApi.BaseClasses;
using ExchangeRESTApi.Controllers;
using ExchangeRESTApi.Data;
using ExchangeRESTApi.Interfaces;
using ExchangeRESTApi.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRESTApi.Classes
{
    public class ExchangeValues : BaseExchangeValues, IExchangeValues
    {
        public ExchangeValues()
        { 
        }

        internal ExchangeValues(DateTime startDate, DateTime endDate)
        {
            try{
                if(DateTime.Compare(startDate, endDate) == 0)
                {
                    //set data when start have no data
                    startDate = SetWhenNothing(startDate);
                    AppConfig.StartDate = startDate;
                    //

                    Set(startDate, endDate);
                }
                else
                {
                    CoursesController coursesController = new CoursesController(new CourseContext());
                    DateTime startDate2 = coursesController.FindDataToAdd(startDate).AddDays(1);

                    Uri uri = new Uri(baseUri.ToString() + String.Format("?startPeriod={0}&endPeriod={1}&format=csvdata", startDate2.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));

                    if (DateTime.Compare(startDate2, endDate) >= 0 || GetCourseFromCSV(uri).Count == 0)
                    {
                        SetFromDb(startDate, endDate);
                    }
                    else
                    {
                        Set(startDate, endDate);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ExchangeValues err");
                Console.WriteLine(e.Message);
            }
        }


        private static DateTime SetWhenNothing(DateTime startDate)
        {
            Uri uri = new Uri(baseUri.ToString() + String.Format("?startPeriod={0}&endPeriod={1}&format=csvdata", startDate.ToString("yyyy-MM-dd"), AppConfig.EndData.ToString("yyyy-MM-dd")));

            if(DateTime.Compare(startDate, DateTime.Now.Date) > 0)
            {
                return SetWhenNothing(startDate.AddDays(-1));
            }

            ExchangeValues ex = new ExchangeValues();
            HashSet<AppCourse> list = ex.GetCourseFromCSV(uri);

            if(list.Count == 0)
            {
                return SetWhenNothing(startDate.AddDays(-1));
            }
            else
            {
                return startDate;
            }
        }

        public void Set(DateTime startDate, DateTime endDate)
        {
            try
            {
                Uri uri = new Uri(baseUri.ToString() + String.Format("?startPeriod={0}&endPeriod={1}&format=csvdata", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));

                HashSet<AppCourse> list = GetCourseFromCSV(uri);

                if(list.Count() != 0)
                {
                    foreach (AppCourse course in list)
                    {
                        courses.Add(course);
                    }

                    CoursesController coursesController = new CoursesController(new CourseContext());
                    coursesController.SetCoursesInDb(list);

                    relaseDate = endDate;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Set err");
                Console.WriteLine(e.Message);
            }
        }

        public void SetFromDb(DateTime startDate, DateTime endDate)
        {
            try
            {
                CoursesController coursesController = new CoursesController(new CourseContext());

                foreach (AppCourse course in coursesController.GetCourses(startDate, endDate))
                {
                    courses.Add(course);
                }

                relaseDate = endDate;
            }
            catch(Exception e)
            {
                Console.WriteLine("SetFromDb err");
                Console.WriteLine(e.Message);
            }
        }

        public HashSet<AppCourse> Get(Dictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, string apiKey)
        {
            if (DateTime.Compare(startDate, endDate) <= 0  && String.Compare(apiKey, AppConfig.SecretKey) == 0)
            {
                List<AppCourse> res = new List<AppCourse>();

                if (DateTime.Compare(startDate, DateTime.Now) <= 0)
                {
                    if (DateTime.Compare(startDate, AppConfig.StartDate) < 0 && DateTime.Compare(endDate, AppConfig.StartDate) < 0)
                    {
                        throw new ArgumentOutOfRangeException("404");
                    }
                    else
                    {
                        try
                        {
                            CoursesController coursesController = new CoursesController(new CourseContext());

                            DateTime currentDate = DateTime.Now.Date;

                            if (DateTime.Compare(relaseDate, currentDate) != 0)
                            {
                                Uri uri = new Uri(baseUri.ToString() + String.Format("?startPeriod={0}&endPeriod={1}&format=csvdata", relaseDate.AddDays(1).ToString("yyyy-MM-dd"), currentDate.ToString("yyyy-MM-dd")));

                                if (DateTime.Compare(currentDate, relaseDate) > 0 && GetCourseFromCSV(uri).Count != 0)
                                {
                                    Set(relaseDate.AddDays(1), currentDate);
                                }
                            }

                            startDate = StartDate(startDate);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Get err");
                            Console.WriteLine(e.Message);
                        }
                    }

                    foreach (KeyValuePair<string, string> codesPair in currencyCodes)
                    {
                        res.AddRange(courses.Where(el => el.From == codesPair.Key.ToUpper() && el.To == codesPair.Value.ToUpper() && el.Date >= startDate && el.Date <= endDate));
                    }

                    return res.ToHashSet();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("404");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("404");
            }
            
        }

        private static DateTime StartDate(DateTime startDate)
        {
            if(DateTime.Compare(startDate, AppConfig.StartDate) <= 0)
            {
                return startDate;
            }
            else if (courses.Where(el => el.Date == startDate).Count() == 0)
            {
                return StartDate(startDate.AddDays(-1));
            }
            else
            {
                return startDate;
            }
        }
    }
}
