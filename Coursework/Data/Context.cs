using Coursework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.Data
{
    public class Context : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=Coursework");
        }
    }
}
