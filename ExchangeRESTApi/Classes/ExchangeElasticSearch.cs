using ExchangeRESTApi.BaseClasses;
using ExchangeRESTApi.Controllers;
using ExchangeRESTApi.Data;
using ExchangeRESTApi.Interfaces;
using ExchangeRESTApi.Objects;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRESTApi.Classes
{
    public class ExchangeElasticSearch : BaseExchangeValues, IExchangeValues
    {
        public ExchangeElasticSearch()
        {
        }

        internal ExchangeElasticSearch(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (DateTime.Compare(startDate, endDate) == 0)
                {
                    //set data when start have no data
                    startDate = SetWhenNothing(startDate);
                    AppConfig.StartDate = startDate;
                    //
                    ElasticClient elasticClient = new ElasticClient();
                    var response = elasticClient.Search<AppCourse>(s => s
                        .Index(AppConfig.Index)
                        .Query(q1 => q1
                         .DateRange(r => r
                            .Field(f => f.Date)
                            .GreaterThan(startDate))
                       ));
                    
                    if(response.Documents.Count <= 0)
                    {
                        Set(startDate, endDate);
                    }
                    
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
        
        public void Set(DateTime startDate, DateTime endDate)
        {
            try
            {
                Uri uri = new Uri(baseUri.ToString() + String.Format("?startPeriod={0}&endPeriod={1}&format=csvdata", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));

                HashSet<AppCourse> list = GetCourseFromCSV(uri);

                if (list.Count() != 0)
                {
                    foreach (AppCourse course in list)
                    {
                        courses.Add(course);
                    }

                    ElasticClient elasticClient = new ElasticClient();
                    var result = elasticClient.IndexMany(courses, AppConfig.Index);
                    if (result.Errors)
                    {
                        foreach (var itemWithError in result.ItemsWithErrors)
                        {
                            Console.WriteLine("Failed to index document {0}: {1}",
                                itemWithError.Id, itemWithError.Error);
                        }
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
                relaseDate = endDate;
            }
            catch (Exception e)
            {
                Console.WriteLine("SetFromDb err");
                Console.WriteLine(e.Message);
            }
        }

        public HashSet<AppCourse> Get(Dictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, string apiKey)
        {
            if (DateTime.Compare(startDate, endDate) <= 0 && String.Compare(apiKey, AppConfig.SecretKey) == 0)
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
                    res.AddRange(Find(currencyCodes, startDate, endDate));

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

        public List<AppCourse> Find(Dictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate)
        {
            List<AppCourse> res = new List<AppCourse>();

            foreach (KeyValuePair<string, string> codesPair in currencyCodes)
            {
                ElasticClient elasticClient = new ElasticClient();

                var response = elasticClient.Search<AppCourse>(s => s
                     .Index(AppConfig.Index)
                     .Query(q1 => q1
                         .DateRange(r => r
                            .Field(f => f.Date)
                            .GreaterThanOrEquals(startDate)
                            .LessThanOrEquals(endDate))
                     )
                     .Query(q => q
                        .Match(m => m
                            .Field(f => f.From)
                            .Query(codesPair.Key))
                    )
                    .Query(q => q
                        .Match(m => m
                            .Field(f => f.To)
                            .Query(codesPair.Value))
                    ));

                if (!response.IsValid)
                {
                    Console.WriteLine("Failed to search documents");
                }
                res.AddRange(response.Documents);
            }
            return res;
        }
    }
}
