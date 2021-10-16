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

			var entity = new BudgetRecord
			{
				Id = request.RecordId,
				BudgetId = request.BudgetId,
				CategoryType = request.CategoryType,
				Amount = request.Amount,
				Description = request.Description,
				CreatedBy = request.CurrentUserId,
				CreatedDate = DateTimeOffset.UtcNow,
			};

			budget.UpdatedBy = request.CurrentUserId;
			budget.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Budgets.Update(budget);
			await _context.BudgetRecords.AddAsync(entity, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
