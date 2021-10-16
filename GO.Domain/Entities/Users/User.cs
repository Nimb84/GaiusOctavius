using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;

namespace GO.Domain.Entities.Users
{
	[Table(nameof(User))]
	public sealed class User
		: BaseEntity
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Scopes Scopes { get; set; }

		public bool IsLocked { get; set; }

		public bool IsArchived { get; set; }

		public List<BudgetsUsersRelation> Budgets { get; set; } = new();

		public List<UserConnection> Connections { get; set; } = new();
	}
}
