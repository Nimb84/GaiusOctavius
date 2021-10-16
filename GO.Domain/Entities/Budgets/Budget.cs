using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GO.Domain.Entities.Budgets
{
	[Table(nameof(Budget))]
	public sealed class Budget
		: BaseEntity
	{
		public bool IsArchived { get; set; }

		public byte Payday { get; set; }

		public List<BudgetsUsersRelation> Users { get; set; }

		public List<BudgetRecord> Records { get; set; } = new();
	}
}
