using System;
using System.Collections.Generic;

namespace GO.Domain.Models
{
	public class PagedQueryFilter
	{
		public List<Guid> UserIdList { get; set; } = new();

		public DateTimeOffset From { get; set; }

		public DateTimeOffset To { get; set; }

		public ushort Skip { get; set; }

		public ushort Take { get; set; }
	}
}
