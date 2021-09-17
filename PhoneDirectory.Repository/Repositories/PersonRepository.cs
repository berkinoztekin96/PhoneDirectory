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
        private readonly PhoneDirectoryDbContext dbContext;
        public PersonRepository(PhoneDirectoryDbContext context)
            : base(context)
        {
        }

     

       public async Task DeletePerson(int id)
        {
            try
            {
                Person person = await GetByIdAsync(id);

                if (person != null)
                    person.Status = 0;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
