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
			var budget = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => !relation.IsDisabled && relation.UserId == request.CurrentUserId))
						.ThenInclude(relation => relation.User)
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == request.BudgetId, cancellationToken);

			if (budget == default)
				throw new GoNotFoundException(nameof(Budget));

			var currentUser = budget.Users.FirstOrDefault()?.User;
			if (currentUser == default || !currentUser.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			budget.IsArchived = true;
			budget.UpdatedBy = request.CurrentUserId;
			budget.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Budgets.Update(budget);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
