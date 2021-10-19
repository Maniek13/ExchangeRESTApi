using ExchangeRESTApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRESTApi.Data
{
    public class CourseContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ExchangeRestAppDB;Trusted_Connection=True;");
        }

    }
}