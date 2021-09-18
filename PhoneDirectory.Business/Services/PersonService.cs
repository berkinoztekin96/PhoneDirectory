using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Common.Dto.Information;
using PhoneDirectory.Common.Dto.Person;
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
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IInformationRepository _informationRepository;
        private readonly PhoneDirectoryDbContext dbContext;
        public PersonService(IPersonRepository personRepository, IInformationRepository informationRepository, PhoneDirectoryDbContext phoneDirectoryDbContext)
        {
            _personRepository = personRepository;
            _informationRepository = informationRepository;
            dbContext = phoneDirectoryDbContext;
        }


        public async Task<Response<PersonDto>> CreatePerson(CreatePersonDto dto)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Person person = new Person()
                    {
                        CreatedDate = DateTime.Now,
                        Name = dto.Name,
                        Surname = dto.Surname,
                        Status = 1
                    };

                    await _personRepository.CreateAsync(person);

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
                        CreatedDate = information.CreatedDate,
                        Detail = information.Detail,
                        Email = information.Email,
                        Location = information.Location,
                        PersonId = information.PersonId,
                        Phone = information.Phone,
                        Status = information.Status
                    };

                    var informationList = new List<InformationDto>();
                    informationList.Add(informationDto);
                    PersonDto returnDto = new PersonDto()
                    {
                        Name = person.Name,
                        Surname = person.Surname,
                        Information = informationList,
                        CreatedDate = person.CreatedDate,
                        Status = person.Status
                    };


                    return new Response<PersonDto>() { isSuccess = true, Data = returnDto, List = null, Message = "Success", Status = 200 };
                }



                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
                }
            }

        }

        public async Task<Response<Person>> DeletePerson(int id) // todo: bak
        {
            try
            {

                Person person = _personRepository.FindBy(x => x.Id == id, x => x.Information).FirstOrDefault();
                if (person == null)
                    return new Response<Person>() { isSuccess = false, Data = person, List = null, Message = "Person could not found", Status = 200 };


                person.Status = 0;

                foreach (var item in person.Information)
                {

                    item.Status = 0;


                }




                await dbContext.SaveChangesAsync();

                return new Response<Person>() { isSuccess = true, Data = person, List = null, Message = "Success", Status = 200 };
            }
            catch (Exception ex)
            {

                return new Response<Person>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Response<PersonDto>> GetAllPersons()
        {
            try
            {
                List<PersonDto> listDto = new List<PersonDto>();
                var personList = await _personRepository.FindBy(x => x.Status == 1, x => x.Information).ToListAsync();

                foreach (var person in personList)
                {
                    PersonDto personDto = new PersonDto()
                    {
                        CreatedDate = person.CreatedDate,
                        Information = new List<InformationDto>(),
                        Name = person.Name,
                        Surname = person.Surname,
                        Status = person.Status
                    };

                    foreach (var item in person.Information)
                    {
                        InformationDto informationDto = new InformationDto()
                        {
                            CreatedDate = item.CreatedDate,
                            Detail = item.Detail,
                            Email = item.Email,
                            Location = item.Location,
                            PersonId = item.PersonId,
                            Phone = item.Phone,
                            Status = item.Status,
                        };
                       personDto.Information.Add(informationDto);
                    }

                    listDto.Add(personDto);
                }

                return new Response<PersonDto>() { isSuccess = true, Data = null, List = listDto, Message = "Success", Status = 200 };

            }
            catch (Exception ex)
            {

                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Response<PersonDto>> GetPersonById(int id)
        {
            try
            {
                if (id <= 0)
                    return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Id is not valid", Status = 500 };




                Person person = await _personRepository.FindBy(x => x.Id == id, x => x.Information).FirstOrDefaultAsync();
                if (person != null)
                {
                    PersonDto personDto = new PersonDto()
                    {
                        CreatedDate = person.CreatedDate,
                        Name = person.Name,
                        Surname = person.Surname,
                        Status = person.Status,
                        Information = new List<InformationDto>(),
                    };


                    foreach (var item in person.Information)
                    {
                        InformationDto informationDto = new InformationDto()
                        {
                            CreatedDate = item.CreatedDate,
                            Detail = item.Detail,
                            Email = item.Email,
                            Location = item.Location,
                            PersonId = item.PersonId,
                            Phone = item.Phone,
                            Status = item.Status
                        };
                        personDto.Information.Add(informationDto);
                    }
                    return new Response<PersonDto>() { isSuccess = true, Data = personDto, List = null, Message = "No user found", Status = 200 };
                }

                else
                    return new Response<PersonDto>() { isSuccess = true, Data = null, List = null, Message = "No user found", Status = 200 };


            }
            catch (Exception ex)
            {

                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
            }
        }

        public async Task<Person> UpdatePerson(Person person)
        {

            try
            {
                await _personRepository.UpdateAsync(person);
                return person;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }


    }
}
