using System;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using Microsoft.EntityFrameworkCore;

namespace GO.UnitTests.Builders
{
	internal sealed class ApplicationDbContextTest
	{
		private readonly DbContextOptions<ApplicationDbContext> _options;

		public ApplicationDbContextTest(string dbName) =>
			_options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(dbName)
				.Options;

		public async Task<TEntity> ExecuteWithTestContextAsync<TEntity>(
			Func<ApplicationDbContext, Task<TEntity>> func)
		{
			await using var context = new ApplicationDbContext(_options);

			return await func(context);
		}
	}
}
