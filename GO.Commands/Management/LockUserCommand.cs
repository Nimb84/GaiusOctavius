using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GO.Commands.Management
{
	public sealed class LockUserCommand
		: IRequest<Unit>
	{
		public Guid UserId { get; }

		public Guid CurrentUserId { get; }

		public LockUserCommand(Guid userId, Guid currentUserId)
		{
			UserId = userId;
			CurrentUserId = currentUserId;
		}
	}
}
