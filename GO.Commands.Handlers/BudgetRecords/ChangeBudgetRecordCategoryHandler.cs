using System;
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
	public sealed class ChangeBudgetRecordCategoryHandler
		: IRequestHandler<ChangeBudgetRecordCategoryCommand, Unit>
	{
		private readonly ApplicationDbContext _context;

		public ChangeBudgetRecordCategoryHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			ChangeBudgetRecordCategoryCommand request,
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
				throw new GoNotFoundException(nameof(Budget.Records));

			var currentUser = budget.Users.FirstOrDefault()?.User;
			if (currentUser == default || !currentUser.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			var record = budget.Records.First();

			record.CategoryType = request.CategoryType;
			record.UpdatedBy = request.CurrentUserId;
			record.UpdatedDate = DateTimeOffset.UtcNow;

			_context.BudgetRecords.Update(record);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
