using Microsoft.EntityFrameworkCore;
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
    }
}
