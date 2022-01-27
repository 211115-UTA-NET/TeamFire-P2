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
    public class TradeControllerTests
    {
        private readonly TradeController _sut;
        private readonly Mock<IRepository> _sqlRepository = new Mock<IRepository>();


        public TradeControllerTests()
        {
            _sut = new TradeController(_sqlRepository.Object);
        }

        [Fact]
        public async Task GetRecentTradesAsync_ShouldReturn_ByName()
        {

            //Arrange
            var name = "Ben";
            var tradeDto = new List<dtoTradeRecord>();
            tradeDto.Add(new(1, "Ben", 1, "Jing", 2, 151, "Mew", 34));

            _sqlRepository.Setup(x => x.GetRecentTradesAsync(name)).Returns(Task.FromResult((IEnumerable<dtoTradeRecord>)tradeDto));

            //Act
            var trades = await _sut.GetRecentTradesAsync(name);
            var expected = new JsonResult(tradeDto);

            //Assert
            Assert.Equal(expected.ToString(), trades.ToString());
        }

        [Fact]
        public async Task GetRecentTradesAsync_ShouldReturn_ByTradeId()
        {

            //Arrange
            var tradeId = 1;
            var tradeDto = new List<dtoTradeRecord>();
            tradeDto.Add(new(1, "Ben", 1, "Jing", 2, 151, "Mew", 34));

            _sqlRepository.Setup(x => x.GetRecentTradesAsync(tradeId)).Returns(Task.FromResult((IEnumerable<dtoTradeRecord>)tradeDto));

            //Act
            var trades = await _sut.GetRecentTradesAsync(tradeId);
            var expected = new JsonResult(tradeDto);

            //Assert
            Assert.Equal(expected.ToString(), trades.ToString());
        }

        [Fact]
        public async Task GetRecentTradesAsync_ShouldReturnNone_WhenNone()
        {

            //Arrange
            var tradeId = 1;
            var tradeDto = new List<dtoTradeRecord>();
            

            _sqlRepository.Setup(x => x.GetRecentTradesAsync(-1)).Returns(Task.FromResult((IEnumerable<dtoTradeRecord>)tradeDto));

            //Act
            var trades = await _sut.GetRecentTradesAsync(-1);
            var expected = new JsonResult(tradeDto);

            //Assert
            Assert.Equal(expected.ToString(), trades.ToString());
        }

        [Fact]
        public async Task GetRecentTradesAsync_ShouldReturn_All()
        {

            //Arrange
            var tradeId = 1;
            var tradeDto = new List<dtoTradeRecord>();
            tradeDto.Add(new(1, "Ben", 1, "Jing", 2, 151, "Mew", 34));

            _sqlRepository.Setup(x => x.GetRecentTradesAsync()).Returns(Task.FromResult((IEnumerable<dtoTradeRecord>)tradeDto));

            //Act
            var trades = await _sut.GetRecentTradesAsync();
            var expected = new JsonResult(tradeDto);

            //Assert
            Assert.Equal(expected.ToString(), trades.ToString());
        }

        [Fact]
        public void CheckTradeable_ShouldWork_()
        {

            //Arrange
            var tradeId = 1;
            var tradeDto = new List<dtoTradeRecord>();
            tradeDto.Add(new(1, "Ben", 1, "Jing", 2, 151, "Mew", 34));

            _sqlRepository.Setup(x => x.CheckTradable(34)).Returns(true);

            //Act
            var actual = _sut.CheckTradable(34);
            var expected = true;

            //Assert
            Assert.Equal(expected, actual);
        }


    }
}
