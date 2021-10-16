using System.Threading;
using System.Threading.Tasks;
using GO.Queries.ResponseModels.Users;
using Telegram.Bot.Types;

namespace GO.Integrations.TelegramBot.Abstractions.Behaviors
{
	internal interface IBaseMessageChatBotBehavior
	{
		Task HandleMessageAsync(
			TelegramUserResponse currentUser,
			Update model,
			CancellationToken cancellationToken = default);
	}
}
