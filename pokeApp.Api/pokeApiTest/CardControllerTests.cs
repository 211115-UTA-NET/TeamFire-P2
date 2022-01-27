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
        private readonly CardController _sut;
        private readonly Mock<IRepository> _sqlRepository = new Mock<IRepository>();
        
        public CardControllerTests()
        {
            _sut = new CardController(_sqlRepository.Object);
        }
      

        [Fact]
        public async Task GetCardsAsync_ShouldReturnCards_WhenCardsExists()
        {
            //Arrange
            var userId = 4;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.GetCardsAsync(userId)).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            //Act
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
            var actual = await _sut.GetCardsAsync(-1);
            //Assert
            Assert.Equal(actual.ToString(), new JsonResult(cards).ToString());
        }
        /*[Fact]
        public async Task UpdateCardOwnerAsync_ShouldUpdateOwner_WhenUserInvalid()
        {
            //Arrange
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.UpdateCardOwnerAsync(4, 1)).Returns(Task.FromResult());
            //Act
            var actual = await _sut.GetCardsAsync(-1);
            //Assert
            Assert.Equal(actual.ToString(), new JsonResult(cards).ToString());
        }*/

        [Fact]
        public async Task GetTradeCardsAsync_ShouldGetTradeableCards()
        {
            //Arrange
            var userId = 4;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.GetTradeCardsAsync()).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            //Act

            var actual = await _sut.GetTradeCardsAsync();
            var expected = new JsonResult(cardDto);
            //Assert
            Assert.Equal(actual.ToString(), expected.ToString());
        }
        [Fact]
        public async Task UpdateCardOwnerAsync_ShouldUpdateCardOwner()
        {
            //Arrange
            var userId = 4;
            var cardId = 1;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.UpdateCardOwnerAsync(userId, cardId)).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            //Act

            var actual = await _sut.UpdateCardOwnerAsync(new dtoUpdateCard());
            var expected = new JsonResult(cardDto);
            //Assert
            Assert.Equal(actual.ToString(), expected.ToString());
        }
        [Fact]
        public async Task GetNewRandCardAsync_ShouldAddNewCard()
        {
            //Arrange
            var userId = 4;
            var cardId = 1;
            var cardDto = new List<dtoCard>();
            cardDto.Add(new(1, 4, "ben", 151, "Mew", 1));

            _sqlRepository.Setup(x => x.GetNewRandCardAsync(userId)).Returns(Task.FromResult((IEnumerable<dtoCard>)cardDto));
            //Act

            var actual = await _sut.GetNewRandCardAsync(new dtoNewCard());
            var expected = new JsonResult(cardDto);
            //Assert
            Assert.Equal(actual.ToString(), expected.ToString());
        }

    }

    
}