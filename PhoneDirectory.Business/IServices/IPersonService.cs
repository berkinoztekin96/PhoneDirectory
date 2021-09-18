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

        Task<Response<PersonDto>> GetAllPersons();
        Task<Response<PersonDto>> GetPersonById(int id);
        Task<Response<PersonDto>> CreatePerson(CreatePersonDto dto);
        Task<Response<PersonDto>> UpdatePerson(UpdatePersonDto dto);
        Task<Response<PersonDto>> DeletePerson(int id);
    }
}
