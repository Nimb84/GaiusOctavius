using MediatR;
using System;

namespace GO.Queries.Budgets
{
	public sealed record GetShareBudgetTokenQuery
		: IRequest<string>
	{
		public Guid BudgetId { get; init; }

		public Guid CurrentUserId { get; init; }
	}
}
