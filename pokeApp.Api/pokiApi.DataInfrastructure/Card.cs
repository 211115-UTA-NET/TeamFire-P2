using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class Card
    {
        public Card()
        {
            TradeDetails = new HashSet<TradeDetail>();
            TradeRequestCards = new HashSet<TradeRequest>();
            TradeRequestOfferCards = new HashSet<TradeRequest>();
        }

        public int CardId { get; set; }
        public int UserId { get; set; }
        public int PokeId { get; set; }
        public int? Trading { get; set; }

        public virtual Dex Poke { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<TradeDetail> TradeDetails { get; set; }
        public virtual ICollection<TradeRequest> TradeRequestCards { get; set; }
        public virtual ICollection<TradeRequest> TradeRequestOfferCards { get; set; }
    }
}
