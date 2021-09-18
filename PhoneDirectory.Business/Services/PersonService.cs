using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Entities.Entities;
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

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        public async Task<Person> CreatePerson(Person person)
        {
            try
            {
                var result = await _personRepository.CreateAsync(person);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

        public List<Person> GetAllPersons()
        {
            try
            {
                return _personRepository.GetAllAsync().ToList();
               
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public  Task<Person> GetPersonById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Bir hata oluştu!");

                //Person person = await _personRepository.GetByIdAsync(id);
                //return person;

                Person person =  _personRepository.FindBy(x => x.Id == id, x=> x.Information).FirstOrDefault();

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
            catch(Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

       
    }
}
