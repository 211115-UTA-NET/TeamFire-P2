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



        public dtoTradeRecord(int tradeID, string offeredBy, string redeemedBy)
        {
            this.tradeID = tradeID;
            this.offeredBy = offeredBy;
            this.redeemedBy = redeemedBy;
        }

        public dtoTradeRecord(int tradeID, int offeredByID, int redeemedByID)
        {
            this.tradeID = tradeID;
            this.offeredByID = offeredByID;
            this.redeemedByID = redeemedByID;
        }

        public dtoTradeRecord(string offeredBy, string pokemon)
        {
            this.offeredBy = offeredBy;
            this.pokemon = pokemon;
        }

        public dtoTradeRecord(int offeredByID, int redeemedByID)
        {
            this.offeredByID = offeredByID;
            this.redeemedByID = redeemedByID;
        }

        public dtoTradeRecord(int cardId)
        {
            this.cardId = cardId;
        }
    }
}

