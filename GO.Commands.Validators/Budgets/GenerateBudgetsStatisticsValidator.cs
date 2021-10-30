using FluentValidation;
using GO.Commands.Budgets;

namespace GO.Commands.Validators.Budgets
{
	public sealed class GenerateBudgetsStatisticsValidator
		: AbstractValidator<GenerateBudgetsStatisticsCommand>
	{
		public GenerateBudgetsStatisticsValidator()
		{
			RuleFor(item => item.DateFrom)
				.NotEmpty()
				.LessThanOrEqualTo(item => item.DateTo);

			RuleFor(item => item.DateTo)
				.NotEmpty()
				.GreaterThanOrEqualTo(item => item.DateFrom);
		}
	}
}
