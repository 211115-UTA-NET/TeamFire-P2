using System;
namespace pokeApi.Models
{
	public class dtoTradeRecord
	{

		public int tradeID { get; set; }
		public string offeredBy { get; set; }
		public string redeemedBy { get; set; }

        public dtoTradeRecord(int tradeID, string offeredBy, string redeemedBy)
        {
            this.tradeID = tradeID;
            this.offeredBy = offeredBy;
            this.redeemedBy = redeemedBy;
        }
    }
}

