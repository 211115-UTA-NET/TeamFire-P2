using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetCardsAsync(
            [FromQuery] int userId
            )
        {
            IEnumerable<dtoCard> Cards = await _repository.GetCardsAsync(userId);
            return new JsonResult(Cards);
        }

        //==============POST==============//
        [HttpPut]
        public async Task<IActionResult> UpdateCardOwnerAsync([FromQuery] int userId, int cardId)
        {
            IEnumerable<dtoCard> Cards = await _repository.UpdateCardOwnerAsync(userId, cardId);
            return new JsonResult(Cards);
        }
    }
}