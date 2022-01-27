﻿using Microsoft.EntityFrameworkCore;
using pokeApi.Models;
using pokiApi.DataInfrastructure;
using System.Linq;

namespace pokeApi.Data
{

    public abstract class RandomNumberGenerator : IDisposable
    {

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class EfRepository : IRepository
    {
        private readonly P2SQLTeamFireContext _context;
        private readonly ILogger<EfRepository> _logger;

        public EfRepository(P2SQLTeamFireContext context, ILogger<EfRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Might be redundant - not sure what to return here
        //should probably return nothing here
        //or at the very least not show return values
        //===================================AddNewRecordAsync=====//
        public async Task<int> AddNewRecordAsync(int offeredByID, int recevedByID)
        {

            var newTrade = new CompletedTrade
            {
                OfferedBy = offeredByID,
                RedeemedBy = recevedByID

            };

            await _context.AddAsync(newTrade);
            await _context.SaveChangesAsync();

            int newID =   _context.CompletedTrades.Select(r => r.TradeId).Max();


            return newID;
        }




        //==============AddNewDetailedRecordAsync=============
        public async Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredByID)
        {
            var newrecord = new TradeDetail
            {
                TradeId = tradeId,
                CardId = cardId,
                UserId = offeredByID
            };

            await _context.AddAsync(newrecord);
            await _context.SaveChangesAsync();

            return await GetRecentTradesAsync(tradeId);

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

        //================assign random card to player======//
        // tried to return card with the highest cardID but could not do it
        public async Task<IEnumerable<dtoCard>> GetNewRandCardAsync(int userId)
        {
            //RandomNumberGenerator;
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
            await _context.SaveChangesAsync();

            return await  GetCardsAsync(userId);


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
                result.Trading = 0;
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
<<<<<<< HEAD

        public async Task<IEnumerable<dtoCard>> toggelTrading(int cardId)
        {
            var result = _context.Cards.SingleOrDefault(c => c.CardId == cardId);
            if (result.Trading != 1)
            {
                result.Trading = 1;
                await _context.SaveChangesAsync();
            }else
            {
                result.Trading = 0;
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

=======
        // ------------------- Trade Request ----------------------

        // return number of rows affected, if already there then return zero row affected
        public async Task<int> AddTradeRequest(int cardID, int userID, int offerCardID)
        {
            int result = 0;
            if (!(await _context.TradeRequests.AnyAsync(tr => tr.CardId == cardID && tr.UserId == userID)))
            {
                var request = new TradeRequest
                {
                    CardId = cardID,
                    UserId = userID,
                    OfferCardId = offerCardID
                };
                await _context.TradeRequests.AddAsync(request);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public bool CheckTradable(int cardId)
        {
            return _context.Cards.Any(card => card.CardId == cardId && card.Trading == 1);
        }

        public async Task<IEnumerable<Requests>> GetSendRequest(int userid)
        {
            var result =  await (
                from tr in _context.TradeRequests
                join owner in _context.Cards on tr.CardId equals owner.CardId
                join dex in _context.Dices on owner.PokeId equals dex.PokeId
                where tr.UserId == userid
                select new
                {
                    requestid = tr.RequestId,
                    cardid = tr.CardId,
                    userid = tr.UserId,
                    offercardid = tr.OfferCardId,
                    pokeid = owner.PokeId,
                    pokemon = dex.Pokemon
                }).ToListAsync();

            List<Requests> records = new();

            foreach(var request in result)
            {
                records.Add(new(request.requestid, request.cardid, request.pokeid, request.pokemon, request.userid, request.offercardid));
            }
            return records;
        }

        public async Task<IEnumerable<Requests>> GetReceivedRequest(int userid)
        {
            var result = await(
               from tr in _context.TradeRequests
               join c in _context.Cards on tr.OfferCardId equals c.CardId
               join owner in _context.Cards on tr.CardId equals owner.CardId
               join dex in _context.Dices on c.PokeId equals dex.PokeId
               where owner.UserId == userid
               select new
               {
                   requestid = tr.RequestId,
                   cardid = tr.CardId,
                   userid = tr.UserId,
                   offercardid = tr.OfferCardId,
                   pokeid = owner.PokeId,
                   pokemon = dex.Pokemon
               }).ToListAsync();

            List<Requests> records = new();

            foreach (var request in result)
            {
                records.Add(new(request.requestid, request.cardid, request.pokeid, request.pokemon, request.userid, request.offercardid));
            }
            return records;
        }
>>>>>>> 86cd6af306ff721a5df0d970ced509697b2967b5
    }
}