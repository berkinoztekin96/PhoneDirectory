using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Repository.IRepositories
{
    public interface IPersonRepository : IRepository<Person>
    {

        Task DeletePerson(int id);
    }
}
