using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class CompletedTrade
    {
        public CompletedTrade()
        {
            TradeDetails = new HashSet<TradeDetail>();
        }

        public int TradeId { get; set; }
        public int OfferedBy { get; set; }
        public int RedeemedBy { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public virtual User OfferedByNavigation { get; set; } = null!;
        public virtual User RedeemedByNavigation { get; set; } = null!;
        public virtual ICollection<TradeDetail> TradeDetails { get; set; }
    }
}
