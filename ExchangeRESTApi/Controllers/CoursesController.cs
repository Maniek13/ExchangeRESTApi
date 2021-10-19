using ExchangeRESTApi.Data;
using ExchangeRESTApi.Interfaces;
using ExchangeRESTApi.Models;
using ExchangeRESTApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRESTApi.Controllers
{
    public class CoursesController : ICoursesController
    {
        private readonly CourseContext _context;

        public CoursesController(CourseContext context)
        {
            _context = context;
        }

        public void SetCoursesInDb(HashSet<AppCourse> courses)
        {
            try
            {
                var list = new HashSet<Course>();

                foreach (AppCourse appCourse in courses)
                {
                    list.Add(new Course { Date = appCourse.Date, From = appCourse.From, To = appCourse.To, Rate = appCourse.Rate });
                }

                _context.AddRangeAsync(list);
                _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new ContextMarshalException(e.Message);
            }
        }

        public DateTime FindDataToAdd(DateTime startDate)
        {
            try
            {
                var date = _context.Courses.OrderBy(el => el.Date).LastOrDefault();

                if(date != null)
                {
                    return date.Date; 
                }
                else
                {
                    return startDate;
                }
            }
            catch (Exception e)
            {
                throw new ContextMarshalException(e.Message);
            }
        }

        public HashSet<AppCourse> GetCourses(DateTime startDate, DateTime endDate)
        {
            try
            {
                HashSet<AppCourse> coursesToSend = new HashSet<AppCourse>();
                var courses = _context.Courses.Where(el => el.Date >= startDate && el.Date <= endDate).ToList();

                courses.ForEach(delegate(Models.Course course)
                {
                    coursesToSend.Add(new AppCourse { Date = course.Date, From = course.From, To = course.To, Rate = course.Rate });
                });

                return coursesToSend;
            }
            catch (Exception e)
            {
                throw new ContextMarshalException(e.Message);
            }
        }


        
    }
}


