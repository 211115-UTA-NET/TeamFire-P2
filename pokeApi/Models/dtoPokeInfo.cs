using System;
namespace pokeApi.Models
{
	public class dtoPokeInfo
	{
		public int pokeID { get; set; }
		public string pokemon { get; set; }

        public dtoPokeInfo(int pokeID, string pokemon)
        {
            this.pokeID = pokeID;
            this.pokemon = pokemon;
        }
    }
}

