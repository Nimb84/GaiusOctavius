using System;
using MediatR;

namespace GO.Commands.Budgets
{
	public sealed class ChangeBudgetPaydayCommand
		: IRequest<Unit>
	{
		public Guid BudgetId { get; set; }

		public Guid CurrentUserId { get; set; }

		public byte Payday { get; set; }
	}
}
