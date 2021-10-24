using System;
using GO.Domain.Enums.Management;

namespace GO.Queries.ResponseModels.Users
{
	public sealed class UserResponse
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Scopes Scopes { get; set; }

		public long ConnectionId { get; set; }

		public string Nickname { get; set; }

		public Scopes? CurrentScope { get; set; }
	}
}
