using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoAddDetail
    {
        [Required]
        public int tradeId { get; set; }
        [Required]
        public int cardId { get; set; }
        [Required]
        public int offeredId { get; set; }
    }
}

