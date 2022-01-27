using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoAddTrade
    {
        [Required]
        public int offeredByID { get; set; }
        [Required]
        public int recevedByID { get; set; }
    }
}

