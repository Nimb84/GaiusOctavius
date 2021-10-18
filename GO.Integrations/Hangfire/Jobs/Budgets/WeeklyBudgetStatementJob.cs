using System.Threading.Tasks;
using GO.Integrations.Hangfire.Abstractions.Jobs.Budgets;
using Hangfire;
using MediatR;

namespace GO.Integrations.Hangfire.Jobs.Budgets
{
	internal sealed class WeeklyBudgetStatementJob
		: IWeeklyBudgetStatementJob
	{
		private readonly IMediator _mediator;

		public WeeklyBudgetStatementJob(IMediator mediator)
		{
			_mediator = mediator;
		}

		public Task SendStatementAsync(IJobCancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
