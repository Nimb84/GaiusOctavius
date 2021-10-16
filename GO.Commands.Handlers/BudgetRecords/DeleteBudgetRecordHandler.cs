using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.BudgetRecords;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Commands.Handlers.BudgetRecords
{
	public sealed class DeleteBudgetRecordHandler
		: IRequestHandler<DeleteBudgetRecordCommand, Unit>
	{
		private readonly ApplicationDbContext _context;

		public DeleteBudgetRecordHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			DeleteBudgetRecordCommand request,
			CancellationToken cancellationToken)
		{
			var budget = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => !relation.IsDisabled && relation.UserId == request.CurrentUserId))
						.ThenInclude(relation => relation.User)
				.Include(budget => budget.Records
					.Where(record => record.Id == request.RecordId))
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == request.BudgetId, cancellationToken);

			if (budget == default)
				throw new GoNotFoundException(nameof(Budget));

			if (!budget.Records.Any())
				return Unit.Value;

			var currentUser = budget.Users.FirstOrDefault()?.User;
			if (currentUser == default || !currentUser.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			_context.BudgetRecords.Remove(budget.Records.First());
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
