using PhoneDirectory.Entities.Entities;
using PhoneDirectory.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PhoneDirectory.Repository.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(PhoneDirectoryDbContext context)
            : base(context)
        {
        }


    }
}
