using System;
using GO.Domain.Enums.Management;

namespace GO.Queries.ResponseModels.Users
{
	public sealed record TelegramUserResponse
	{
		public Guid UserId { get; init; }

		public long ChatId { get; init; }

		public Scopes CurrentScope { get; init; }

		public Guid? BudgetId { get; init; }
	}
}
