using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoRequest
    {
        [Required]
        public int cardID { get; set; }
        [Required]
        public int userID { get; set; }
        [Required]
        public int offerCardID { get; set; }
        [Required]
        public int targetUserID { get; set; }
    }
}

