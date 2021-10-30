using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GO.Integrations.TelegramBot.Abstractions;
using Microsoft.Extensions.Logging;

namespace GO.Integrations.TelegramBot.Logging
{
	internal class TelegramLogger : ILogger
	{
		private readonly ITelegramBotListenerService _telegramBotListenerService;

		public TelegramLogger(ITelegramBotListenerService telegramBotListenerService)
		{
			_telegramBotListenerService = telegramBotListenerService;
		}

		public IDisposable BeginScope<TState>(TState state) => null;

		public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Critical;

		public void Log<TState>(LogLevel logLevel, EventId eventId,
			TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (IsEnabled(logLevel))
			{
				var msg = formatter(state, exception);
			}
		}
	}
}
