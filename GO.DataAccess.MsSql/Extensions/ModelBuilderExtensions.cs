using GO.DataAccess.MsSql.Configurations.Budgets;
using GO.DataAccess.MsSql.Configurations.Users;
using GO.DataAccess.MsSql.Seeds;
using Microsoft.EntityFrameworkCore;

namespace GO.DataAccess.MsSql.Extensions
{
	internal static class ModelBuilderExtensions
	{
		public static ModelBuilder ApplyApplicationConfigurations(this ModelBuilder builder) =>
			builder
				.ApplyConfiguration(new UserConfiguration())
				.ApplyConfiguration(new UserConnectionConfiguration())
				.ApplyConfiguration(new BudgetConfiguration())
				.ApplyConfiguration(new BudgetRecordConfiguration())
				.ApplyConfiguration(new BudgetsUsersConfiguration());

		public static ModelBuilder ApplyApplicationSeeds(this ModelBuilder builder) =>
			builder
				.AddUserSeedData();
	}
}
