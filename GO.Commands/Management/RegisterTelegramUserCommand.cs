using System;
using MediatR;

namespace GO.Commands.Management
{
	public sealed class RegisterTelegramUserCommand
		: IRequest<Unit>
	{
		public Guid CurrentUserId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string NickName { get; set; }

		public long TelegramId { get; set; }
	}
}
