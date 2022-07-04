using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleService
{
    class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CPU-0191; Initial Catalog=config;Trusted_Connection=True");
        }
        public DbSet<UserClass>  UserPath { get; set; }
    }
}
