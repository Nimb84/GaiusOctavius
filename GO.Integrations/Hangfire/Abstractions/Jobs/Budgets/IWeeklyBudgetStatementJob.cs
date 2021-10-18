using System.Threading.Tasks;
using Hangfire;

namespace GO.Integrations.Hangfire.Abstractions.Jobs.Budgets
{
	internal interface IWeeklyBudgetStatementJob
	{
		Task SendStatementAsync(IJobCancellationToken cancellationToken);
	}
}
