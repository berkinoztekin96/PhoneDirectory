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

        public async Task<bool> DeletePerson(int id) // soft delete
        {
            try
            {
                Person person = await GetAllAsync().Where(x => x.Id == id).FirstOrDefaultAsync();

                if (person != null)
                {
                    person.Status = 0;
                    return true;
                }

                else
                    return false;


            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
