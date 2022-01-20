using Microsoft.AspNetCore.Mvc;
using pokeApi.Data;
using pokeApi.Models;

namespace pokeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IRepository _repository;

        public TradeController(IRepository repository)
        {
            _repository = repository;
        }
        //============ GET ===============//
        [HttpGet]
        [ActionName("GetByName")]
        public async Task<IActionResult> GetRecentTradesAsync(
            [FromQuery] string name
            )
        {
            IEnumerable<dtoTradeRecord> Records = await _repository.GetRecentTradesAsync(name);
            return new JsonResult(Records);
        }

        [HttpGet]
        [ActionName("GetByTradeId")]
        public async Task<IActionResult> GetRecentTradesAsync(
            [FromQuery] int tradeId
            )
        {
            IEnumerable<dtoTradeRecord> Records = await _repository.GetRecentTradesAsync(tradeId);
            return new JsonResult(Records);
        }

        [HttpGet]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetRecentTradesAsync()
        {
            IEnumerable<dtoTradeRecord> Records = await _repository.GetRecentTradesAsync();
            return new JsonResult(Records);
        }



        //==============POST==============///
        [HttpPost]
        [ActionName("GetDetail")]
        public async Task<IActionResult> AddNewRecordAsync([FromQuery] int tradeId, int cardId, int offeredId)
        {
            IEnumerable<dtoTradeRecord> record = await _repository.AddNewRecordAsync(tradeId, cardId, offeredId);
            return new JsonResult(record);
        }

        [HttpPost]
        [ActionName("GetRecord")]
        public async Task<IActionResult> AddNewRecordAsync([FromQuery] int offeredByID, int recevedByID)
        {
            IEnumerable<dtoTradeRecord> record = await _repository.AddNewRecordAsync(offeredByID, recevedByID);
            return new JsonResult(record);
        }
    }
}