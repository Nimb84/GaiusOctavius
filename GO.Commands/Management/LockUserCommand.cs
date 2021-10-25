using System;
using MediatR;

namespace GO.Commands.Management
{
	public sealed record LockUserCommand(Guid UserId, Guid CurrentUserId)
		: IRequest<Unit>;
}
