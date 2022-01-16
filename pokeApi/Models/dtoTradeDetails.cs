using System;
namespace pokeApi.Models
{
	public class dtoTradeDetails
	{
		public int tradeID { get; set; }
		public string offeredBy { get; set; }
		public string redeemedBy { get; set; }
		public int userID { get; set; }

        public dtoTradeDetails(int tradeID, string offeredBy, string redeemedBy, int userID)
        {
            this.tradeID = tradeID;
            this.offeredBy = offeredBy;
            this.redeemedBy = redeemedBy;
            this.userID = userID;
        }
    }
}

