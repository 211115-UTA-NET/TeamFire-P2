using System;
namespace pokeApi.Models
{
	public class dtoCard
	{

		public string cardID { get; set; }
		public string pokemon { get; set; }
		public string cardOwner { get; set; }
		public bool trading { get; set; }

        public dtoCard(string cardID, string pokemon, string cardOwner, bool trading)
        {
            this.cardID = cardID;
            this.pokemon = pokemon;
            this.cardOwner = cardOwner;
            this.trading = trading;
        }
	}
}

