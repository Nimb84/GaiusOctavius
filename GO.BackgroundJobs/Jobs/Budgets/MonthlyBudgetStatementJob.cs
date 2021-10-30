using GO.BackgroundJobs.Abstractions.Jobs.Budgets;
using Hangfire;
using System.Threading.Tasks;

namespace GO.BackgroundJobs.Jobs.Budgets
{
	internal sealed class MonthlyBudgetStatementJob
		: IMonthlyBudgetStatementJob
	{
		public Task GenerateAsync(IJobCancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}
