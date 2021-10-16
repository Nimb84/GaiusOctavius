using System;
using GO.Domain.Models;
using GO.Queries.ResponseModels.Budgets;
using MediatR;

namespace GO.Queries.Budgets
{
	public sealed class GetBudgetMonthStatisticsQuery
		: PagedQueryFilter
		, IRequest<BudgetResponse>
	{
		public Guid BudgetId { get; set; }

		public Guid UserId { get; set; }
	}
}
