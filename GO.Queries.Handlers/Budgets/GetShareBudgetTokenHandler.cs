using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Domain.Helpers;
using GO.Domain.Models;
using GO.Queries.Budgets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Queries.Handlers.Budgets
{
	public sealed class GetShareBudgetTokenHandler
		: IRequestHandler<GetShareBudgetTokenQuery, string>
	{
		private readonly ApplicationDbContext _context;

		public GetShareBudgetTokenHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<string> Handle(
			GetShareBudgetTokenQuery request,
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

			return SecurityTokenHelper.Generate(
				new SecurityToken(
					budgetEntity.Id,
					request.CurrentUserId,
					budgetEntity.UpdatedDate.GetValueOrDefault(budgetEntity.CreatedDate).UtcDateTime));
		}
	}
}
