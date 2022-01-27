using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class TradeRequest
    {
        public int RequestId { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public int OfferCardId { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual Card OfferCard { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
