using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Domain.Exceptions;
using GO.Domain.Options;
using GO.Integrations.TelegramBot.Abstractions;
using GO.Integrations.TelegramBot.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Services
{
	internal class TelegramBotListenerService
		: ITelegramBotListenerService
	{
		private readonly IBotBehaviorFactory _botBehaviorFactory;
		private readonly TelegramBotSettings _telegramBotSettings;
		private readonly ITelegramBotClientService _telegramBotClientService;

		public TelegramBotListenerService(
			IBotBehaviorFactory botBehaviorFactory,
			IOptions<TelegramBotSettings> telegramBotSettings,
			ITelegramBotClientService telegramBotClientService)
		{
			_botBehaviorFactory = botBehaviorFactory;
			_telegramBotSettings = telegramBotSettings.Value;
			_telegramBotClientService = telegramBotClientService;
		}

		public async Task EchoUpdateAsync(Update model, CancellationToken cancellationToken)
		{
			try
			{
				await _botBehaviorFactory.HandleUpdateAsync(model, cancellationToken);
			}
			catch (GoException ex)
			{
				await _telegramBotClientService.SendTextMessageAsync(model.GetChatId(), ex.Title, cancellationToken);
			}
			catch (Exception ex)
			{
				await _telegramBotClientService.SendTextMessageAsync(_telegramBotSettings.LogChatId, ex.Message, cancellationToken);
			}
		}
	}
}
