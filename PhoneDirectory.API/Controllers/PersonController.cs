using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Business.Services;
using PhoneDirectory.Common.Dto;
using PhoneDirectory.Common.Helper;
using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PhoneDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly  IPersonService _personService;
        public PersonController(IPersonService personService)
        {

            _personService = personService;
        }
        // GET: api/<PersonController>
        [HttpGet("GetPerson/{id}")]
        public async Task<Response<PersonDto>> Get(int id)
        {
            Person person = await _personService.GetPersonById(id);
            if(person == null)
                return new Response<PersonDto>() { Data = null, List = null, Message = "Kullanıcı bulunamadı", Status = 200 };

            PersonDto dto = new PersonDto()
            {
                CreatedDate = person.CreatedDate,
                Name = person.Name,
                Surname = person.Surname,
                Status = person.Status,
              
            };

            return new Response<PersonDto>() { Data = dto, List = null, Message = "Success", Status = 200 };

        }

       
    }
}
