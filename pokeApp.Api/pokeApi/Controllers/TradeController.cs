using Microsoft.AspNetCore.Mvc;
using pokeApi.Data;
using pokeApi.Models;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> GetRecentTradesAsync([FromQuery, Required] string name)
        {
            IEnumerable<dtoTradeRecord> Records = await _repository.GetRecentTradesAsync(name);
            return new JsonResult(Records);
        }

        [HttpGet]
        [ActionName("GetByTradeId")]
        public async Task<IActionResult> GetRecentTradesAsync([FromQuery, Required] int tradeId)
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
        [ActionName("AddTradeDetail")]
        public async Task<IActionResult> AddNewRecordAsync([FromQuery, Required] int tradeId, [Required] int cardId, [Required] int offeredId)
        {
            IEnumerable<dtoTradeRecord> record = await _repository.AddNewRecordAsync(tradeId, cardId, offeredId);
            return new JsonResult(record);
        }

        [HttpPost]
        [ActionName("AddCompletedTrade")]
        public async Task<IActionResult> AddNewRecordAsync([FromQuery, Required] int offeredByID, [Required] int recevedByID)
        {
            IEnumerable<dtoTradeRecord> record = await _repository.AddNewRecordAsync(offeredByID, recevedByID);
            return new JsonResult(record);
        }

        // ------------------- Trade Request ----------------------

        [HttpGet]
        [ActionName("isTradable")]
        public bool CheckTradable([FromQuery, Required] int cardId)
        {
            bool result = _repository.CheckTradable(cardId);
            return result;
        }

        [HttpPost]
        [ActionName("tradeRequest")]
        public async Task<int> AddTradeRequest([FromBody, FromQuery] dtoRequest request)
        {
            int result = await _repository.AddTradeRequest(request.cardID, request.userID, request.offerCardID);
            return result;
        }

        [HttpGet]
        [ActionName("sendrequest")]
        public async Task<IActionResult> GetSendRequest([FromQuery, Required] int userid)
        {
            IEnumerable<Requests> record = await _repository.GetSendRequest(userid);
            return new JsonResult(record);
        }

        [HttpGet]
        [ActionName("receivedrequest")]
        public async Task<IActionResult> GetReceivedRequest([FromQuery, Required] int userid)
        {
            IEnumerable<Requests> record = await _repository.GetReceivedRequest(userid);
            return new JsonResult(record);
        }
    }
}