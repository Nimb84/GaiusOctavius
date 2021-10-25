using System;
using System.Collections.Generic;
using GO.Queries.ResponseModels.Users;

namespace GO.Queries.ResponseModels.Budgets
{
	public sealed record BudgetResponse
	{
		public Guid Id { get; init; }

		public bool IsArchived { get; init; }

		public byte Payday { get; init; }

		public Guid CreatedById { get; init; }

		public List<UserShortResponse> Users { get; init; } = new();

		public List<BudgetRecordResponse> Records { get; init; } = new();
	}
}
