using GO.Queries.ResponseModels.Users;
using MediatR;

namespace GO.Queries.Users
{
	public sealed class GetTelegramUserQuery
		: IRequest<TelegramUserResponse>
	{
		public long TelegramUserId { get; }

		public GetTelegramUserQuery(long telegramUserId)
		{
			TelegramUserId = telegramUserId;
		}
	}
}
