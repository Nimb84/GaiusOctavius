using GO.Commands.Handlers.Management;
using GO.Queries.Handlers.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GO.API.Bootstrap
{
	public static class MediatRBootstrapConfigurations
	{
		public static IServiceCollection RegisterMediatR(this IServiceCollection services) =>
			services
				.AddMediatRQueries()
				.AddMediatRCommands();

		private static IServiceCollection AddMediatRCommands(this IServiceCollection services) =>
			services.AddMediatR(typeof(RegisterTelegramUserHandler).Assembly);

		private static IServiceCollection AddMediatRQueries(this IServiceCollection services) =>
			services.AddMediatR(typeof(GetUserHandler).Assembly);
	}
}
