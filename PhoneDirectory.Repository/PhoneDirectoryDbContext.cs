using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Repository
{
    public class PhoneDirectoryDbContext : DbContext
    {
        public PhoneDirectoryDbContext(DbContextOptions<PhoneDirectoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Information> Informations{ get; set; }
    }
}
