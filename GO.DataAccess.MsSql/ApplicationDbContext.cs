using GO.DataAccess.MsSql.Extensions;
using GO.Domain.Entities.Budgets;
using GO.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GO.DataAccess.MsSql
{
	public sealed class ApplicationDbContext
		: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<UserConnection> UserConnections { get; set; }

		public DbSet<Budget> Budgets { get; set; }
		public DbSet<BudgetRecord> BudgetRecords { get; set; }
		public DbSet<BudgetsUsersRelation> BudgetsUsers { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public ApplicationDbContext()
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyApplicationConfigurations();

#if DEBUG
			builder.ApplyApplicationSeeds();
#endif

			base.OnModelCreating(builder);
		}
	}
}
