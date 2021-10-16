using System;
using System.Collections.Generic;
using GO.Queries.ResponseModels.Users;

namespace GO.Queries.ResponseModels.Budgets
{
	public sealed class BudgetResponse
	{
		public Guid Id { get; set; }

		public bool IsArchived { get; set; }

		public byte Payday { get; set; }

		public Guid CreatedById { get; set; }

		public List<UserShortResponse> Users { get; set; } = new();

		public List<BudgetRecordResponse> Records { get; set; } = new();
	}
}
