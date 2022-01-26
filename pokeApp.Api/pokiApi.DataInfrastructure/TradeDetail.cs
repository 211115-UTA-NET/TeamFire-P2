using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class TradeDetail
    {
        public int Id { get; set; }
        public int TradeId { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual CompletedTrade Trade { get; set; } = null!;
    }
}
