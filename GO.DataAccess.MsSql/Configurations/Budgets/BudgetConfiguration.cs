using GO.Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GO.DataAccess.MsSql.Configurations.Budgets
{
	internal sealed class BudgetConfiguration
		: IEntityTypeConfiguration<Budget>
	{
		public void Configure(EntityTypeBuilder<Budget> builder)
		{
			builder.HasKey(budget => budget.Id);

			builder.HasMany(budget => budget.Records)
				.WithOne(record => record.Budget)
				.HasForeignKey(group => group.BudgetId);
		}
	}
}
