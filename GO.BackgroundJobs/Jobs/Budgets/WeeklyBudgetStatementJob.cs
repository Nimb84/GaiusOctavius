using GO.BackgroundJobs.Abstractions.Jobs.Budgets;
using Hangfire;
using MediatR;
using System.Threading.Tasks;

namespace GO.BackgroundJobs.Jobs.Budgets
{
	internal sealed class WeeklyBudgetStatementJob
		: IWeeklyBudgetStatementJob
	{
		private readonly IMediator _mediator;

		public WeeklyBudgetStatementJob(IMediator mediator)
		{
			_mediator = mediator;
		}

		public Task GenerateAsync(IJobCancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
