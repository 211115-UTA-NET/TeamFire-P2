using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class Dex
    {
        public Dex()
        {
            Cards = new HashSet<Card>();
        }

        public int PokeId { get; set; }
        public string Pokemon { get; set; } = null!;

        public virtual ICollection<Card> Cards { get; set; }
    }
}
