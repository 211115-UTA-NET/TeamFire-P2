using System;
namespace pokeApi.Models
{
	public class dtoTrade
	{

		public int tradeID { get; set; }
		public string cardID { get; set; }
		public string pokemon { get; set; }
		public string offeredBy { get; set; }
		public string redeemedBy { get; set; }

        public dtoTrade(int tradeID, string cardID, string pokemon, string offeredBy, string redeemedBy)
        {
            this.tradeID = tradeID;
            this.cardID = cardID;
            this.pokemon = pokemon;
            this.offeredBy = offeredBy;
            this.redeemedBy = redeemedBy;
        }

	}
}

