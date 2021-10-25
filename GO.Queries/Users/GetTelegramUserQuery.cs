using GO.Queries.ResponseModels.Users;
using MediatR;

namespace GO.Queries.Users
{
	public sealed record GetTelegramUserQuery(long TelegramUserId)
		: IRequest<TelegramUserResponse>;
}
