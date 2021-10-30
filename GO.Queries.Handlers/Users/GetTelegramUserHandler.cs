using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using GO.Queries.ResponseModels.Users;
using GO.Queries.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Queries.Handlers.Users
{
	public sealed class GetTelegramUserHandler
			: IRequestHandler<GetTelegramUserQuery, TelegramUserResponse>
	{
		private readonly ApplicationDbContext _context;

		public GetTelegramUserHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<TelegramUserResponse> Handle(
			GetTelegramUserQuery request,
			CancellationToken cancellationToken)
		{
			var connectionEntity = await _context.UserConnections
				.Include(connection => connection.User)
					.ThenInclude(user => user.Budgets.Where(budget => !budget.IsDisabled))
				.AsSplitQuery()
				.FirstOrDefaultAsync(connection => connection.Type == ConnectionType.Telegram
																					 && connection.ConnectionId == request.TelegramUserId
																					 && !connection.User.IsArchived, cancellationToken);

			if (connectionEntity == default)
				throw new GoNotFoundException(nameof(User));

			if (connectionEntity.User.IsLocked)
				throw new GoForbiddenException();

			return new TelegramUserResponse
			{
				UserId = connectionEntity.UserId,
				ChatId = connectionEntity.ConnectionId,
				CurrentScope = connectionEntity.CurrentScope,
				BudgetId = connectionEntity.User.Budgets.FirstOrDefault()?.BudgetId
			};
		}
	}
}
