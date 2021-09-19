using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PhoneDirectory.Business.IServices;
using PhoneDirectory.Business.Services;
using PhoneDirectory.Common.Dto.Information;
using PhoneDirectory.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IInformationService _informationService;
        private readonly IDistributedCache _redisDistributedCache;
        public InformationController(IInformationService informationService, IPersonService personService, IDistributedCache distributedCache)
        {

            _personService = personService;
            _informationService = informationService;
            _redisDistributedCache = distributedCache;
        }

        [HttpGet("GetInformationList")]
        public async Task<Response<InformationDto>> GetInformationList()
        {

            #region Redis control


            byte[] informationListFromCache = null;
            string cacheJsonItem;

            try
            {
                informationListFromCache = await _redisDistributedCache.GetAsync("Informations");
            }
            catch (Exception ex)
            {
                informationListFromCache = null;
            }
            if (informationListFromCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(informationListFromCache);

                var listDto = JsonConvert.DeserializeObject<List<InformationDto>>(cacheJsonItem);
                return new Response<InformationDto>() { isSuccess = true, Data = null, List = listDto, Message = "Success", Status = 200 };
            }
            #endregion
            else
            {
                var serviceResult = await _informationService.GetAllInformations();


                if (serviceResult.isSuccess)
                {
                    #region Redis update
                    try
                    {

                        cacheJsonItem = JsonConvert.SerializeObject(serviceResult.List);
                        informationListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Informations", informationListFromCache, options);
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = serviceResult.List, Message = "Success", Status = serviceResult.Status };
                }
                else
                    return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
            }
        }


        [HttpGet("GetInformation/{id}")]
        public async Task<Response<InformationDto>> Get(int id)
        {

            var serviceResult = await _informationService.GetInformationById(id);

            if (serviceResult.isSuccess)
                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }

        [HttpPost("CreateInformation")]
        public async Task<Response<InformationDto>> CreatePerson(CreateInformationDto dto)
        {
            RegexHelper helper = new RegexHelper();
            if (String.IsNullOrEmpty(dto.Location) && String.IsNullOrEmpty(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Location or phone cannot be empty!", Status = 200 };

            if (!helper.IsValidMail(dto.Email))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid ", Status = 200 };



            var serviceResult = await _informationService.CreateInformation(dto);

            if (serviceResult.isSuccess)
            {
                #region Redis update
                byte[] personListFromCache = null;
                string cacheJsonItem;

                serviceResult.List = new List<InformationDto>();  //Added dto to list because in redis, I hold the type of serviceResult.List in redis
                serviceResult.List.Add(serviceResult.Data);

                cacheJsonItem = JsonConvert.SerializeObject(serviceResult.List);
                personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1))
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                await _redisDistributedCache.SetAsync("Informations", personListFromCache, options);
                #endregion

                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }

        [HttpDelete("DeleteInformation/{id}")]
        public async Task<Response<InformationDto>> DeletePerson(int id)
        {

            var serviceResult = await _informationService.DeleteInformation(id);

            if (serviceResult.isSuccess)
            {
                #region Redis update
                byte[] personListFromCache = null;
                string cacheJsonItem;


                serviceResult.List = new List<InformationDto>();  //Added dto to list because in redis, I hold the type of serviceResult.List in redis
                serviceResult.List.Add(serviceResult.Data);

                cacheJsonItem = JsonConvert.SerializeObject(serviceResult.List);
                personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1))
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                await _redisDistributedCache.SetAsync("Informations", personListFromCache, options);
                #endregion

                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }


        [HttpPut("UpdateInformation")]
        public async Task<Response<InformationDto>> UpdatePerson([FromBody] UpdateInformationDto dto)
        {
            RegexHelper helper = new RegexHelper();

            if (String.IsNullOrEmpty(dto.Location) && String.IsNullOrEmpty(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Location or phone cannot be empty", Status = 500 };


            else if (!helper.IsValidMail(dto.Email))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid ", Status = 200 };

            var serviceResult = await _informationService.UpdateInformation(dto);

            if (serviceResult.isSuccess)
            {
                #region Redis update
                byte[] personListFromCache = null;
                string cacheJsonItem;


                serviceResult.List = new List<InformationDto>();  //Added dto to list because in redis, I hold the type of serviceResult.List in redis
                serviceResult.List.Add(serviceResult.Data);

                cacheJsonItem = JsonConvert.SerializeObject(serviceResult.List);
                personListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1))
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                await _redisDistributedCache.SetAsync("Informations", personListFromCache, options);
                #endregion

                return new Response<InformationDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = "Name or surname cannot be empty", Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }

    }
}
