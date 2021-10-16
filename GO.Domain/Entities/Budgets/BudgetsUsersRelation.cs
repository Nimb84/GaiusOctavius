using System;
using System.ComponentModel.DataAnnotations.Schema;
using GO.Domain.Entities.Users;

namespace GO.Domain.Entities.Budgets
{
	[Table("BudgetsUsers")]
	public sealed class BudgetsUsersRelation
	{
		public bool IsDisabled { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid BudgetId { get; set; }
		public Budget Budget { get; set; }
	}
}
