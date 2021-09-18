using PhoneDirectory.Common.Dto.Person;
using PhoneDirectory.Common.Helper;
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

        Task<List<Person>> GetAllPersons();
        Task<Person> GetPersonById(int id);
        Task<Response<Person>> CreatePerson(CreatePersonDto dto);
        Task<Person> UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
