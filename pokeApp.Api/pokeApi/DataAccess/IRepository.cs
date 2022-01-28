using System;
using pokeApi.Models;
namespace pokeApi.Data
{
    public interface IRepository
    {
        Task<IEnumerable<dtoUser>> GetUsersAsync(string name, string email);
        Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string email);
        Task<IEnumerable<dtoCard>> GetCardsAsync(int userId);
        Task<IEnumerable<dtoCard>> GetTradeCardsAsync();
        Task<IEnumerable<dtoCard>> GetNewRandCardAsync(int userId);
        Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int userId, int cardId);
        Task<IEnumerable<dtoCard>> CheckCardOwner(int cardID);
        Task<IEnumerable<dtoCard>> toggelTrading(int cardId);
        Task<int> UpdateTradeStatus(int requestID, string requestStatus);
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(string name);
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(int tradeId);
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync();
        Task<int> AddNewRecordAsync(int offeredByID, int receivedByID);
        Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredById);
        bool CheckTradable(int cardId);
        Task<int> AddTradeRequest(int cardID, int userID, int offerCardID, int targetuserid);
        Task<IEnumerable<Requests>> GetSendRequest(int userid);
        Task<IEnumerable<Requests>> GetReceivedRequest(int userid);
    }
}
