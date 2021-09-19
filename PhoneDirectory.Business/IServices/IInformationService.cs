using PhoneDirectory.Common.Dto.Information;
using PhoneDirectory.Common.Helper;
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
        Task<Response<InformationDto>> GetAllInformations();
        Task<Response<InformationDto>> GetInformationById(int id);
        Task<Response<InformationDto>> CreateInformation(CreateInformationDto dto);
        Task<Response<InformationDto>> UpdateInformation(UpdateInformationDto dto);
        Task<Response<InformationDto>> DeleteInformation(int id);
    }
}
