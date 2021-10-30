using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Budgets;
using GO.DataAccess.MsSql;
using MediatR;

namespace GO.Commands.Handlers.Budgets
{
	public sealed class GenerateBudgetsStatisticsHandler
		: IRequestHandler<GenerateBudgetsStatisticsCommand>
	{
		private readonly ApplicationDbContext _context;

		public GenerateBudgetsStatisticsHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			GenerateBudgetsStatisticsCommand request,
			CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
