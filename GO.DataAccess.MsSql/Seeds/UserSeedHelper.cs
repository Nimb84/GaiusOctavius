using System;
using System.Collections.Generic;
using GO.Domain.Entities.Budgets;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Management;
using GO.Domain.Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace GO.DataAccess.MsSql.Seeds
{
	internal static class UserSeedHelper
	{
		public static ModelBuilder AddUserSeedData(this ModelBuilder builder)
		{
			builder
				.Entity<User>()
				.HasData(new List<User>
				{
					new()
					{
						Id = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
						FirstName = "Dmytro",
						LastName = "😇",
						Scopes = Scopes.Administration | Scopes.Management | Scopes.Budget,
						CreatedDate = DateTimeOffset.UtcNow,
						CreatedBy = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8")
					}
				});

			builder
				.Entity<UserConnection>()
				.HasData(new List<UserConnection>()
				{
					new()
					{
						Id = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
						CreatedBy = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
						CreatedDate = DateTimeOffset.UtcNow,
						NickName = "Nimb84",
						ConnectionId = 428296956,
						Type = ConnectionType.Telegram,
						CurrentScope = Scopes.Budget,
						UserId = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8")
					}
				});

			builder
				.Entity<Budget>()
				.HasData(new List<Budget>
			{
				new ()
				{
					Id = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
					CreatedBy = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
					CreatedDate = DateTimeOffset.UtcNow,
					IsArchived = false
				}
			});

			builder
				.Entity<BudgetsUsersRelation>()
				.HasData(new List<BudgetsUsersRelation>
			{
				new ()
				{
					IsDisabled = false,
					UserId = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
					BudgetId = Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8"),
				}
			});

			return builder;
		}
	}
}
