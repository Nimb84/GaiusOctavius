using System;
using GO.Domain.Enums.Management;

namespace GO.Queries.ResponseModels.Users
{
	public sealed record UserResponse
	{
		public Guid Id { get; init; }

		public string FirstName { get; init; }

		public string LastName { get; init; }

		public Scopes Scopes { get; init; }

		public long ConnectionId { get; init; }

		public string Nickname { get; init; }

		public Scopes? CurrentScope { get; init; }
	}
}
