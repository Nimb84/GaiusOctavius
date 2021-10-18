using System.Threading.Tasks;
using GO.Integrations.Hangfire.Abstractions.Jobs.Budgets;
using Hangfire;

namespace GO.Integrations.Hangfire.Jobs.Budgets
{
	internal sealed class MonthlyBudgetStatementJob
		: IMonthlyBudgetStatementJob
	{
		public Task SendStatementAsync(IJobCancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
