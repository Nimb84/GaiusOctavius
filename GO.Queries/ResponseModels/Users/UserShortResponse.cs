using System;

namespace GO.Queries.ResponseModels.Users
{
	public sealed record UserShortResponse
	{
		public Guid Id { get; init; }

		public string FirstName { get; init; }

		public string LastName { get; init; }
	}
}
