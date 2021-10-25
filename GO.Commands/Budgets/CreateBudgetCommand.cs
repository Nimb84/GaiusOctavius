using System;
using MediatR;

namespace GO.Commands.Budgets
{
	public sealed record CreateBudgetCommand(Guid BudgetId, Guid CurrentUserId)
		: IRequest<Unit>;
}
