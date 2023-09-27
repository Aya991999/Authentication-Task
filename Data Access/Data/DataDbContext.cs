using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Data
{
    public class DataDbContext:DbContext
    {
        public DataDbContext()
        {
        }
        public DataDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
        public virtual DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Default");
            }
        }

    }
}
