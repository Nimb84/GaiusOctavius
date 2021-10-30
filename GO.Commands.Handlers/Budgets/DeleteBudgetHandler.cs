using System;
using System.Linq;
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
	public sealed class DeleteBudgetHandler
		: IRequestHandler<DeleteBudgetCommand>
	{
		private readonly ApplicationDbContext _context;

		public DeleteBudgetHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			DeleteBudgetCommand request,
			CancellationToken cancellationToken)
		{
			var budgetEntity = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => !relation.IsDisabled && relation.UserId == request.CurrentUserId))
						.ThenInclude(relation => relation.User)
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == request.BudgetId, cancellationToken);

			if (budgetEntity == default)
				throw new GoNotFoundException(nameof(Budget));

			var currentUser = budgetEntity.Users.FirstOrDefault()?.User;
			if (currentUser == default || !currentUser.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			budgetEntity.IsArchived = true;
			budgetEntity.UpdatedBy = request.CurrentUserId;
			budgetEntity.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Budgets.Update(budgetEntity);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
