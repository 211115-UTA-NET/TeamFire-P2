using System.Threading.Tasks;
using Xunit;
using pokeApi.Controllers;
using pokeApi.Data;
using Moq;
using System.Linq;
using System.Collections.Generic;
using pokeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace pokeApiTest
{
    public class UserControllerTests
    {
        private readonly UserController _sut;
        private readonly Mock<IRepository> _sqlRepository = new Mock<IRepository>();


        public UserControllerTests()
        {
            _sut = new UserController(_sqlRepository.Object);
        }

       [Fact]
       public async Task GetAllByNameAsync_ShouldReturnUsers_WhenTheyExist()
        {
            //Arange
            var name = "benjamin";
            var email = "test@gmail.com";
            var userDto = new List<dtoUser>();
            userDto.Add(new(1, "benjamin", "test", "test@gmail.com"));

            _sqlRepository.Setup(x => x.GetUsersAsync(name, email)).Returns(Task.FromResult((IEnumerable<dtoUser>)userDto));

            //Act
            var users = await _sut.GetAllByNameAsync(name, email);
            var expected = new JsonResult(userDto);
            //Assert

            Assert.Equal(userDto.ToString(), users.ToString());
        }
    }
}
