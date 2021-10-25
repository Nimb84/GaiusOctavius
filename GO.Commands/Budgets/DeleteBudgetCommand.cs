using System;
using MediatR;

namespace GO.Commands.Budgets
{
	public sealed record DeleteBudgetCommand(Guid BudgetId, Guid CurrentUserId)
		: IRequest<Unit>;
}
