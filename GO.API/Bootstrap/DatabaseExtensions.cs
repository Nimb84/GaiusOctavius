using GO.DataAccess.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GO.API.Bootstrap
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection RegisterSqlDatabase(
			this IServiceCollection services,
			IConfiguration configuration) =>
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("GaiusBotDb")));
	}
}
