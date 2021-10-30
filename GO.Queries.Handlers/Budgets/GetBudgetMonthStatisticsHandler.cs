using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Queries.Budgets;
using GO.Queries.ResponseModels.Budgets;
using GO.Queries.ResponseModels.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Queries.Handlers.Budgets
{
	public sealed class GetBudgetMonthStatisticsHandler
		: IRequestHandler<GetBudgetMonthStatisticsQuery, BudgetResponse>
	{
		private readonly ApplicationDbContext _context;

		public GetBudgetMonthStatisticsHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BudgetResponse> Handle(
			GetBudgetMonthStatisticsQuery request,
			CancellationToken cancellationToken)
		{
			var userEntity = await _context.Users
				.FirstOrDefaultAsync(user => !user.IsArchived
																		 && !user.IsLocked
																		 && user.Id == request.UserId, cancellationToken);

			if (userEntity == default || userEntity.HasAccessTo(Scopes.Budget))
				throw new GoForbiddenException();

			var budgetEntity = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => request.UserIdList.Contains(relation.UserId)
														 || relation.UserId == request.UserId))
				.ThenInclude(relation => relation.User)
				.Include(budget => budget.Records
					.Where(record => (request.UserIdList.Contains(record.CreatedBy)
														|| record.CreatedBy == request.UserId)
														&& record.CreatedDate < request.To
														&& record.CreatedDate >= request.From)
					.OrderByDescending(record => record.CreatedDate)
					.Skip(request.Skip)
					.Take(request.Take))
				.Select(budget => new BudgetResponse
				{
					Id = budget.Id,
					IsArchived = budget.IsArchived,
					CreatedById = budget.CreatedBy,
					Users = budget.Users
						.Select(user => new UserShortResponse
						{
							Id = user.UserId,
							FirstName = user.User.FirstName,
							LastName = user.User.LastName
						})
						.ToList(),
					Records = budget.Records
						.Select(record => new BudgetRecordResponse
						{
							Id = record.Id,
							CategoryType = record.CategoryType,
							Amount = record.Amount,
							CreatedBy = record.CreatedBy,
							CreatedDate = record.CreatedDate
						})
						.ToList()
				})
				.AsSplitQuery()
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == request.BudgetId, cancellationToken);

			if (budgetEntity == default)
				throw new GoNotFoundException(nameof(Budget));

			return budgetEntity;
		}
	}
}
