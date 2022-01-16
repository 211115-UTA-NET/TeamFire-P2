using System;
using pokeApi.Models;
namespace pokeApi.Data
{
    public interface IRepository
    {
        Task<IEnumerable<dtoUser>> GetUsersAsync(string name);
        Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string Email);
        Task<IEnumerable<dtoCard>> GetCardsAsync(int userId);
        Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int userId, int cardId);
        //Task<IEnumerable<dtoTradeRecord>> GetTradesDetailsAsync(string name);
        //Task<IEnumerable<dtoTradeRecord>> AddNewTradesAsync(string cardId, string pokemon, string seller, string buyer);

    }
}
