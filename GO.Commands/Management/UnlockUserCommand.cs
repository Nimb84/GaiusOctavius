using System;
using MediatR;

namespace GO.Commands.Management
{
	public sealed record UnlockUserCommand(Guid UserId, Guid CurrentUserId)
		: IRequest<Unit>;
}
