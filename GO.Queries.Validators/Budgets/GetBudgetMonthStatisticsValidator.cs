using System;
using System.Linq;
using FluentValidation;
using GO.Domain.Constants;
using GO.Queries.Budgets;

namespace GO.Queries.Validators.Budgets
{
	public sealed class GetBudgetMonthStatisticsValidator
		: AbstractValidator<GetBudgetMonthStatisticsQuery>
	{
		public GetBudgetMonthStatisticsValidator()
		{
			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.UserId)
				.NotEmpty();

			RuleFor(item => item.UserIdList)
				.Must((item, ids) => ids.Contains(item.UserId))
				.When(item => item.UserIdList.Any());

			RuleFor(item => item.From)
				.NotEmpty();

			RuleFor(item => item.To)
				.NotEmpty()
				.LessThanOrEqualTo(DateTimeOffset.UtcNow);

			RuleFor(item => item.Skip)
				.NotEmpty();

			RuleFor(item => item.Take)
				.NotEmpty()
				.LessThanOrEqualTo(ValidationConstants.MaxTake);
		}
	}
}
