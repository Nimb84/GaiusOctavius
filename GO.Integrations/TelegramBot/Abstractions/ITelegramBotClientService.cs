using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GO.Integrations.TelegramBot.Abstractions
{
	internal interface ITelegramBotClientService
	{
		Task<Message> SendTextMessageAsync(
			long chatId,
			string message,
			CancellationToken cancellationToken = default);

		Task<Message> SendTextMessageAsync(
			long chatId,
			string message,
			IReplyMarkup markup = null,
			CancellationToken cancellationToken = default);

		Task<Message> UpdateTextMessageAsync(
			long chatId,
			int messageId,
			string message,
			IReplyMarkup markup = null,
			CancellationToken cancellationToken = default);

		Task DeleteMessageAsync(
			long chatId,
			int messageId,
			CancellationToken cancellationToken = default);
	}
}
