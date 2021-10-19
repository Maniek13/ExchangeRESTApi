using ExchangeRESTApi.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRESTApi.BaseClasses
{
    public class BaseExchangeValues
    {
        private protected static DateTime relaseDate = AppConfig.EndData;
        private protected static HashSet<AppCourse> courses = new HashSet<AppCourse>();
        private protected readonly static Uri baseUri = new Uri("https://sdw-wsrest.ecb.europa.eu/service/data/EXR/D...SP00.A");

        internal HashSet<AppCourse> GetCourseFromCSV(Uri uri)
        {
            HashSet<AppCourse> courses = new HashSet<AppCourse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            using (WebResponse response = request.GetResponse())
            {
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);

                List<string> lines = new List<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                var csv = from l in lines
                          select (l.Split(',')).ToArray();

                List<string[]> list = csv.ToList();

                if (list.Count() != 0)
                {
                    list.RemoveAt(0);

                    list.ForEach(delegate (string[] el)
                    {
                        courses.Add(new AppCourse { Date = Convert.ToDateTime(el[6]), From = el[3], To = el[2], Rate = Convert.ToDouble(el[7].Replace(".", ",")) });
                    });
                }
            }

            return courses;
        }

    }
}
