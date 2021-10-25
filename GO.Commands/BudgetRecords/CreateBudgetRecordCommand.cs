using System;
using GO.Domain.Enums.Budgets;
using MediatR;

namespace GO.Commands.BudgetRecords
{
	public sealed record CreateBudgetRecordCommand(
			Guid RecordId,
			Guid BudgetId,
			uint Amount,
			CategoryType CategoryType,
			Guid CurrentUserId)
		: IRequest<Unit>;
}
