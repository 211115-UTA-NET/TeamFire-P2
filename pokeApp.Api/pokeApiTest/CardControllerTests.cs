using System.Threading.Tasks;
using Xunit;
using pokeApi.Controllers;
using pokeApi.Data;
using Moq;
using pokeApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace pokeApiTest
{
    public class CardControllerTests
    {
        
        private readonly Mock<IRepository> _sqlRepository = new Mock<IRepository>();
        
      

        [Fact]
        public async Task GetCardsAsync_ShouldReturnCards_WhenCardsExistsAsync()
        {
            //Arrange

            CardController _sut = new CardController(_sqlRepository.Object);
            var userId = 4;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.GetCardsAsync(userId)).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            
            var cards = await _sut.GetCardsAsync(userId);
            var expected = new JsonResult(cardDto);
            //Assert
            Assert.Equal(cards.ToString(), expected.ToString());
        }

        [Fact]
        public async Task GetCardsAsync_ShouldReturnNone_WhenUserInvalidAsync()
        {
            //Arrange
            var cards = (IEnumerable<dtoCard>)new List<dtoCard>();

            _sqlRepository.Setup(x => x.GetCardsAsync(-1)).Returns(Task.FromResult(cards));
            //Act
            CardController _sut = new CardController(_sqlRepository.Object);
            var actual = await _sut.GetCardsAsync(-1);
            //Assert
            Assert.Equal(actual.ToString(), new JsonResult(cards).ToString());
        }
    }
}