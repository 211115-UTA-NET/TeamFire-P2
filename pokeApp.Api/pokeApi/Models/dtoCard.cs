using System;

namespace pokeApi.Models
{
	public class dtoCard
	{

		public int cardID { get; set; }
		public int userID { get; set; }
        public string userName { get; set; }
        public int pokeID { get; set; }
        public string pokemon { get; set; }
		public int? trading { get; set; }

        public dtoCard(int cardID, int userID, string userName, int pokeID, string pokemon, int? trading)
        {
            this.cardID = cardID;
            this.userID = userID;
            this.userName = userName;
            this.pokeID = pokeID;
            this.pokemon = pokemon;
            this.trading = trading;
        }

        public static explicit operator dtoCard(Task<IEnumerable<dtoCard>> v)
        {
            throw new NotImplementedException();
        }
    }
}

