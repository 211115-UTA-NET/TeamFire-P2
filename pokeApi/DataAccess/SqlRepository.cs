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
        //public async Task<IEnumerable<User>> GetUsersAsync(string name)
        //{
        //    List<User> result = new List<User>();

        //    using SqlConnection connection = new(_connectionString);
        //    connection.Open();

        //    using SqlCommand cmd = new(
        //                @"SELECT * FROM shop.Users WHERE userName=@sortName;",
        //        connection);

        //    cmd.Parameters.AddWithValue("@sortName", name);

        //    using SqlDataReader reader = cmd.ExecuteReader();

        //    // get trx from db
        //    while (reader.Read())
        //    {
        //        string Name = reader["userName"].ToString();
        //        string ID = reader["userID"].ToString();
        //        result.Add(new(Name, ID));
        //        Console.WriteLine($"{Name}'s userID: {ID}");

        //    }

        //    return result;
        //}

        public async Task<IEnumerable<dtoUser>> AddNewUserAsync(string user)
        {
            List<dtoUser> result = new List<dtoUser>();
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"INSERT INTO poke.Users (userName)
                                SELECT * FROM(SELECT (@thisName) AS userName) AS temp
                                WHERE NOT EXISTS (Select *from poke.Users where userName = (@thisName));
                                SELECT * FROM poke.Users
                                WHERE userName=@thisName;";
            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@thisName", user);

            using SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                string userName = reader["userName"].ToString();
                string userID = reader["userID"].ToString();
                result.Add(new(userName, userID));
                Console.WriteLine($"{userName}'s userID: {userID}");

            }

            return result;
        }

        //================= GET CARDS =========================//
        public async Task<IEnumerable<dtoCard>> GetCardsAsync(string name)
        {
            List<dtoCard> result = new List<dtoCard>();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            using SqlCommand cmd = new(
                        @"SELECT * FROM poke.Cards
                          WHERE cardOwner = @sortName;",

            connection);

            cmd.Parameters.AddWithValue("@sortName", name);

            using SqlDataReader reader = cmd.ExecuteReader();

            // get trx from db
            while (reader.Read())
            {
                string cardID = reader["cardID"].ToString();
                string pokemon = reader["pokemon"].ToString();
                string cardOwner = reader["cardOwner"].ToString();
                bool trading = (bool)reader["trading"];
                result.Add(new(cardID, pokemon, cardOwner, trading));
                Console.WriteLine($"{pokemon} number: {cardID} is owned by {cardOwner}.\nTrade being offered: {trading}");

            }

            return result;
        }

        //================== SET INVENTORY ======//
        public async Task<IEnumerable<dtoCard>> UpdateInventoryAsync(string newOwner, int cardUPC)
        {
            List<dtoCard> result = new List<dtoCard>();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string cmdText = @"UPDATE poke.Cards 
                            SET cardOwner = @newOwner 
                            WHERE cardID = @cardUPC;

                            UPDATE poke.Cards 
                            SET trading = @isTrading 
                            WHERE cardID = @cardUPC;

                            SELECT * FROM poke.Cards
                            WHERE cardOwner=@newOwner;";
            using SqlCommand cmd = new(cmdText, connection);

            // ado.net requires you to use DBNull instead of null when you mean a SQL NULL value
            cmd.Parameters.AddWithValue("@newOwner", newOwner);
            cmd.Parameters.AddWithValue("@cardUPC", cardUPC);

            using SqlDataReader reader = cmd.ExecuteReader();

            // get trx from db
            while (reader.Read())
            {
                string cardID = reader["cardID"].ToString();
                string pokemon = reader["pokemon"].ToString();
                string cardOwner = reader["cardOwner"].ToString();
                bool trading = (bool)reader["trading"];
                result.Add(new(cardID, pokemon, cardOwner, trading));
                Console.WriteLine($"{pokemon} number: {cardID} is owned by {cardOwner}.\nTrade being offered: {trading}");

            }

            Console.WriteLine($"Inventory Updated");

            return result;
        }


        //================= GET TRADES ========//
        public async Task<IEnumerable<dtoTrade>> GetTradesDetailsAsync(string name)
        {
            List<dtoTrade> result = new List<dtoTrade>();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            using SqlCommand cmd = new(
                        @"SELECT * FROM poke.Trades
                        WHERE offeredBy = @sortName
                        OR redeemedBy = @sortName;",
                connection);

            cmd.Parameters.AddWithValue("@sortName", name);

            using SqlDataReader reader = cmd.ExecuteReader();

            // get trx from db
            while (reader.Read())
            {
                int tradeID = (int)reader["tradeID"];
                string cardID = reader["cardID"].ToString();
                string pokemon = reader["pokemon"].ToString();
                string offeredBy = reader["offeredBy"].ToString();
                string redeemedBy = reader["redeemedBy"].ToString();
                result.Add(new(tradeID, cardID, pokemon, offeredBy, redeemedBy));
                Console.WriteLine($"Trade id: {tradeID} - {pokemon}/{cardID}\nTraded to {redeemedBy} by {offeredBy}.");


            }

            return result;
        }

        //==================POST TRADE=======// --needs to be corrected b/c there could be multiple trades w/ same card number -should just use timestamp for re-selection
        public async Task<IEnumerable<dtoTrade>> AddNewTradesAsync(string cardId, string pokemonName, string seller, string buyer)
        {
            List<dtoTrade> result = new List<dtoTrade>();
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            // assume the order exist already in the DB
            string cmdText = @"INSERT INTO poke.Trades ( cardID, pokemon, offeredBy, redeemedBy)
                               VALUES (  @thisID, @thisPokemon, @thisSeller , @thisBuyer);

                                SELECT * FROM poke.Trades
                                WHERE cardID = @thisID";
            using SqlCommand cmd = new(cmdText, connection);

            // ado.net requires you to use DBNull instead of null when you mean a SQL NULL value
            cmd.Parameters.AddWithValue("@thisID", cardId);
            cmd.Parameters.AddWithValue("@thisPokemon", pokemonName);
            cmd.Parameters.AddWithValue("@thisSeller", seller);
            cmd.Parameters.AddWithValue("@thisBuyer", buyer);
            //cmd.ExecuteNonQuery();
            //connection.Close();

            using SqlDataReader reader = cmd.ExecuteReader();

            // get trx from db
            while (reader.Read())
            {
                int tradeID = (int)reader["tradeID"];
                string cardID = reader["cardID"].ToString();
                string pokemon = reader["pokemon"].ToString();
                string offeredBy = reader["offeredBy"].ToString();
                string redeemedBy = reader["redeemedBy"].ToString();
                result.Add(new(tradeID, cardID, pokemon, offeredBy, redeemedBy));
                Console.WriteLine($"Trade id: {tradeID} - {pokemon}/{cardID}\nTraded to {redeemedBy} by {offeredBy}.");


            }

            return result;
        }

        

        

    }
}

