using System;
using GO.Queries.ResponseModels.Budgets;
using MediatR;

namespace GO.Queries.BudgetRecords
{
	public sealed record GetBudgetRecordQuery(Guid BudgetRecordId, Guid UserId)
		: IRequest<BudgetRecordResponse>;
}
