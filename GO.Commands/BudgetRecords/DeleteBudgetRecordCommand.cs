using System;
using MediatR;

namespace GO.Commands.BudgetRecords
{
	public sealed class DeleteBudgetRecordCommand
		: IRequest<Unit>
	{
		public Guid RecordId { get; set; }

		public Guid BudgetId { get; set; }

		public Guid CurrentUserId { get; set; }
	}
}
