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


        public async Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int offeredByID, int recevedByID)
        {

            CompletedTrade td = new CompletedTrade
            {
                OfferedBy = offeredByID,
                RedeemedBy = recevedByID

            };
            await _context.CompletedTrades.AddAsync(td);
            await _context.SaveChangesAsync();


            var trades = await _context.CompletedTrades
                 .Include(trade => trade.TradeId)
                 .Include(trade => trade.OfferedByNavigation.UserName)
                 .Include(trade => trade.RedeemedByNavigation.UserName)
                 .Include(trade => trade.TradeDetails)
                 .ThenInclude(TradeDetail => TradeDetail.CardId)
                 .Include(trade => trade.TradeDetails)
                 .ThenInclude(TradeDetail => TradeDetail.Card.Poke.Pokemon)
                 .ToListAsync();

            return trades.Select(trade =>
            {
                return new dtoTradeRecord(
                    trade.TradeId, trade.OfferedByNavigation.UserName, -1, trade.RedeemedByNavigation.UserName, -1, -1, "1", 1
                    );
            });
        }

        public async Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredByID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string email)
        {
            bool ifExist = await _context.Users.AnyAsync(user => user.UserName == name && user.Email == email);
            if (!ifExist)
            {
                var newUser = new User
                {
                    UserName = name,
                    Password = pw,
                    Email = email
                };

                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync();

            }
            return await GetUsersAsync(name, email);

        }
        //---------------------------//
        public async Task<IEnumerable<dtoCard>> GetCardsAsync(int userId)
        {

            var cards = await _context.Cards
                .Include(card => card.Poke)
                .Include(card => card.User)
                .Where(card => card.UserId == userId)
                .ToListAsync();
            return cards.Select(card =>
            {
                return new dtoCard(card.CardId, card.UserId, card.User.UserName, card.PokeId, card.Poke.Pokemon, card.Trading);
            });
        }

        public async Task<IEnumerable<dtoCard>> GetNewRandCardAsync(int userId)
        {
            System.Random rand = new System.Random();
            int pokeid = rand.Next(1, 810);
            int thisID = 0;
            var card = new Card
            {

                UserId = userId,
                PokeId = pokeid,
                Trading = thisID
            };
            var result = _context.Cards.AddAsync(card);


            var cards = await _context.Cards
                .Include(card => card.Poke)
                .Include(card => card.User)
                .Where(card => card.UserId == userId)
                .ToListAsync();
            return cards.Select(card =>
            {
                return new dtoCard(card.CardId, card.UserId, card.User.UserName, card.PokeId, card.Poke.Pokemon, card.Trading);
            });



        }

        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(string name)
        {

            List<dtoTradeRecord> records2 = new();
            var trades = await (
               from records in _context.CompletedTrades
               join o in _context.Users on records.OfferedBy equals o.UserId
               join r in _context.Users on records.RedeemedBy equals r.UserId
               join dets in _context.TradeDetails on records.TradeId equals dets.TradeId
               join crd in _context.Cards on dets.CardId equals crd.CardId
               join dex in _context.Dices on crd.PokeId equals dex.PokeId
               where o.UserName == name || r.UserName == name
               select new
               {
                   tradeID = records.TradeId,
                   offeredBy = o.UserName,
                   offeredByID = records.OfferedBy,
                   redeemedBy = r.UserName,
                   redeemedByID = records.RedeemedBy,
                   pokeID = dex.PokeId,
                   pokemon = dex.Pokemon,
                   cardId = dets.CardId
               }).ToListAsync();

            foreach (var Trade in trades)
            {
                records2.Add(new dtoTradeRecord(Trade.tradeID, Trade.offeredBy, Trade.offeredByID, Trade.redeemedBy, Trade.redeemedByID, Trade.pokeID, Trade.pokemon, Trade.cardId
                    ));
            }
            return records2;
        }

        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(int tradeId)
        {

            List<dtoTradeRecord> records2 = new();
            var trades = await (
               from records in _context.CompletedTrades
               join o in _context.Users on records.OfferedBy equals o.UserId
               join r in _context.Users on records.RedeemedBy equals r.UserId
               join dets in _context.TradeDetails on records.TradeId equals dets.TradeId
               join crd in _context.Cards on dets.CardId equals crd.CardId
               join dex in _context.Dices on crd.PokeId equals dex.PokeId
               where records.TradeId == tradeId
               select new
               {
                   tradeID = records.TradeId,
                   offeredBy = o.UserName,
                   offeredByID = records.OfferedBy,
                   redeemedBy = r.UserName,
                   redeemedByID = records.RedeemedBy,
                   pokeID = dex.PokeId,
                   pokemon = dex.Pokemon,
                   cardId = dets.CardId
               }).ToListAsync();

            foreach (var Trade in trades)
            {
                records2.Add(new dtoTradeRecord(Trade.tradeID, Trade.offeredBy, Trade.offeredByID, Trade.redeemedBy, Trade.redeemedByID, Trade.pokeID, Trade.pokemon, Trade.cardId
                    ));
            }
            return records2;
        }

        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync()
        {
            List<dtoTradeRecord> records2 = new();
            var trades = await (
               from records in _context.CompletedTrades
               join o in _context.Users on records.OfferedBy equals o.UserId
               join r in _context.Users on records.RedeemedBy equals r.UserId
               join dets in _context.TradeDetails on records.TradeId equals dets.TradeId
               join crd in _context.Cards on dets.CardId equals crd.CardId
               join dex in _context.Dices on crd.PokeId equals dex.PokeId
               select new
               {
                   tradeID = records.TradeId,
                   offeredBy = o.UserName,
                   offeredByID = records.OfferedBy,
                   redeemedBy = r.UserName,
                   redeemedByID = records.RedeemedBy,
                   pokeID = dex.PokeId,
                   pokemon = dex.Pokemon,
                   cardId = dets.CardId
               }).ToListAsync();

            foreach (var Trade in trades)
            {
                records2.Add(new dtoTradeRecord(Trade.tradeID, Trade.offeredBy, Trade.offeredByID, Trade.redeemedBy, Trade.redeemedByID, Trade.pokeID, Trade.pokemon, Trade.cardId
                    ));
            }
            return records2;
        }

        public async Task<IEnumerable<dtoCard>> GetTradeCardsAsync()
        {

            var cards = await _context.Cards
                .Include(card => card.Poke)
                .Include(card => card.User)
                .Where(card => card.Trading == 1)
                .ToListAsync();

            return cards.Select(card =>
            {
                return new dtoCard(card.CardId, card.UserId, card.User.UserName, card.PokeId, card.Poke.Pokemon, card.Trading);
            });
        }

        public async Task<IEnumerable<dtoUser>> GetUsersAsync(string name, string email)
        {
            var ppls = await _context.Users
                .Where(ppl => ppl.UserName == name && ppl.Email == email)
                .ToListAsync();

            return ppls.Select(ppl =>
            {
                return new dtoUser(ppl.UserId, ppl.UserName, "pw", "email");
            });
        }

        public async Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int userId, int cardId)
        {


            var result = _context.Cards.SingleOrDefault(c => c.CardId == cardId);
            if (result != null)
            {
                result.UserId = userId;
                await _context.SaveChangesAsync();
            }

            var result2 = await _context.Cards
               .Include(card => card.Poke)
               .Include(card => card.User)
               .Where(card => card.CardId == cardId)
               .ToListAsync();

            return result2.Select(card =>
            {
                return new dtoCard(card.CardId, card.UserId, card.User.UserName, card.PokeId, card.Poke.Pokemon, card.Trading);
            });

        }
    }
}