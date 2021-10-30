using System.Threading.Tasks;
using Hangfire;

namespace GO.BackgroundJobs.Abstractions.Jobs.Budgets
{
	internal interface IWeeklyBudgetStatementJob
	{
		Task GenerateAsync(IJobCancellationToken cancellationToken);
	}
}
