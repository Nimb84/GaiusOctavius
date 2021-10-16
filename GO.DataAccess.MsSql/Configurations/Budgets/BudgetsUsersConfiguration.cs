using GO.Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GO.DataAccess.MsSql.Configurations.Budgets
{
	internal sealed class BudgetsUsersConfiguration
		: IEntityTypeConfiguration<BudgetsUsersRelation>
	{
		public void Configure(EntityTypeBuilder<BudgetsUsersRelation> builder)
		{
			builder.HasKey(entity => new { entity.UserId, entity.BudgetId });

			builder.HasOne(entity => entity.User)
				.WithMany(user => user.Budgets)
				.HasForeignKey(entity => entity.UserId);

			builder.HasOne(entity => entity.Budget)
				.WithMany(budget => budget.Users)
				.HasForeignKey(entity => entity.BudgetId);
		}
	}
}
