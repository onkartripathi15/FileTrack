using MailService.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MailService.WebApi.DataContext
{
    class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CPU-0191; Initial Catalog=config;Trusted_Connection=True");
        }
   
    }
}
