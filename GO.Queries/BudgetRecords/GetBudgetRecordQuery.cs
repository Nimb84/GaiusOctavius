using System;
using GO.Queries.ResponseModels.Budgets;
using MediatR;

namespace GO.Queries.BudgetRecords
{
	public sealed class GetBudgetRecordQuery
		: IRequest<BudgetRecordResponse>
	{
		public Guid BudgetRecordId { get; set; }

		public Guid UserId { get; set; }
	}
}
