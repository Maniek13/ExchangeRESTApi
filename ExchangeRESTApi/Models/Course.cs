using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRESTApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public double Rate { get; set; }
    }
}
