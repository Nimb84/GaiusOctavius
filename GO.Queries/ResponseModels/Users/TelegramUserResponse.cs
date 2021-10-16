using System;
using GO.Domain.Enums.Management;

namespace GO.Queries.ResponseModels.Users
{
	public sealed class TelegramUserResponse
	{
		public Guid UserId { get; set; }

		public long ChatId { get; set; }

		public Scopes CurrentScope { get; set; }

		public Guid? ServiceId { get; set; }
	}
}
