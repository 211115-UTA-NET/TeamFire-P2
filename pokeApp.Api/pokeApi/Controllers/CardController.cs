using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pokeApi.Data;
using pokeApi.Models;

namespace pokeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly IRepository _repository;

        public CardController(IRepository repository)
        {
            _repository = repository;
        }
        //============ GET ===============//
        [HttpGet]
        public async Task<IActionResult> GetCardsAsync([FromQuery, Required] int userId)
        {
            IEnumerable<dtoCard> Cards = await _repository.GetCardsAsync(userId);
            return new JsonResult(Cards);
        }

        //================CHECK WHO OWNS THE CARD====//
        [HttpGet("checkInfo")]
        public async Task<IActionResult> CheckCardOwner(int cardID)
        {
            IEnumerable<dtoCard> cardInfo = await _repository.CheckCardOwner(cardID);
            return new JsonResult(cardInfo);
        }


        //============ GET CARDS BEING TRADED ===============//
        [HttpGet("trading")]

        public async Task<IActionResult> GetTradeCardsAsync()
            //[FromQuery, Required]
        {
            IEnumerable<dtoCard> Cards = await _repository.GetTradeCardsAsync();
            return new JsonResult(Cards);
        }

        //==============POST==============//
        [HttpPost("NewCard")]
        public async Task<IActionResult> GetNewRandCardAsync([FromBody, Required] dtoNewCard card)
        {
            IEnumerable<dtoCard> Cards = await _repository.GetNewRandCardAsync(card.userId);
            return new JsonResult(Cards);
        }

        [HttpPut("updateOwner")]
        public async Task<IActionResult> UpdateCardOwnerAsync([FromBody, Required] dtoUpdateCard info )
        {
            IEnumerable<dtoCard> Cards = await _repository.UpdateCardOwnerAsync(info.userId, info.cardId);
            return new JsonResult(Cards);

        }

        [HttpPut("toggleTradable")]
        public async Task<IActionResult> toggelTrading([FromBody, Required] dtoToggle Id)
        {
            IEnumerable<dtoCard> Cards = await _repository.toggelTrading(Id.cardId);
            return new JsonResult(Cards);

        }
    }
}