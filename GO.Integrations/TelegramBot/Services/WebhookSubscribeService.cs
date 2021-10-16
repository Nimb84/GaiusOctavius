using System.Threading;
using System.Threading.Tasks;
using GO.Domain.Options;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Constants;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace GO.Integrations.TelegramBot.Services
{
	internal class WebhookSubscribeService
		: IHostedService
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly ITelegramBotClientService _telegramBotClientService;
		private readonly AppSettings _appSettings;
		private readonly TelegramBotSettings _telegramSettings;

		public WebhookSubscribeService(
			ITelegramBotClient telegramBotClient,
			ITelegramBotClientService telegramBotClientService,
			IOptions<TelegramBotSettings> telegramSettings,
			IOptions<AppSettings> appSettings)
		{
			_telegramBotClient = telegramBotClient;
			_telegramBotClientService = telegramBotClientService;
			_appSettings = appSettings.Value;
			_telegramSettings = telegramSettings.Value;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _telegramBotClient.SetWebhookAsync(
				$"{_appSettings.Domain}{_telegramSettings.WebhookUrl}",
				allowedUpdates: TelegramConstants.AllowedUpdateTypes,
				cancellationToken: cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				_telegramSettings.LogChatId,
				"Application started",
				cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);

			await _telegramBotClientService.SendTextMessageAsync(
				_telegramSettings.LogChatId,
				"Application stopped",
				cancellationToken);
		}
	}
}
