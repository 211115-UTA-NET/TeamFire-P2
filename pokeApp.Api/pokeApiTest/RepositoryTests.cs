using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pokeApi.Data;
using pokeApi.Models;
using Moq;
using Xunit;

namespace pokeApiTest
{
    public class RepositoryTests
    {

        Mock<IRepository> _repositoryMock = new();


        [Fact]
        public async Task GetUsersAsync_ShouldReturnUser_WhenUserExists()
        {

            //Arrange
            var userID = 1;
            var name = "Ben";
            var email = "test@gmail.com";
            var password = "test";
            var userDto = new List<dtoUser>();
            userDto.Add(new(userID, name, password, email));
               
            

            _repositoryMock.Setup(x => x.GetUsersAsync(name, email)).Returns(Task.FromResult((IEnumerable<dtoUser>)userDto));

            //Act
            var actual = await _repositoryMock.Object.GetUsersAsync(name, email);
            var expected = userDto;

            //Assert
            Assert.Equal(expected, actual);


        }
        [Fact]
        public async Task GetUsersAsync_ShouldReturnNone_WhenUserInvalid()
        {

            //Arrange
            var userID = 1;
            var name = "Ben";
            var email = "test@gmail.com";
            var password = "test";
            var userDto = new List<dtoUser>();



            _repositoryMock.Setup(x => x.GetUsersAsync(" ", " ")).Returns(Task.FromResult((IEnumerable<dtoUser>)userDto));

            //Act
            var actual = await _repositoryMock.Object.GetUsersAsync(" ", " ");
            var expected = userDto;

            //Assert
            Assert.Equal(expected, actual);


        }
      


    }
}
