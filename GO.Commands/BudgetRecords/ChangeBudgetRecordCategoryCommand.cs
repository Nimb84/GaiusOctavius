using System;
using GO.Domain.Enums.Budgets;
using MediatR;

namespace GO.Commands.BudgetRecords
{
	public sealed class ChangeBudgetRecordCategoryCommand
		: IRequest<Unit>
	{
		public Guid RecordId { get; set; }

		public Guid BudgetId { get; set; }

		public Guid CurrentUserId { get; set; }

		public CategoryType CategoryType { get; set; }
	}
}
