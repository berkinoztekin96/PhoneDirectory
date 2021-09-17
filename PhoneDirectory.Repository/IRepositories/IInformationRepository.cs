using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Repository.IRepositories
{
    public interface IInformationRepository : IRepository<Information>
    {

        Task<Information> GetInformationByUserId(int personId);

        Task<bool> DeleteInformation(int id);
    }
}
