using System;
namespace pokeApi.Models
{
	public class dtoUser
	{
		public int userID { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public string email { get; set; }

        public dtoUser(int userID, string userName, string password, string email)
        {
            this.userID = userID;
            this.userName = userName;
            this.password = password;
            this.email = email;
        }
    }
}

