using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
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


        //================= GET TRADES ========//
        //public async Task<IEnumerable<dtoTradeRecord>> GetTradesDetailsAsync(int userId)
        //{
        //    List<dtoTradeRecord> result = new List<dtoTradeRecord>();

        //    using SqlConnection connection = new(_connectionString);
        //    connection.Open();

        //    using SqlCommand cmd = new(
        //                @"SELECT * FROM poke.Trades
        //                WHERE offeredBy = @sortName
        //                OR redeemedBy = @sortName;",
        //        connection);

        //    cmd.Parameters.AddWithValue("@sortName", userId);

        //    using SqlDataReader reader = cmd.ExecuteReader();

        //    // get trx from db
        //    while (reader.Read())
        //    {
        //        int tradeID = (int)reader["tradeID"];
        //        string cardID = reader["cardID"].ToString();
        //        string pokemon = reader["pokemon"].ToString();
        //        string offeredBy = reader["offeredBy"].ToString();
        //        string redeemedBy = reader["redeemedBy"].ToString();
        //        result.Add(new(tradeID, cardID, pokemon, offeredBy, redeemedBy));
        //        Console.WriteLine($"Trade id: {tradeID} - {pokemon}/{cardID}\nTraded to {redeemedBy} by {offeredBy}.");


        //    }

        //    return result;
        //}

        ////==================POST TRADE=======// --needs to be corrected b/c there could be multiple trades w/ same card number -should just use timestamp for re-selection
        //public async Task<IEnumerable<dtoTrade>> AddNewTradesAsync(string cardId, string pokemonName, string seller, string buyer)
        //{
        //    List<dtoTrade> result = new List<dtoTrade>();
        //    using SqlConnection connection = new(_connectionString);
        //    connection.Open();

        //    // assume the order exist already in the DB
        //    string cmdText = @"INSERT INTO poke.Trades ( cardID, pokemon, offeredBy, redeemedBy)
        //                       VALUES (  @thisID, @thisPokemon, @thisSeller , @thisBuyer);

        //                        SELECT * FROM poke.Trades
        //                        WHERE cardID = @thisID";
        //    using SqlCommand cmd = new(cmdText, connection);

        //    // ado.net requires you to use DBNull instead of null when you mean a SQL NULL value
        //    cmd.Parameters.AddWithValue("@thisID", cardId);
        //    cmd.Parameters.AddWithValue("@thisPokemon", pokemonName);
        //    cmd.Parameters.AddWithValue("@thisSeller", seller);
        //    cmd.Parameters.AddWithValue("@thisBuyer", buyer);
        //    //cmd.ExecuteNonQuery();
        //    //connection.Close();

        //    using SqlDataReader reader = cmd.ExecuteReader();

        //    // get trx from db
        //    while (reader.Read())
        //    {
        //        int tradeID = (int)reader["tradeID"];
        //        string cardID = reader["cardID"].ToString();
        //        string pokemon = reader["pokemon"].ToString();
        //        string offeredBy = reader["offeredBy"].ToString();
        //        string redeemedBy = reader["redeemedBy"].ToString();
        //        result.Add(new(tradeID, cardID, pokemon, offeredBy, redeemedBy));
        //        Console.WriteLine($"Trade id: {tradeID} - {pokemon}/{cardID}\nTraded to {redeemedBy} by {offeredBy}.");


        //    }

        //    return result;
        //}





    }
}

