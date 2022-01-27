using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoUpdateCard
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public int cardId { get; set; }
        
    }
}

