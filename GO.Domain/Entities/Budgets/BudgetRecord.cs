using System;
using System.ComponentModel.DataAnnotations.Schema;
using GO.Domain.Enums.Budgets;

namespace GO.Domain.Entities.Budgets
{
	[Table(nameof(BudgetRecord))]
	public sealed class BudgetRecord
		: BaseEntity
	{
		public CategoryType CategoryType { get; set; }

		public uint Amount { get; set; }

		public Guid BudgetId { get; set; }
		public Budget Budget { get; set; }
	}
}
