using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Budgets;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Commands.Handlers.Budgets
{
	public sealed class CreateBudgetHandler
		: IRequestHandler<CreateBudgetCommand>
	{
		private readonly ApplicationDbContext _context;

		public CreateBudgetHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			CreateBudgetCommand request,
			CancellationToken cancellationToken)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(user => user.Id == request.CurrentUserId, cancellationToken);

			if (user == default || !user.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			var entity = new BudgetsUsersRelation
			{
				UserId = request.CurrentUserId,
				Budget = new Budget
				{
					Id = request.BudgetId,
					CreatedBy = request.CurrentUserId,
					CreatedDate = DateTimeOffset.UtcNow
				}
			};

			await _context.BudgetsUsers.AddAsync(entity, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
