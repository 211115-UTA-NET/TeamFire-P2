using Microsoft.EntityFrameworkCore;
using pokeApi.Models;
using pokiApi.DataInfrastructure;
using System.Linq;

namespace pokeApi.Data
{
    public class EfRepository : IRepository
    {
        private readonly P2SQLTeamFireContext _context;
        private readonly ILogger<EfRepository> _logger;

        public EfRepository(P2SQLTeamFireContext context, ILogger<EfRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int offeredByID, int recevedByID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredByID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string Email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dtoCard>> GetCardsAsync(int userId)
        {
            //throw new NotImplementedException();
            //using SqlCommand cmd = new(
            //            @"SELECT cardID,poke.Cards.userID,userName,poke.Cards.pokeID,pokemon,trading
            //            From poke.Cards 
            //            INNER JOIN poke.Dex ON poke.Cards.pokeID = poke.Dex.pokeID
            //            INNER JOIN poke.Users On poke.Cards.userID = poke.Users.userID
            //            WHERE poke.Cards.userID = @sortID;",

            //connection);
            var cards = await _context.Cards        // From poke.Cards
                .Include(card => card.Poke)         // INNER JOIN poke.Dex -> Dex = Poke
                .Include(card=>card.User)           // INNER JOIN poke.Users
                .Where(card=>card.UserId==userId)   // WHERE poke.Cards.userID = @sortID;
                .ToListAsync();

            // SELECT cardID,poke.Cards.userID,userName,poke.Cards.pokeID,pokemon,trading
            return cards.Select(card =>
            {
                return new dtoCard(card.CardId, card.UserId, card.User.UserName, card.PokeId, card.Poke.Pokemon, card.Trading);
            });
        }

        public Task<IEnumerable<dtoCard>> GetNewRandCardAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(int tradeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoCard>> GetTradeCardsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoUser>> GetUsersAsync(string name, string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int userId, int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
