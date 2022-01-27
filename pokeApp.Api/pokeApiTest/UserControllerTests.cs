using System.Threading.Tasks;
using Xunit;
using pokeApi.Controllers;
using pokeApi.Data;
using Moq;
using System.Linq;
using System.Collections.Generic;
using pokeApi.Models;

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
            var name = "Benjamin";
            var email = "test@gmail.com";
            var userDto = new dtoUser(1, "Benjamin", "test", "test@gmail.com");

            _sqlRepository.Setup(x => x.GetUsersAsync(name, email)).ReturnsAsync(userDto);

            //Act
            IEnumerable<dtoUser> users = (IEnumerable<dtoUser>)_sut.GetAllByNameAsync(name, email);
            //Assert

            Assert.Equal(userDto, users);
        }
    }
}
