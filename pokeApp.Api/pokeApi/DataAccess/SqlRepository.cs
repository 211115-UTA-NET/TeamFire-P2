using Microsoft.Data.SqlClient;
using pokeApi.Models;

namespace pokeApi.Data
{
    public class SqlRepository : IRepository
    {



        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        //============== Get User===========//
        public async Task<IEnumerable<dtoUser>> GetUsersAsync(string name)
        {
            List<dtoUser> result = new List<dtoUser>();
            /**
             * if there's a Async version of the method, just replace with the Async one:
             * for example: connection.Open() has an Async version called connection.OpenAsync()
             *              reader.Read() has an Async version call reader.ReadAsync()
             *   And always await the async method.
             *   And always close the connection after you done.
             */
            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            
            using SqlCommand cmd = new(
                        @"SELECT * FROM poke.Users WHERE userName=@sortName;",
                connection);

            cmd.Parameters.AddWithValue("@sortName", name);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int ID = (int)reader["userID"];
                string Name = reader["userName"].ToString()!;
                string pw = reader["password"].ToString()!;
                string email = reader["email"].ToString()!;

                result.Add(new( ID, Name, pw, email));
                Console.WriteLine($"{Name}'s userID: {ID}");

            }
            await connection.CloseAsync();
            return result;
        }

        public async Task<IEnumerable<dtoUser>> AddNewUserAsync(string name, string pw, string Email)
        {
            List<dtoUser> result = new List<dtoUser>();
            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            string cmdText = @"INSERT INTO poke.Users (userName, password, email)
            SELECT * FROM(SELECT (@thisName) AS userName, (@thispw) as password, (@thisemail) as email) AS temp
            WHERE NOT EXISTS (Select *from poke.Users where userName = (@thisName));
                                SELECT * FROM poke.Users
                                WHERE userName=@thisName;";
            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@thisName", name);
            cmd.Parameters.AddWithValue("@thispw", pw);
            cmd.Parameters.AddWithValue("@thisemail", Email);


            using SqlDataReader reader = await cmd.ExecuteReaderAsync();


            while (await reader.ReadAsync())
            {
                int userID = (int)reader["userID"];
                string userName = reader["userName"].ToString()!;
                string password = reader["password"].ToString()!;
                string email = reader["email"].ToString()!;

                result.Add(new(userID, userName, password, email));
                Console.WriteLine($"{userName}'s userID: {userID}");

            }
            await connection.CloseAsync();
            return result;
        }

        //================= GET CARDS =========================//
        public async Task<IEnumerable<dtoCard>> GetCardsAsync(int userId)
        {
            List<dtoCard> result = new List<dtoCard>();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            using SqlCommand cmd = new(
                        @"SELECT cardID,poke.Cards.userID,userName,poke.Cards.pokeID,pokemon,trading
                        From poke.Cards 
                        INNER JOIN poke.Dex ON poke.Cards.pokeID = poke.Dex.pokeID
                        INNER JOIN poke.Users On poke.Cards.userID = poke.Users.userID
                        WHERE poke.Cards.userID = @sortID;",

            connection);

            cmd.Parameters.AddWithValue("@sortID", userId);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int cardID = (int)reader["cardID"];
                int userID = (int)reader["userID"];
                string userName = reader["userName"].ToString()!;
                int pokeID = (int)reader["pokeID"];
                string pokemon = reader["pokemon"].ToString()!;
                int trading = (int)reader["trading"];
                result.Add(new(cardID, userID, userName, pokeID, pokemon, trading));
                Console.WriteLine($"{pokemon} number: {cardID} is owned by {userName}.\nTrade being offered: {trading}");

            }
            await connection.CloseAsync();
            return result;
        }

        //================== UPDATE CARD OWNER ======//
        public async Task<IEnumerable<dtoCard>> UpdateCardOwnerAsync(int newOwner, int cardId)
        {
            List<dtoCard> result = new List<dtoCard>();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            string cmdText = @"UPDATE poke.Cards 
                            SET userID = @newOwner 
                            WHERE cardID = @cardID;

                            UPDATE poke.Cards 
                            SET trading = 0 
                            WHERE cardID = @cardID;

                            SELECT cardID, poke.Cards.userID,userName,poke.Cards.pokeID,pokemon,trading
                         From poke.Cards
                         INNER JOIN poke.Dex ON poke.Cards.pokeID = poke.Dex.pokeID
                        INNER JOIN poke.Users On poke.Cards.userID = poke.Users.userID
                        WHERE poke.Cards.userID = @newOwner AND poke.Cards.cardID = @cardID;
                        ";
            using SqlCommand cmd = new(cmdText, connection);

            // ado.net requires you to use DBNull instead of null when you mean a SQL NULL value
            cmd.Parameters.AddWithValue("@newOwner", newOwner);
            cmd.Parameters.AddWithValue("@cardID", cardId);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int cardID = (int)reader["cardID"];
                int userID = (int)reader["userID"];
                string userName = reader["userName"].ToString()!;
                int pokeID = (int)reader["pokeID"];
                string pokemon = reader["pokemon"].ToString()!;
                int trading = (int)reader["trading"];
                result.Add(new(cardID, userID, userName, pokeID, pokemon, trading));
                Console.WriteLine($"{pokemon} number: {cardID} is owned by {userName}.\nTrade being offered: {trading}");

            }

            Console.WriteLine($"Inventory Updated");
            await connection.CloseAsync();
            return result;
        }

        //------------SPECIFIC USER-----------------//
        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(string name)
        {
            List<dtoTradeRecord> result = new List<dtoTradeRecord>();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            using SqlCommand cmd = new(
                        @"select poke.CompletedTrades.tradeID, o.userName as offeredBy, r.userName as redeemedBy
                        from poke.CompletedTrades
                        join poke.Users o on  poke.CompletedTrades.offeredBy = o.userID
                        join poke.Users r on  poke.CompletedTrades.redeemedBy = r.userID
                        where (o.userName = @sortID or r.userName = @sortID );",
                connection);

            cmd.Parameters.AddWithValue("@sortID", name);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                string offeredBy = reader["offeredBy"].ToString()!;
                int tradeID = (int)reader["tradeID"];
                string redeemedBy = reader["redeemedBy"].ToString()!;
                result.Add(new(tradeID, offeredBy, redeemedBy));
                Console.WriteLine($"Trade id: {tradeID} - \nInitiator :{redeemedBy} - Redeemer: {offeredBy}.");


            }
            await connection.CloseAsync();

            return result;
        }

        //--------------------ALL RECENT TRADES----------------//
        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync()
        {
            List<dtoTradeRecord> result = new List<dtoTradeRecord>();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            using SqlCommand cmd = new(
                        @"select poke.CompletedTrades.tradeID, o.userName as offeredBy, r.userName as redeemedBy
                        from poke.CompletedTrades
                        join poke.Users o on  poke.CompletedTrades.offeredBy = o.userID
                        join poke.Users r on  poke.CompletedTrades.redeemedBy = r.userID;",
                connection);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int tradeID = (int)reader["tradeID"];
                string offeredBy = reader["offeredBy"].ToString()!;
                string redeemedBy = reader["redeemedBy"].ToString()!;
                result.Add(new(tradeID, offeredBy, redeemedBy));
                Console.WriteLine($"Trade id: {tradeID} - \nInitiator :{redeemedBy} - Redeemer: {offeredBy}.");

            }
            await connection.CloseAsync();
            return result;
        }


        //-----------GET TRADE DETAILS-----------//
        public async Task<IEnumerable<dtoTradeRecord>> GetRecentTradesAsync(int tradeId)
        {
            List<dtoTradeRecord> result = new List<dtoTradeRecord>();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            using SqlCommand cmd = new(
                        @"select poke.TradeDetail.tradeID, p.pokemon as cardID, o.userName
                    from poke.TradeDetail
                    join poke.Dex p on poke.TradeDetail.cardID = p.pokeID
                    join poke.Users o on  poke.TradeDetail.userID = o.userID
                    where tradeID = @sortId;",
                connection);

            cmd.Parameters.AddWithValue("@sortId", tradeId);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int tradeID = (int)reader["tradeID"];
                string pokemon = reader["cardID"].ToString()!;
                string offeredBy = reader["userName"].ToString()!;
                result.Add(new(tradeID, pokemon, offeredBy));
                Console.WriteLine($"{offeredBy} traded {pokemon}.");


            }
            await connection.CloseAsync();
            return result;
        }

        //-------------ADD TRADE RECORD----------///
        public async Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int offeredByID, int receivedByID)
        {
            List<dtoTradeRecord> result = new List<dtoTradeRecord>();
            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            string cmdText = @"INSERT into poke.CompletedTrades(offeredBy, redeemedBy)
                            OUTPUT INSERTED.tradeID
                            VALUES (@offeredId, @redeemedId);";
            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@offeredId", offeredByID);
            cmd.Parameters.AddWithValue("@redeemedId", receivedByID);


            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db

            while (await reader.ReadAsync())
            {
                // store tradeID
                int tradeID = (int)reader["tradeID"];
                result.Add(new(tradeID, offeredByID, receivedByID));
                Console.WriteLine($"Trade ID: {tradeID}");

            }
            await connection.CloseAsync();
            return result;
        }

        //-------------ADD TRADE DETAIL----------//


        public async Task<IEnumerable<dtoTradeRecord>> AddNewRecordAsync(int tradeId, int cardId, int offeredById)
        {
            List<dtoTradeRecord> result = new List<dtoTradeRecord>();
            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            string cmdText = @"INSERT INTO poke.TradeDetail( tradeID, cardID, userID)
                            OUTPUT  INSERTED.cardID
                            VALUES (@tradeId, @cardId, @offeredId);";
            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@tradeId", tradeId);
            cmd.Parameters.AddWithValue("@cardId", cardId);
            cmd.Parameters.AddWithValue("@offeredId", offeredById);


            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // get trx from db
            while (await reader.ReadAsync())
            {
                int cardID = (int)reader["cardID"];
                result.Add(new(cardID));
                Console.WriteLine($"{cardId} traded!");
            }
            await connection.CloseAsync();
            return result;
        }

    }



}


