using FakeItEasy;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using PhoneDirectory.API.Controllers;
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
    public class PersonControllerTest
    {
        [Fact]
        public async void GetPersonListTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var persons = GetAllPersonFakeData();
            service.Setup(x => x.GetAllPersons()).Returns(persons);
            var controller = new PersonController(service.Object, redis.Object);

            // Act
            var results = await controller.GetPersonList();



            // Assert
            Assert.Equal(200, results.Status);
        }

        [Fact]
        public async void GetPersonByIdTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();

            var persons = await GetAllPersonFakeData();
            var firstPerson = GetPersonWithIdFakeData(1);



            service.Setup(x => x.GetPersonById(1)).Returns(firstPerson);

            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.Get(1);


            // assert
            Assert.Equal(1, result.Data.Id);
        }

        [Fact]
        public async void GetPersonByZeroIdTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.Get(0);

            // assert
            Assert.Equal(500,result.Status);
        }

        [Fact]
        public async void GetPersonByMinusIdTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.Get(-5);

            // assert
            Assert.Equal(500, result.Status);
        }

        [Fact]
        public async void CreatePersonTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var fakePersonDto = new CreatePersonDto()
            {
                CompanyName = "TestCompany",
                Detail = "Test Detail",
                Email = "test@mail.com",
                Location = "Test Location",
                Name = "Test Name",
                Phone = "05323414131",
                Surname = "Test Surname"

            };
            var response = CreatePersonFakeData(fakePersonDto);
            service.Setup(x => x.CreatePerson(fakePersonDto)).Returns(response);
            var controller = new PersonController(service.Object, redis.Object);

            // Act
            var results = await controller.CreatePerson(fakePersonDto);



            // Assert
            Assert.Equal(200, results.Status);
        }

        [Fact]
        public async void CreatePersonWithAllNullValuesTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();  
            var controller = new PersonController(service.Object, redis.Object);
            var fakePerson = new CreatePersonDto()
            {

            };

            // act
            var result = await controller.CreatePerson(fakePerson);


            // assert
            Assert.Equal("Name or surname cannot be empty!", result.Message);
        }

        [Fact]
        public async void CreatePersonWithInvalidEmailTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);
            var fakePerson = new CreatePersonDto()
            {
                CompanyName = "TestCompany",
                Detail = "Test Detail",
                Email = "asdasda",
                Location = "Test Location",
                Name = "Test Name",
                Phone = "05323414131",
                Surname = "Test Surname"

            };

            // act
            var result = await controller.CreatePerson(fakePerson);


            // assert
            Assert.Equal("Email address is not valid", result.Message);
        }


        [Fact]
        public async void CreatePersonWithInvalidPhoneTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);
            var fakePerson = new CreatePersonDto()
            {
                CompanyName = "TestCompany",
                Detail = "Test Detail",
                Email = "test@mail.com",

                Location = "Test Location",
                Name = "Test Name",
                Phone = "3253253253",
                Surname = "Test Surname"

            };

            // act
            var result = await controller.CreatePerson(fakePerson);


            // assert
            Assert.Equal("Phone format is not valid", result.Message);
        }

        [Fact]
        public async void UpdatePersonTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);
            UpdatePersonDto fakeUpdatePerson = new UpdatePersonDto()
            {
                CompanyName = "Updated Company Name Test",
                Name = "Updated Name Test",
                Surname = "Updated Surname Test",
                Id = 15,
            };
            var response = UpdatePersonFakeData(fakeUpdatePerson);
            service.Setup(x => x.UpdatePerson(fakeUpdatePerson)).Returns(response);

            // act
            var result = await controller.UpdatePerson(fakeUpdatePerson);

            // assert
            Assert.Equal(200, result.Status);

        }

        [Fact]
        public async void UpdatePersonWithNullNameTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);
            UpdatePersonDto fakeUpdatePerson = new UpdatePersonDto()
            {
                CompanyName = "Updated Company Name Test",
                Name = null,
                Surname = "Updated Surname Test",
                Id = 15,
            };

            // act
            var result = await controller.UpdatePerson(fakeUpdatePerson);

            // assert
            Assert.Equal("Name or surname cannot be empty", result.Message);

        }
        [Fact]
        public async void UpdatePersonWithNullSurNameTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();
            var controller = new PersonController(service.Object, redis.Object);
            UpdatePersonDto fakeUpdatePerson = new UpdatePersonDto()
            {
                CompanyName = "Updated Company Name Test",
                Name = "Updated Test Name",
                Surname = null,
                Id = 15,
            };

            // act
            var result = await controller.UpdatePerson(fakeUpdatePerson);

            // assert
            Assert.Equal("Name or surname cannot be empty", result.Message);

        }


        [Fact]
        public async void DeletePersonTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();

            var firstPerson = DeletePersonWithIdFakeData(1);



            service.Setup(x => x.DeletePerson(1)).Returns(firstPerson);

            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.DeletePerson(1);


            // assert
            Assert.Equal(0, result.Data.Status);
        }

        [Fact]
        public async void DeletePersonWithMinusIdTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();

            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.DeletePerson(-1);


            // assert
            Assert.Equal("Value of id is invalid", result.Message);
        }

        [Fact]
        public async void DeletePersonWithZeroIdTest()
        {
            // arrange
            var service = new Mock<IPersonService>();
            var redis = new Mock<IDistributedCache>();

            var controller = new PersonController(service.Object, redis.Object);

            // act
            var result = await controller.DeletePerson(0);


            // assert
            Assert.Equal("Value of id is invalid", result.Message);
        }


        private async Task<Response<PersonDto>> GetAllPersonFakeData()
        {
            Response<PersonDto> response = new Response<PersonDto>();
            List<PersonDto> list = new List<PersonDto>();

            var createFake = new PersonDto()
            {
                CompanyName = "Test1",
                CreatedDate = DateTime.Now,
                Id = 1,
                Name = "Test1",
                Status = 1,
                Surname = "Test1"
            };

            var createFake2 = new PersonDto()
            {
                CompanyName = "Test2",
                CreatedDate = DateTime.Now,
                Id = 1,
                Name = "Test2",
                Status = 1,
                Surname = "Test2"
            };

            list.Add(createFake);
            list.Add(createFake2);

            response.List = list;
            response.Status = 200;
            response.isSuccess = true;


            return response;
        }


        private async Task<Response<PersonDto>> GetPersonWithIdFakeData(int id)
        {

            Response<PersonDto> response = new Response<PersonDto>();
            var list = await GetAllPersonFakeData();
            var person = list.List.Where(x => x.Id == id).FirstOrDefault();

            response.Data = person;
            response.isSuccess = true;
            response.List = null;
            response.Status = 200;

            return response;

        }

        private async Task<Response<PersonDto>> CreatePersonFakeData(CreatePersonDto dto)
        {
            Response<PersonDto> response = new Response<PersonDto>();
            InformationDto informationDto = new InformationDto()
            {
                CreatedDate = DateTime.Now,
                Detail = dto.Detail,
                Email = dto.Email,
                Id = 5,
                Location = dto.Location,
                PersonId = 5,
                PersonName = dto.Name,
                PersonSurname = dto.Surname,
                Phone = dto.Phone,
                Status = 1

            };

            response.List = null;
            response.Status = 200;
            response.isSuccess = true;
            response.Data = new PersonDto()
            {
                CompanyName = dto.CompanyName,
                CreatedDate = DateTime.Now,
                Id = 5,
                Name = dto.Name,
                Status = 1,
                Surname = dto.Surname,        
               Information = new List<InformationDto>()
            };

            response.Data.Information.Add(informationDto);

            return response;
        }

        private async Task<Response<PersonDto>> UpdatePersonFakeData(UpdatePersonDto dto)
        {
            Response<PersonDto> response = new Response<PersonDto>();
           

            response.List = null;
            response.Status = 200;
            response.isSuccess = true;
            response.Data = new PersonDto()
            {
                CompanyName = dto.CompanyName,
                CreatedDate = DateTime.Now,
                Id = dto.Id,
                Name = dto.Name,
                Status = 1,
                Surname = dto.Surname,
            };

            response.isSuccess = true;
            response.List = null;
            response.Message = "Success";
            response.Status = 200;
            return response;
        }

        private async Task<Response<PersonDto>> DeletePersonWithIdFakeData(int id)
        {

            Response<PersonDto> response = new Response<PersonDto>();
            var list = await GetAllPersonFakeData();
            var person = list.List.Where(x => x.Id == id).FirstOrDefault();
            person.Status = 0;
            response.Data = person;
            response.isSuccess = true;
            response.List = null;
            response.Status = 200;

            return response;

        }

    }
}
