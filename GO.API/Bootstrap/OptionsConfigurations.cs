using GO.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GO.API.Bootstrap
{
	public static class OptionsConfigurations
	{
		public static IServiceCollection RegisterOptions(
			this IServiceCollection services,
			IConfiguration configuration) =>
			services
				.Configure<ConfigurationRoot>(configuration)
				.Configure<TelegramBotSettings>(configuration.GetSection("TelegramBotSettings"))
				.Configure<AppSettings>(configuration.GetSection("AppSettings"))
				.AddOptions();
	}
}
