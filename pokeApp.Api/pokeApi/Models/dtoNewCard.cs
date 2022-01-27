using System.ComponentModel.DataAnnotations;
namespace pokeApi.Models
{
    public class dtoNewCard
    {
        [Required]
        public int userId { get; set; }
    }
}

