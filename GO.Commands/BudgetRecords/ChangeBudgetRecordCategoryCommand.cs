using System;
using GO.Domain.Enums.Budgets;
using MediatR;

namespace GO.Commands.BudgetRecords
{
	public sealed record ChangeBudgetRecordCategoryCommand(
			Guid RecordId,
			Guid BudgetId,
			Guid CurrentUserId,
			CategoryType CategoryType)
		: IRequest<Unit>;
}
