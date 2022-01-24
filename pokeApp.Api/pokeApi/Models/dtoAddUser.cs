using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoAddUser
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public string? pw { get; set; }
        [Required]
        public string? email { get; set; }
    }
}
