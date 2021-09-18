using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Business.Services;
using PhoneDirectory.Common.Dto;
using PhoneDirectory.Common.Dto.Person;
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
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {

            _personService = personService;
        }


        [HttpGet("GetPerson/{id}")]
        public async Task<Response<PersonDto>> Get(int id)
        {
            Person person = await _personService.GetPersonById(id);
            if (person == null)
                return new Response<PersonDto>() { Data = null, List = null, Message = "User could not find!", Status = 200 };

            PersonDto dto = new PersonDto()
            {
                CreatedDate = person.CreatedDate,
                Name = person.Name,
                Surname = person.Surname,
                Status = person.Status,            
            };

            return new Response<PersonDto>() {isSuccess = false, Data = dto, List = null, Message = "Success", Status = 200 };
        }


        [HttpPost("CreatePerson")]
        public async Task<Response<PersonDto>> CreatePerson(CreatePersonDto dto)
        {
            RegexHelper helper = new RegexHelper();
            if(String.IsNullOrEmpty(dto.Name) && String.IsNullOrEmpty(dto.Surname))
                return new Response<PersonDto>() {isSuccess = false, Data = null, List = null, Message = "Name or surname cannot be empty!", Status = 200 };

           else if(!helper.IsValidMail(dto.Email))
                return new Response<PersonDto>() {isSuccess = false, Data = null, List = null, Message = "Email address is not valid ", Status = 200 };

           var serviceResult = await _personService.CreatePerson(dto);

            if(serviceResult.isSuccess)
            {
                PersonDto resultData = new PersonDto()
                {
                    CreatedDate = serviceResult.Data.CreatedDate,
                    Name = serviceResult.Data.Name,
                    Surname = serviceResult.Data.Surname

                };
                return new Response<PersonDto>() { isSuccess = true, Data = resultData, List = null, Message = "Success", Status = serviceResult.Status };
            }
         
            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status};

        }


    }
}
