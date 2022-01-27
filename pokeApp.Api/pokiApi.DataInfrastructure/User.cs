using System;
using System.Collections.Generic;

namespace pokiApi.DataInfrastructure
{
    public partial class User
    {
        public User()
        {
            Cards = new HashSet<Card>();
            CompletedTradeOfferedByNavigations = new HashSet<CompletedTrade>();
            CompletedTradeRedeemedByNavigations = new HashSet<CompletedTrade>();
            TradeRequests = new HashSet<TradeRequest>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<CompletedTrade> CompletedTradeOfferedByNavigations { get; set; }
        public virtual ICollection<CompletedTrade> CompletedTradeRedeemedByNavigations { get; set; }
        public virtual ICollection<TradeRequest> TradeRequests { get; set; }
    }
}
