using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Business.IServices
{
    public interface IInformationService
    {
        List<Information> GetAllInformations();
        Task<Information> GetInformationById(int id);
        Task<Information> CreateInformation(Information Information);
        Task<Information> UpdateInformation(Information Information);
        Task DeleteInformation(int id);
    }
}
