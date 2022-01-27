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
    public class CardControllerTests
    {
        
        private readonly Mock<IRepository> _sqlRepository = new Mock<IRepository>();
        
      

        [Fact]
        public void GetCardsAsync_ShouldReturnCards_WhenCardsExists()
        {
            //Arrange

            CardController _sut = new CardController(_sqlRepository.Object);
            var userId = 4;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.GetCardsAsync(userId)).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            
            var cards = new JsonResult(_sut.GetCardsAsync(userId));
            var expected = new JsonResult(cardDto);
            //Assert
            Assert.Equal(cards, expected);
        }

        [Fact]
        public async Task GetCardsAsync_ShouldReturnNone_WhenUserInvalid()
        {
            //Arrange
            var cards = (IEnumerable<dtoCard>)new List<dtoCard>();

            _sqlRepository.Setup(x => x.GetCardsAsync(-1)).Returns(Task.FromResult(cards));
            //Act
            IEnumerable<dtoCard> actual = (IEnumerable<dtoCard>)_sut.GetCardsAsync(-1);
            //Assert
            Assert.Equal(cards, actual);
        }
    }
}