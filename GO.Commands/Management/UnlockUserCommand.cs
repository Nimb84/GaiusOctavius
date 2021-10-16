using System;
using MediatR;

namespace GO.Commands.Management
{
	public sealed class UnlockUserCommand
		: IRequest<Unit>
	{
		public Guid UserId { get; }

		public Guid CurrentUserId { get; }

		public UnlockUserCommand(Guid userId, Guid currentUserId)
		{
			UserId = userId;
			CurrentUserId = currentUserId;
		}
	}
}
