using System;
using MediatR;

namespace GO.Commands.BudgetRecords
{
	public sealed record DeleteBudgetRecordCommand(Guid RecordId, Guid BudgetId, Guid CurrentUserId)
		: IRequest<Unit>;
}
