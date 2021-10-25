using System;
using MediatR;

namespace GO.Commands.Management
{
	public sealed record RegisterTelegramUserCommand(
			Guid UserId,
			string FirstName,
			string LastName,
			string NickName,
			long TelegramId)
		: IRequest<Unit>;
}
