using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Exceptions;
using GO.Queries.BudgetRecords;
using GO.Queries.ResponseModels.Budgets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Queries.Handlers.BudgetRecords
{
	public sealed class GetBudgetRecordHandler
		: IRequestHandler<GetBudgetRecordQuery, BudgetRecordResponse>
	{
		private readonly ApplicationDbContext _context;

		public GetBudgetRecordHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BudgetRecordResponse> Handle(
			GetBudgetRecordQuery request,
			CancellationToken cancellationToken)
		{
			var recordEntity = await _context.BudgetRecords
				.Select(record => new BudgetRecordResponse
				{
					Id = record.Id,
					CategoryType = record.CategoryType,
					Amount = record.Amount,
					CreatedBy = record.CreatedBy,
					CreatedDate = record.CreatedDate
				})
				.FirstOrDefaultAsync(
					record => record.Id == request.BudgetRecordId
										&& record.CreatedBy == request.UserId, cancellationToken);

			return recordEntity ?? throw new GoNotFoundException(nameof(BudgetRecord));
		}
	}
}
