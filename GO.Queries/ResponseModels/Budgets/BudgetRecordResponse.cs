using System;
using GO.Domain.Enums.Budgets;

namespace GO.Queries.ResponseModels.Budgets
{
	public sealed class BudgetRecordResponse
	{
		public Guid Id { get; set; }

		public CategoryType CategoryType { get; set; }

		public uint Amount { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTimeOffset CreatedDate { get; set; }
	}
}
