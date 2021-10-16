using GO.Domain.Options;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Abstractions.Behaviors;
using GO.Integrations.TelegramBot.Behaviors;
using GO.Integrations.TelegramBot.Constants;
using GO.Integrations.TelegramBot.Logging;
using GO.Integrations.TelegramBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace GO.Integrations.TelegramBot.Bootstrap
{
	public static class TelegramBotConfigurations
	{
		public static void RegisterTelegramBot(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTelegramServices();
			//services.AddTelegramLogger();

			services.AddHttpClient(TelegramConstants.HttpClientName)
				.AddTypedClient<ITelegramBotClient>(httpClient
					=> new TelegramBotClient(GetBotToken(configuration), httpClient));

			services.AddHostedService<WebhookSubscribeService>();
		}

		private static void AddTelegramServices(this IServiceCollection services)
		{
			services.AddTransient<IBudgetBotBehavior, BudgetBotBehavior>();
			services.AddTransient<IManagementBotBehavior, ManagementBotBehavior>();

			services.AddTransient<IBotBehaviorFactory, BotBehaviorFactory>();
			services.AddTransient<ITelegramBotListenerService, TelegramBotListenerService>();
			services.AddTransient<ITelegramBotClientService, TelegramBotClientService>();
		}

		public static void AddTelegramLogger(this IServiceCollection services)
		{
			services.AddLogging(loggingBuilder =>
				loggingBuilder.AddProvider(new TelegramLoggerProvider(loggingBuilder.Services.BuildServiceProvider()
					.GetRequiredService<ITelegramBotListenerService>())));
		}

		private static string GetBotToken(IConfiguration configuration) =>
			configuration
				.GetSection("TelegramBotSettings")
				.Get<TelegramBotSettings>()
				.Token;
	}
}
