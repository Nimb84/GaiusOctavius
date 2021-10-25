using System;
using GO.Domain.Models;
using GO.Queries.ResponseModels.Budgets;
using MediatR;

namespace GO.Queries.Budgets
{
	public sealed record GetBudgetMonthStatisticsQuery
		: PagedQueryFilter
		, IRequest<BudgetResponse>
	{
		public Guid BudgetId { get; init; }

		public Guid UserId { get; init; }
	}
}
