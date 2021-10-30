using System.Threading.Tasks;
using Hangfire;

namespace GO.BackgroundJobs.Abstractions.Jobs.Budgets
{
	internal interface IMonthlyBudgetStatementJob
	{
		Task GenerateAsync(IJobCancellationToken cancellationToken);
	}
}
