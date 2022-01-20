using System;
namespace pokeApi.Models
{
    public class dtoTradeRecord
    {

        public int tradeID { get; }
        public string offeredBy { get; }
        public int offeredByID { get; }
        public string redeemedBy { get; }
        public int redeemedByID { get; }
        public int pokeID { get; }
        public string pokemon { get; }
        public int cardId { get; }

        public dtoTradeRecord(int tradeid, string offeredby, int offeredbyid, 
            string redeemedby, int redeemedbyid, int pokeid, string pokemon, int cardid)
        {
            tradeID = tradeid;
            offeredBy = offeredby;
            offeredByID = offeredbyid;
            redeemedBy = redeemedby;
            redeemedByID = redeemedbyid;
            pokeID = pokeid;
            this.pokemon = pokemon;
            cardId = cardid;
        }
    }
}

