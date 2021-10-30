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
	public sealed class CreateBudgetRecordHandler
		: IRequestHandler<CreateBudgetRecordCommand, Unit>
	{
		private readonly ApplicationDbContext _context;

		public CreateBudgetRecordHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(CreateBudgetRecordCommand request, CancellationToken cancellationToken)
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

			var entity = new BudgetRecord
			{
				Id = request.RecordId,
				BudgetId = request.BudgetId,
				CategoryType = request.CategoryType,
				Amount = request.Amount,
				CreatedBy = request.CurrentUserId,
				CreatedDate = DateTimeOffset.UtcNow,
			};

			budgetEntity.UpdatedBy = request.CurrentUserId;
			budgetEntity.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Budgets.Update(budgetEntity);
			await _context.BudgetRecords.AddAsync(entity, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
