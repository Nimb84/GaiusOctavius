using GO.Domain.Constants;
using GO.Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GO.DataAccess.MsSql.Configurations.Budgets
{
	internal sealed class BudgetRecordConfiguration
		: IEntityTypeConfiguration<BudgetRecord>
	{
		public void Configure(EntityTypeBuilder<BudgetRecord> builder)
		{
			builder.HasKey(budget => budget.Id);

			builder.Property(record => record.Amount)
				.IsRequired();
		}
	}
}
