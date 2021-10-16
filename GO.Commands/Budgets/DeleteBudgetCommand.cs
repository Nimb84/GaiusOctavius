using System;
using MediatR;

namespace GO.Commands.Budgets
{
	public sealed class DeleteBudgetCommand
		: IRequest<Unit>
	{
		public Guid BudgetId { get; set; }

		public Guid CurrentUserId { get; set; }
	}
}
