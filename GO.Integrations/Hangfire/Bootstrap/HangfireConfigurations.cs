using System;
using GO.Integrations.Hangfire.Abstractions;
using GO.Integrations.Hangfire.Abstractions.Jobs.Budgets;
using GO.Integrations.Hangfire.Jobs.Budgets;
using GO.Integrations.Hangfire.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GO.Integrations.Hangfire.Bootstrap
{
	public static class HangfireConfigurations
	{
		private const string CronMonthlyFirstDay = "0 12 * * 1";
		private const string CronWeeklyFirstDay = "0 12 1 * *";

		public static void RegisterHangfire(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddTransient<IScheduleJobService, ScheduleJobService>();

			services.AddJobs();

			services.AddHangfire(settings => settings
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(
					configuration.GetConnectionString("GaiusBotDb"),
					new SqlServerStorageOptions
					{
						CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
						SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
						QueuePollInterval = TimeSpan.Zero,
						UseRecommendedIsolationLevel = true,
						DisableGlobalLocks = true
					}));

			services.AddHangfireServer();
		}

		public static void UseHangfire(this IApplicationBuilder app)
		{
			app.UseHangfireDashboard();

			var recurringJobManager = app.ApplicationServices
				.GetRequiredService<IRecurringJobManager>();

			recurringJobManager.RunScheduledJobs();
		}

		private static void RunScheduledJobs(this IRecurringJobManager recurringJobManager)
		{
			recurringJobManager.AddOrUpdate<IMonthlyBudgetStatementJob>(
				nameof(IMonthlyBudgetStatementJob.SendStatementAsync),
				job => job.SendStatementAsync(JobCancellationToken.Null),
				CronWeeklyFirstDay,
				TimeZoneInfo.Utc);

			recurringJobManager.AddOrUpdate<IMonthlyBudgetStatementJob>(
				nameof(IMonthlyBudgetStatementJob.SendStatementAsync),
				job => job.SendStatementAsync(JobCancellationToken.Null),
				CronMonthlyFirstDay,
				TimeZoneInfo.Utc);
		}

		private static void AddJobs(this IServiceCollection services)
		{
			services.AddTransient<IMonthlyBudgetStatementJob, MonthlyBudgetStatementJob>();
			services.AddTransient<IWeeklyBudgetStatementJob, WeeklyBudgetStatementJob>();
		}
	}
}
