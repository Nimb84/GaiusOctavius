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
			var budgetEntity = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => !relation.IsDisabled && relation.UserId == request.CurrentUserId))
						.ThenInclude(relation => relation.User)
				.Include(budget => budget.Records
					.Where(record => record.Id == request.RecordId))
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == request.BudgetId, cancellationToken);

			if (budgetEntity == default)
				throw new GoNotFoundException(nameof(Budget));

			if (!budgetEntity.Records.Any())
				throw new GoNotFoundException(nameof(Budget.Records));

			var currentUser = budgetEntity.Users.FirstOrDefault()?.User;
			if (currentUser == default || !currentUser.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			var recordEntity = budgetEntity.Records.First();

			recordEntity.CategoryType = request.CategoryType;
			recordEntity.UpdatedBy = request.CurrentUserId;
			recordEntity.UpdatedDate = DateTimeOffset.UtcNow;

			_context.BudgetRecords.Update(recordEntity);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
