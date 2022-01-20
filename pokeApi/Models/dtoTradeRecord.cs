using System;
namespace pokeApi.Models
{
    public class dtoTradeRecord
    {

        public int tradeID { get; set; }
        public string offeredBy { get; set; }
        public int offeredByID { get; set; }
        public string redeemedBy { get; set; }
        public int redeemedByID { get; set; }
        public int pokeID { get; set; }
        public string pokemon { get; set; }
        public int cardId { get; set; }



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

