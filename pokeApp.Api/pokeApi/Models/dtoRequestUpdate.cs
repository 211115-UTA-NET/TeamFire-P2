using System.ComponentModel.DataAnnotations;

namespace pokeApi.Models
{
    public class dtoRequestUpdate
    {
        [Required]
        public int RequestID { get; set; }
        [Required]
        public string? RequestStatus { get; set; }

        //public dtoRequestUpdate(int requestID, string? requestStatus)
        //{
        //    this.RequestID = requestID;
        //    this.RequestStatus = requestStatus;
        //}
    }
}

