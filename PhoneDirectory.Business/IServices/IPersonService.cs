using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Business.Services
{
    public interface IPersonService
    {

        Task<IQueryable<Person>> GetAllPersons();
        Task<Person> GetPersonById(int id);
        Task<Person> CreatePerson(Person person);
        Task<Person> UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
