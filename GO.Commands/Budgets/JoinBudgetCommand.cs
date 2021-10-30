using MediatR;
using System;

namespace GO.Commands.Budgets
{
	public sealed record JoinBudgetCommand(string Token, Guid CurrentUserId)
		: IRequest<Unit>;
}
