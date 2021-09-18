using Microsoft.EntityFrameworkCore;
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


        public async Task<Response<Person>> CreatePerson(CreatePersonDto dto)
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

                    return new Response<Person>() { isSuccess = true, Data = person, List = null, Message = "Success", Status = 200 };
                }



                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response<Person>() { isSuccess = false, Data = null, List = null, Message = ex.Message, Status = 500 };
                }
            }

        }

        public async Task DeletePerson(int id)
        {
            try
            {
                await _personRepository.DeletePerson(id);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Person>> GetAllPersons()
        {
            try
            {
                return await _personRepository.FindBy(x => x.Status == 1).ToListAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<Person> GetPersonById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Bir hata oluştu!");

                //Person person = await _personRepository.GetByIdAsync(id);
                //return person;

                Person person = _personRepository.FindBy(x => x.Id == id, x => x.Information).FirstOrDefault();

                return Task.FromResult(person);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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
