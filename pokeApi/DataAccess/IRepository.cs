using System;
using pokeApi.Models;
namespace pokeApi.Data
{
    public interface IRepository
    {
        //Task<IEnumerable<User>> GetUsersAsync(string name);
        Task<IEnumerable<dtoUser>> AddNewUserAsync(string user);
        Task<IEnumerable<dtoCard>> GetCardsAsync(string name);
        Task<IEnumerable<dtoCard>> UpdateInventoryAsync(string newOwner, int cardUPC);
        Task<IEnumerable<dtoTrade>> GetTradesDetailsAsync(string name);
        Task<IEnumerable<dtoTrade>> AddNewTradesAsync(string cardId, string pokemon, string seller, string buyer);

    }
}
