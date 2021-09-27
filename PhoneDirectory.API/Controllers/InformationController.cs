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
            if (informationListFromCache != null && informationListFromCache.Length > 0)
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
            if (id <= 0)
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Value of id is invalid", Status = 500 };

            var serviceResult = await _informationService.GetInformationById(id);

            if (serviceResult.isSuccess)
                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }

        [HttpPost("CreateInformation")]
        public async Task<Response<InformationDto>> CreateInformation([FromBody] CreateInformationDto dto)
        {
            RegexHelper helper = new RegexHelper();
            if (String.IsNullOrEmpty(dto.Location) || String.IsNullOrEmpty(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Location or phone cannot be empty!", Status = 500 };

            if (!helper.IsValidMail(dto.Email))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid", Status = 500 };

            if (!helper.IsPhoneNumber(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Phone format is not valid", Status = 500 };



            var serviceResult = await _informationService.CreateInformation(dto);

            if (serviceResult.isSuccess)
            {
                List<InformationDto> listDto = new List<InformationDto>();
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
                if (informationListFromCache != null && informationListFromCache.Length > 0)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(informationListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<InformationDto>>(cacheJsonItem);

                }
                #endregion


                #region Redis update
                try
                {
                    if (informationListFromCache != null)
                    {
                        listDto.Add(serviceResult.Data);

                        informationListFromCache = null;

                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        informationListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Informations", informationListFromCache, options);
                    }
                }
                catch (Exception ex)
                {


                }
                #endregion

                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }

        [HttpDelete("DeleteInformation/{id}")]
        public async Task<Response<InformationDto>> DeleteInformation(int id)
        {
            if (id <= 0)
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Value of id is invalid", Status = 500 };

            var serviceResult = await _informationService.DeleteInformation(id);

            if (serviceResult.isSuccess)
            {
                List<InformationDto> listDto = new List<InformationDto>();
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
                if (informationListFromCache != null && informationListFromCache.Length > 0)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(informationListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<InformationDto>>(cacheJsonItem);

                }
                #endregion



                #region Redis update
                try
                {
                    if (informationListFromCache != null)
                    {
                        listDto.RemoveAll(x => x.Id == serviceResult.Data.Id);
                        informationListFromCache = null;
                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        informationListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Informations", informationListFromCache, options);
                    }
                }
                catch (Exception)
                {


                }
                #endregion
                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = "Success", Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
        }


        [HttpPut("UpdateInformation")]
        public async Task<Response<InformationDto>> UpdateInformation([FromBody] UpdateInformationDto dto)
        {
            RegexHelper helper = new RegexHelper();

            if (String.IsNullOrEmpty(dto.Location) || String.IsNullOrEmpty(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Location or phone cannot be empty", Status = 500 };


             if (!helper.IsValidMail(dto.Email))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Email address is not valid", Status = 500 };

            if (!helper.IsPhoneNumber(dto.Phone))
                return new Response<InformationDto>() { isSuccess = false, Data = null, List = null, Message = "Phone format is not valid", Status = 500 };

            var serviceResult = await _informationService.UpdateInformation(dto);

            if (serviceResult.isSuccess)
            {
                List<InformationDto> listDto = new List<InformationDto>();
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
                if (informationListFromCache != null && informationListFromCache.Length > 0)
                {
                    cacheJsonItem = Encoding.UTF8.GetString(informationListFromCache);

                    listDto = JsonConvert.DeserializeObject<List<InformationDto>>(cacheJsonItem);

                }
                #endregion


                #region Redis update
                try
                {

                    if (informationListFromCache != null)
                    {
                        foreach (var item in listDto.Where(x => x.Id == dto.Id))  // updating redis cache
                        {
                            item.Detail = dto.Detail;
                            item.Email = dto.Email;
                            item.Location = dto.Location;
                            item.Phone = dto.Phone;
                        }
                        informationListFromCache = null;
                        cacheJsonItem = JsonConvert.SerializeObject(listDto);
                        informationListFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                        var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(10));
                        await _redisDistributedCache.SetAsync("Informations", informationListFromCache, options);
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                return new Response<InformationDto>() { isSuccess = true, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };
            }

            else
                return new Response<InformationDto>() { isSuccess = false, Data = serviceResult.Data, List = null, Message = serviceResult.Message, Status = serviceResult.Status };


        }


        [HttpGet("LocationInformationReport")]
        public async Task<Response<LocationInformationDto>> LocationInformationReport()
        {

            var serviceResult = await _informationService.LocationInformationReport();
            
            if(serviceResult.isSuccess)
                return new Response<LocationInformationDto>() { isSuccess = true, Data = null, List = serviceResult.List, Message = serviceResult.Message, Status = serviceResult.Status };

            else
                return new Response<LocationInformationDto>() { isSuccess = false, Data = null, List = null, Message = serviceResult.Message, Status = serviceResult.Status };

        }
    }
}
