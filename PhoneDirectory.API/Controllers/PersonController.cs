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

            var serviceResult = await _personService.GetPersonById(id);

            if (serviceResult.isSuccess)
                return new Response<PersonDto>() { Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };

            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }


        [HttpPost("CreatePerson")]
        public async Task<Response<PersonDto>> CreatePerson(CreatePersonDto dto)
        {
            RegexHelper helper = new RegexHelper();
            if (String.IsNullOrEmpty(dto.Name) && String.IsNullOrEmpty(dto.Surname))
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Name or surname cannot be empty!", Status = 200 };

            else if (!helper.IsValidMail(dto.Email))
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid ", Status = 200 };

            var serviceResult = await _personService.CreatePerson(dto);

            if (serviceResult.isSuccess)
            {

                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }


        [HttpGet("GetPersonList")]
        public async Task<Response<PersonDto>> GetPersonList()
        {

            var serviceResult = await _personService.GetAllPersons();

            if (serviceResult.isSuccess)
                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = serviceResult.List, Message = "Success", Status = serviceResult.Status };
            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }

        [HttpDelete("DeletePerson/{id}")]
        public async Task<Response<PersonDto>> DeletePerson(int id)
        {

            var serviceResult = await _personService.DeletePerson(id);

            if (serviceResult.isSuccess)
                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };


            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }


        [HttpPut("UpdatePerson")]
        public async Task<Response<PersonDto>> UpdatePerson([FromBody] UpdatePersonDto dto)
        {

            if (String.IsNullOrEmpty(dto.Name) && String.IsNullOrEmpty(dto.Surname))
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Name or surname cannot be empty", Status = 500 };

            var serviceResult = await _personService.UpdatePerson(dto);

            if (serviceResult.isSuccess)
                return new Response<PersonDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = "Name or surname cannot be empty", Status = serviceResult.Status };

            else
                return new Response<PersonDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }
    }
}
