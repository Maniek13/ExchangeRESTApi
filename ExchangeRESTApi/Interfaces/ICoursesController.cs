using ExchangeRESTApi.Models;
using ExchangeRESTApi.Objects;
using System;
using System.Collections.Generic;

namespace ExchangeRESTApi.Interfaces
{
    interface ICoursesController
    {
        public void SetCoursesInDb(HashSet<AppCourse> courses);
        public DateTime FindDataToAdd(DateTime startDate);
        public HashSet<AppCourse> GetCourses(DateTime startDate, DateTime endDate);
    }
}
