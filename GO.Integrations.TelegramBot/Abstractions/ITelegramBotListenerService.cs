using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Abstractions
{
	public interface ITelegramBotListenerService
	{
		Task EchoUpdateAsync(Update model, CancellationToken cancellationToken);
	}
}
