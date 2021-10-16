using System;

namespace GO.Queries.ResponseModels.Users
{
	public sealed class UserShortResponse
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
