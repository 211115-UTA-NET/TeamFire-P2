namespace pokeApi.Models
{
    public class Requests
    {

        public int requestID { get; set; }
        public int cardID { get; set; }
        public int pokeID { get; set; }
        public string pokemon { get; set; }
        public int userID { get; set; }
        public int offerCardID { get; set; }

        public Requests(int requestid, int cardid, int pokeid, string pokemon, int userid, int offercardid)
        {
            requestID = requestid;
            cardID = cardid;
            pokeID = pokeid;
            this.pokemon = pokemon;
            userID = userid;
            offerCardID = offercardid;
        }
    }
}
