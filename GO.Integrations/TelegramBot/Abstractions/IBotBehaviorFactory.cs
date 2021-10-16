using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Abstractions
{
	internal interface IBotBehaviorFactory
	{
		Task HandleUpdateAsync(Update model, CancellationToken cancellationToken = default);
	}
}
