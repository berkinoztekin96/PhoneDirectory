using Microsoft.Extensions.Caching.Distributed;
using Moq;
using PhoneDirectory.API.Controllers;
using PhoneDirectory.Business.IServices;
using PhoneDirectory.Business.Services;
using PhoneDirectory.Common.Dto.Information;
using PhoneDirectory.Common.Dto.Person;
using PhoneDirectory.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneDirectory.Test.TestController
{
    public class InformationControllerTest
    {

        [Fact]
        public async void GetInformationListTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var informations = GetAllInformationFakeData();
            informationService.Setup(x => x.GetAllInformations()).Returns(informations);
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            // Act
            var results = await controller.GetInformationList();



            // Assert
            Assert.Equal(200, results.Status);
        }

        [Fact]
        public async void GetInformationByIdTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();

            var informations = await GetAllInformationFakeData();
            var firstInformation = GetInformationWithIdFakeData(1);



            informationService.Setup(x => x.GetInformationById(1)).Returns(firstInformation);
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
     

            // act
            var result = await controller.Get(1);


            // assert
            Assert.Equal(1, result.Data.Id);
        }


        [Fact]
        public async void GetInformationByZeroIdTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            // act
            var result = await controller.Get(0);

            // assert
            Assert.Equal(500, result.Status);
        }

        [Fact]
        public async void GetInformationByMinusIdTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            // act
            var result = await controller.Get(-1);

            // assert
            Assert.Equal(500, result.Status);
        }



        [Fact]
        public async void CreateInformationTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            var fakePersonDto = new CreateInformationDto()
            {
             Detail = "Test Information",
             Email = "test@mail.com",
             Location = "Test Information",
             PersonId = 5,
             Phone = "05323414131",
            };
            var response = CreateInformationFakeData(fakePersonDto);
            informationService.Setup(x => x.CreateInformation(fakePersonDto)).Returns(response);

            // Act
            var results = await controller.CreateInformation(fakePersonDto);



            // Assert
            Assert.Equal(200, results.Status);
        }


        [Fact]
        public async void CreateInformationWithNullValuesTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            var fakePersonDto = new CreateInformationDto()
            {
             
            };


            // Act
            var results = await controller.CreateInformation(fakePersonDto);



            // Assert
            Assert.Equal("Location or phone cannot be empty!", results.Message);
        }

        [Fact]
        public async void CreateInformationWithInvalidEmailTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            var fakePersonDto = new CreateInformationDto()
            {
                Detail = "Test Information",
                Email = "asdasd",
                Location = "Test Information",
                PersonId = 5,
                Phone = "05323414131",
            };


            // Act
            var results = await controller.CreateInformation(fakePersonDto);



            // Assert
            Assert.Equal("Email address is not valid", results.Message);
        }

        [Fact]
        public async void CreateInformationWithInvalidPhoneTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            var fakePersonDto = new CreateInformationDto()
            {
                Detail = "Test Information",
                Email = "test@mail.com",
                Location = "Test Information",
                PersonId = 5,
                Phone = "453452242",
            };


            // Act
            var results = await controller.CreateInformation(fakePersonDto);



            // Assert
            Assert.Equal("Phone format is not valid", results.Message);
        }


        [Fact]
        public async void UpdateInformationTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            UpdateInformationDto fakeUpdatePerson = new UpdateInformationDto()
            {
               Detail = "Updated detail test",
               Email = "test@mail.com",
               Id = 15,
               Location = "Updated location test",
               Phone = "05323414131"
            };
            var response = UpdateInformationFakeData(fakeUpdatePerson);
            informationService.Setup(x => x.UpdateInformation(fakeUpdatePerson)).Returns(response);

            // act
            var result = await controller.UpdateInformation(fakeUpdatePerson);

            // assert
            Assert.Equal(200, result.Status);

        }


        [Fact]
        public async void UpdateInformationWithNullValuesTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            UpdateInformationDto fakeUpdatePerson = new UpdateInformationDto()
            {
               
            };
 
            // act
            var result = await controller.UpdateInformation(fakeUpdatePerson);

            // assert
            Assert.Equal("Location or phone cannot be empty", result.Message);

        }

        [Fact]
        public async void UpdateInformationWithInvalidEmailTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            UpdateInformationDto fakeUpdatePerson = new UpdateInformationDto()
            {
                Detail = "Updated detail test",
                Email = "asdasd",
                Id = 15,
                Location = "Updated location test",
                Phone = "05323414131"
            };

            // act
            var result = await controller.UpdateInformation(fakeUpdatePerson);

            // assert
            Assert.Equal("Email address is not valid", result.Message);

        }


        [Fact]
        public async void UpdateInformationWithInvalidPhoneTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);
            UpdateInformationDto fakeUpdatePerson = new UpdateInformationDto()
            {
                Detail = "Updated detail test",
                Email = "test@mail.com",
                Id = 15,
                Location = "Updated location test",
                Phone = "0532332432414131"
            };

            // act
            var result = await controller.UpdateInformation(fakeUpdatePerson);

            // assert
            Assert.Equal("Phone format is not valid", result.Message);

        }

        [Fact]
        public async void DeletePersonTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            var firstPerson = DeleteInformationWithIdFakeData(1);
            informationService.Setup(x => x.DeleteInformation(1)).Returns(firstPerson);


            // act
            var result = await controller.DeleteInformation(1);


            // assert
            Assert.Equal(0, result.Data.Status);
        }

        [Fact]
        public async void DeletePersonWithMinusIdTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            // act
            var result = await controller.DeleteInformation(-4);


            // assert
            Assert.Equal("Value of id is invalid", result.Message);
        }



        [Fact]
        public async void DeletePersonWithZeroIdTest()
        {
            // arrange
            var personService = new Mock<IPersonService>();
            var informationService = new Mock<IInformationService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new InformationController(informationService.Object, personService.Object, redis.Object);

            // act
            var result = await controller.DeleteInformation(0);


            // assert
            Assert.Equal("Value of id is invalid", result.Message);
        }


        private async Task<Response<InformationDto>> DeleteInformationWithIdFakeData(int id)
        {
            Response<InformationDto> response = new Response<InformationDto>();
            var list = await GetAllInformationFakeData();
            var information = list.List.Where(x => x.Id == id).FirstOrDefault();
            information.Status = 0;
            response.Data = information;
            response.isSuccess = true;
            response.List = null;
            response.Status = 200;

            return response;
        }

        private async Task<Response<InformationDto>> UpdateInformationFakeData(UpdateInformationDto fakeUpdatePerson)
        {
            Response<InformationDto> response = new Response<InformationDto>();


            response.List = null;
            response.Status = 200;
            response.isSuccess = true;
            response.Data = new InformationDto()
            {
                CreatedDate = DateTime.Now,
                Detail = fakeUpdatePerson.Detail,
                Email = fakeUpdatePerson.Email,
                Id = fakeUpdatePerson.Id,
                Location = fakeUpdatePerson.Location,
                PersonId = 15,
                PersonName = "Test Name",
                PersonSurname = "Test Surname",
                Phone = fakeUpdatePerson.Phone,
                Status = 1
            };

            response.isSuccess = true;
            response.List = null;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        private async Task<Response<InformationDto>> CreateInformationFakeData(CreateInformationDto fakeInformationDto)
        {

            Response<InformationDto> response = new Response<InformationDto>();
            InformationDto informationDto = new InformationDto()
            {
                CreatedDate = DateTime.Now,
                Detail = fakeInformationDto.Detail,
                Email = fakeInformationDto.Email,
                Id = 5,
                Location = fakeInformationDto.Location,
                PersonId = 5,
                PersonName = "Test Information Create",
                PersonSurname = "Test Information Create",
                Phone = fakeInformationDto.Phone,
                Status = 1,

            };

            response.List = null;
            response.Status = 200;
            response.isSuccess = true;
            response.Data = informationDto;
          
            return response;
        }

        private async Task<Response<InformationDto>> GetAllInformationFakeData()
        {
            Response<InformationDto> response = new Response<InformationDto>();
            List<InformationDto> list = new List<InformationDto>();

            var createFake = new InformationDto()
            {
                CreatedDate = DateTime.Now,
                Detail = "Test1",
                Email = "test@mail.com",
                Id = 1,
                Location = "Test1",
                PersonId = 1,
                PersonName = "Test1",
                PersonSurname = "Test1",
                Phone = "05323414131",
                Status = 1

            };

            var createFake2 = new InformationDto()
            {
                CreatedDate = DateTime.Now,
                Detail = "Test2",
                Email = "test@mail.com",
                Id = 2,
                Location = "Test2",
                PersonId = 1,
                PersonName = "Test2",
                PersonSurname = "Test2",
                Phone = "05323414131",
                Status = 1
            };

            list.Add(createFake);
            list.Add(createFake2);

            response.List = list;
            response.Status = 200;
            response.isSuccess = true;


            return response;
        }

        private async Task<Response<InformationDto>> GetInformationWithIdFakeData(int id)
        {

            Response<InformationDto> response = new Response<InformationDto>();
            var list = await GetAllInformationFakeData();
            var person = list.List.Where(x => x.Id == id).FirstOrDefault();

            response.Data = person;
            response.isSuccess = true;
            response.List = null;
            response.Status = 200;

            return response;

        }
    }
}
