using System;
using GO.Domain.Enums.Budgets;

namespace GO.Queries.ResponseModels.Budgets
{
	public sealed record BudgetRecordResponse
	{
		public Guid Id { get; init; }

		public uint Amount { get; init; }

		public CategoryType CategoryType { get; init; }

		public DateTimeOffset CreatedDate { get; init; }

		public Guid CreatedBy { get; init; }
	}
}
