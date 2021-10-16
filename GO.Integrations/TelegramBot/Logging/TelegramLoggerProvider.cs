using GO.Integrations.TelegramBot.Abstractions;
using Microsoft.Extensions.Logging;

namespace GO.Integrations.TelegramBot.Logging
{
	internal class TelegramLoggerProvider
		: ILoggerProvider
	{
		private readonly ITelegramBotListenerService _telegramBotListenerService;

		public TelegramLoggerProvider(ITelegramBotListenerService telegramBotListenerService)
		{
			_telegramBotListenerService = telegramBotListenerService;
		}

		public ILogger CreateLogger(string category) => new TelegramLogger(_telegramBotListenerService);

		public void Dispose()
		{
		}
	}
}
