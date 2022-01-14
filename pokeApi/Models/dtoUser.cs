using System;
namespace pokeApi.Models
{
	public class dtoUser
	{
		public string userID { get; set; }
		public string userName { get; set; }

		public dtoUser(string id, string name)
		{
			userID = id;
			userName = name;
		}
	}
}

