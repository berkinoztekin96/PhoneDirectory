using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Business.IServices;
using PhoneDirectory.Entities.Entities;
using PhoneDirectory.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Business.Services
{
    public class InformationService : IInformationService
    {
        private readonly IInformationRepository _informationRepository;
        public InformationService(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }
        public async Task<Information> CreateInformation(Information Information)
        {
            try
            {
                var result = await _informationRepository.CreateAsync(Information);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task DeleteInformation(int id)
        {
            try
            {
                await _informationRepository.DeleteInformation(id);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<Information> GetAllInformations()
        {
            try
            {
                return _informationRepository.GetAllAsync().ToList();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Information> GetInformationById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Bir hata oluştu!");

                Information Information = await _informationRepository.FindBy(x=> x.Id == id).FirstOrDefaultAsync();
                return Information;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Information> UpdateInformation(Information Information)
        {

            try
            {
                await _informationRepository.UpdateAsync(Information);
                return Information;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

      
    }
}
