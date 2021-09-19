using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Business.IServices;
using PhoneDirectory.Common.Dto.Information;
using PhoneDirectory.Common.Helper;
using PhoneDirectory.Entities.Entities;
using PhoneDirectory.Repository;
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
        private readonly IPersonRepository _personRepository;
        private readonly IInformationRepository _informationRepository;
        private readonly PhoneDirectoryDbContext dbContext;
        public InformationService(IInformationRepository informationRepository, IPersonRepository personRepository, PhoneDirectoryDbContext phoneDirectoryDbContext)
        {
            _informationRepository = informationRepository;
            _personRepository = personRepository;
            dbContext = phoneDirectoryDbContext;
        }

        public async Task<Response<InformationDto>> CreateInformation(CreateInformationDto dto)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Person person = await _personRepository.FindBy(x => x.Id == dto.PersonId).FirstOrDefaultAsync();

                    if (person == null)
                        return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Person could not found for this information", Status = 500 };

                    Information information = new Information()
                    {
                        CreatedDate = DateTime.Now,
                        Detail = dto.Detail,
                        Email = dto.Email,
                        Location = dto.Location,
                        Phone = dto.Phone,
                        Status = 1,
                        PersonId = person.Id
                    };

                    await _informationRepository.CreateAsync(information);

                    transaction.Commit();


                    InformationDto informationDto = new InformationDto()
                    {
                        PersonName = person.Name,
                        PersonSurname = person.Surname,
                        PersonId = person.Id,
                        CreatedDate = information.CreatedDate,
                        Detail = information.Detail,
                        Email = information.Email,
                        Location = information.Location,
                        Phone = information.Phone,
                        Status = information.Status
                    };

                    return new Response<InformationDto>() { isSuccess = true, Data = informationDto, List = null, Message = "Success", Status = 200 };
                }



                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
                }
            }
        }

        public async Task<Response<InformationDto>> DeleteInformation(int id)
        {
            if (id <= 0)
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Id is not valid", Status = 500 };

            try
            {
                Information information = await _informationRepository.FindBy(x=> x.Id == id && x.Status == 1).FirstOrDefaultAsync();

                if (information != null)
                {
                    information.Status = 0;
                    await dbContext.SaveChangesAsync();

                    Person person = await _personRepository.FindBy(x => x.Id == information.PersonId).FirstOrDefaultAsync();

                    if (person == null)
                        return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Person could not found", Status = 500 };

                    InformationDto informationDto = new InformationDto()
                    {
                        PersonName = person.Name,
                        PersonSurname = person.Surname,
                        CreatedDate = information.CreatedDate,
                        Detail = information.Detail,
                        Email = information.Email,
                        Location = information.Location,
                        PersonId = information.PersonId,
                        Phone = information.Phone,
                        Status = 0
                    };


                    return new Response<InformationDto>() { isSuccess = true, Data = informationDto, List = null, Message = "Success", Status = 200 };
                }
                else
                    return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Information could not found", Status = 500 };

            }
            catch (Exception ex)
            {

                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Response<InformationDto>> GetAllInformations()
        {
            try
            {
                List<InformationDto> listDto = new List<InformationDto>();
                var informationList = await _informationRepository.FindBy(x => x.Status == 1).ToListAsync();

                if (informationList.Count <= 0)
                    return new Response<InformationDto>() { isSuccess = true, Data = null, List = listDto, Message = "Success", Status = 200 };

                foreach (var item in informationList)
                {
                    if (item.Status == 1)
                    {
                        Person person = await _personRepository.FindBy(x => x.Id == item.PersonId).FirstOrDefaultAsync(); // if person is null, then catch part will be executed.
                        if (person.Status != 0)
                        {
                            InformationDto dto = new InformationDto()
                            {
                                PersonName = person.Name,
                                PersonSurname = person.Surname,
                                CreatedDate = item.CreatedDate,
                                Detail = item.Detail,
                                Email = item.Email,
                                Location = item.Location,
                                PersonId = item.PersonId,
                                Phone = item.Phone,
                                Status = item.Status
                            };

                            listDto.Add(dto);
                        }
                    }
                }

                return new Response<InformationDto>() { isSuccess = true, Data = null, List = listDto, Message = "Success", Status = 200 };

            }
            catch (Exception ex)
            {

                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Response<InformationDto>> GetInformationById(int id)
        {

            try
            {
                if (id <= 0)
                    return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Id is not valid", Status = 500 };

                Information information = await _informationRepository.FindBy(x => x.Id == id && x.Status == 1).FirstOrDefaultAsync();

                if (information != null)
                {
                    Person person = await _personRepository.FindBy(x => x.Id == information.PersonId).FirstOrDefaultAsync();
                    if (person == null)
                        return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Person could not found", Status = 500 };


                    InformationDto dto = new InformationDto()
                    {
                        PersonName = person.Name,
                        PersonSurname = person.Surname,
                        CreatedDate = information.CreatedDate,
                        Detail = information.Detail,
                        Email = information.Email,
                        Location = information.Location,
                        Phone = information.Phone,
                        Status = information.Status,
                        PersonId = information.PersonId
                    };


                    return new Response<InformationDto>() { isSuccess = true, Data = dto, List = null, Message = "Success", Status = 200 };
                }


                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Information could not found", Status = 500 };


            }
            catch (Exception ex)
            {

                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Response<InformationDto>> UpdateInformation(UpdateInformationDto dto)
        {
            try
            {
                Information information = await _informationRepository.FindBy(x => x.Id == dto.Id && x.Status == 1, x=> x.Person).FirstOrDefaultAsync();
                if (information == null)
                    return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Information could not found", Status = 200 };



                information.Detail = dto.Detail;
                information.Email = dto.Email;
                information.Location = dto.Location;
                information.Phone = dto.Phone;
                await dbContext.SaveChangesAsync();


                InformationDto resultDto = new InformationDto()
                {
                    PersonName = information.Person.Name,
                    PersonSurname = information.Person.Surname,
                    CreatedDate = information.CreatedDate,
                    Detail = information.Detail,
                    Email = information.Email,
                    Location = information.Location,
                    PersonId = information.Person.Id,
                    Phone = information.Phone,
                    Status = information.Status
                };




                return new Response<InformationDto>() { isSuccess = false, Data = resultDto, List = null, Message = "Success", Status = 200 };


            }
            catch (Exception ex)
            {

                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }


        }


    }

}

