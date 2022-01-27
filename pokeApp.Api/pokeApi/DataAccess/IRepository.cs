﻿using System;
using pokeApi.Models;
namespace pokeApi.Data
{
    public interface IRepository
    {
        Task<IEnumerable<dtoUser>> GetUsersAsync(string name, string email);
        Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string Email);
        Task<IEnumerable<dtoCard>> GetCardsAsync(int userId);
        Task<IEnumerable<dtoCard>> GetTradeCardsAsync();
        Task<IEnumerable<dtoCard>> GetNewRandCardAsync(int userId);
        Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int userId, int cardId);
        Task<IEnumerable<dtoCard>> toggelTrading(int cardId); 
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(string name);
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(int tradeId);
        Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync();
        Task<int> AddNewRecordAsync(int offeredByID, int recevedByID);
        Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredByID);
        bool CheckTradable(int cardId);
        Task<int> AddTradeRequest(int cardID, int userID, int offerCardID);
        Task<IEnumerable<Requests>> GetSendRequest(int userid);
        Task<IEnumerable<Requests>> GetReceivedRequest(int userid);
    }
}
