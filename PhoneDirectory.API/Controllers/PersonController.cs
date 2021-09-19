using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PhoneDirectory.Business.Services;
using PhoneDirectory.Common.Dto;
using PhoneDirectory.Common.Dto.Person;
using PhoneDirectory.Common.Helper;
using PhoneDirectory.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhoneDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IDistributedCache _redisDistributedCache;
        public PersonController(IPersonService personService, IDistributedCache distributedCache)
        {

            _personService = personService;
            _redisDistributedCache = distributedCache;
        }


        [HttpGet("GetPerson/{id}")]
        public async Task<Response<PersonDto>> Get(int id)
        {

            var serviceResult = await _personService.GetPersonById(id);

            if (serviceResult.isSuccess)
                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };

            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }


        [HttpPost("CreatePerson")]
        public async Task<Response<PersonDto>> CreatePerson(CreatePersonDto dto)
        {
            RegexHelper helper = new RegexHelper();
            if (String.IsNullOrEmpty(dto.Name) && String.IsNullOrEmpty(dto.Surname))
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Name or surname cannot be empty!", Status = 200 };

            if (!helper.IsValidMail(dto.Email))
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid ", Status = 200 };



            var serviceResult = await _personService.CreatePerson(dto);

            if (serviceResult.isSuccess)
            {
                List<PersonDto> listDto = new List<PersonDto>();
                #region Redis control


                byte[] personListFromCache = null;
                string cacheJsonItem;

                try
                {
                    personListFromCache = await _redisDistributedCache.GetAsync("Persons");
                }
                catch (Exception ex)
                {
                    personListFromCache = null;
                }
                if (personListFromCache != null)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(personListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<PersonDto>>(cacheJsonItem);

                }
                #endregion

                #region Redis update
                try
                {
                    if (personListFromCache != null)
                    {
                        listDto.Add(serviceResult.Data);

                        personListFromCache = null;

                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Persons", personListFromCache, options);
                    }
                }
                catch (Exception ex)
                {


                }
                #endregion

                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }


        [HttpGet("GetPersonList")]
        public async Task<Response<PersonDto>> GetPersonList()
        {

            #region Redis control


            byte[] personListFromCache = null;
            string cacheJsonItem;

            try
            {
                personListFromCache = await _redisDistributedCache.GetAsync("Persons");
            }
            catch (Exception ex)
            {
                personListFromCache = null;
            }
            if (personListFromCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(personListFromCache);

                var listDto = JsonConvert.DeserializeObject<List<PersonDto>>(cacheJsonItem);
                return new Response<PersonDto>() { isSuccess = true, Data = null, List = listDto, Message = "Success", Status = 200 };
            }
            #endregion
            else
            {
                var serviceResult = await _personService.GetAllPersons();


                if (serviceResult.isSuccess)
                {
                    #region Redis update
                    try
                    {

                        cacheJsonItem = JsonConvert.SerializeObject(serviceResult.List);
                        personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Persons", personListFromCache, options);
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = serviceResult.List, Message = "Success", Status = serviceResult.Status };
                }
                else
                    return new Response<PersonDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
            }
        }

        [HttpDelete("DeletePerson/{id}")]
        public async Task<Response<PersonDto>> DeletePerson(int id)
        {

            var serviceResult = await _personService.DeletePerson(id);

            if (serviceResult.isSuccess)
            {
                List<PersonDto> listDto = new List<PersonDto>();
                #region Redis control


                byte[] personListFromCache = null;
                string cacheJsonItem;

                try
                {
                    personListFromCache = await _redisDistributedCache.GetAsync("Persons");
                }
                catch (Exception ex)
                {
                    personListFromCache = null;
                }
                if (personListFromCache != null)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(personListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<PersonDto>>(cacheJsonItem);

                }
                #endregion




                #region Redis update
                try
                {
                    if (personListFromCache != null)
                    {
                        listDto.RemoveAll(x => x.Id == serviceResult.Data.Id);
                        personListFromCache = null;
                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Persons", personListFromCache, options);
                    }
                }
                catch (Exception)
                {


                }
                #endregion

                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

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
            {
                List<PersonDto> listDto = new List<PersonDto>();
                #region Redis control


                byte[] personListFromCache = null;
                string cacheJsonItem;

                try
                {
                    personListFromCache = await _redisDistributedCache.GetAsync("Persons");
                }
                catch (Exception ex)
                {
                    personListFromCache = null;
                }
                if (personListFromCache != null)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(personListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<PersonDto>>(cacheJsonItem);

                }
                #endregion

                #region Redis update
                try
                {

                    if (personListFromCache != null)
                    {
                        foreach (var item in listDto.Where(x => x.Id == dto.Id))  // updating redis cache
                        {
                            item.Name = dto.Name;
                            item.Surname = dto.Surname;
                            item.CompanyName = dto.CompanyName;

                            foreach (var info in item.Information)
                            {
                                info.PersonName = dto.Name;
                                info.PersonSurname = dto.Surname;
                            }
                        }

                        personListFromCache = null;
                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Persons", personListFromCache, options);
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                return new Response<PersonDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
            }

            else
                return new Response<PersonDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }
    }
}
