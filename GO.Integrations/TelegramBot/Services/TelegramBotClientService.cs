using System.Threading;
using System.Threading.Tasks;
using GO.Integrations.TelegramBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GO.Integrations.TelegramBot.Services
{
	internal class TelegramBotClientService
		: ITelegramBotClientService
	{
		private readonly ITelegramBotClient _telegramBotClient;

		public TelegramBotClientService(
			ITelegramBotClient telegramBotClient)
		{
			_telegramBotClient = telegramBotClient;
		}

		public Task<Message> SendTextMessageAsync(
			long chatId,
			string message,
			CancellationToken cancellationToken = default) =>
			SendTextMessageAsync(
				chatId,
				message,
				null,
				cancellationToken: cancellationToken);

		public Task<Message> SendTextMessageAsync(
			long chatId,
			string message,
			IReplyMarkup markup = null,
			CancellationToken cancellationToken = default) =>
			_telegramBotClient.SendTextMessageAsync(
				chatId,
				message,
				replyMarkup: markup,
				cancellationToken: cancellationToken);

		public Task<Message> UpdateTextMessageAsync(
			long chatId,
			int messageId,
			string message,
			CancellationToken cancellationToken = default) =>
			_telegramBotClient.EditMessageTextAsync(
				new ChatId(chatId),
				messageId,
				message,
				replyMarkup: null,
				cancellationToken: cancellationToken);

		public Task<Message> UpdateTextMessageAsync(
			long chatId,
			int messageId,
			string message,
			IReplyMarkup markup = null,
			CancellationToken cancellationToken = default) =>
			_telegramBotClient.EditMessageTextAsync(
				new ChatId(chatId),
				messageId,
				message,
				replyMarkup: markup as InlineKeyboardMarkup,
				cancellationToken: cancellationToken);

		public Task DeleteMessageAsync(
			long chatId,
			int messageId,
			CancellationToken cancellationToken = default) =>
			_telegramBotClient.DeleteMessageAsync(new ChatId(chatId), messageId, cancellationToken);
	}
}
