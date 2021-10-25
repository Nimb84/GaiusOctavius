using System;
using System.Collections.Generic;

namespace GO.Domain.Models
{
	public record PagedQueryFilter
	{
		public List<Guid> UserIdList { get; init; } = new();

		public DateTimeOffset From { get; init; }

		public DateTimeOffset To { get; init; }

		public ushort Skip { get; init; }

		public ushort Take { get; init; }
	}
}
