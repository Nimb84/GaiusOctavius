using FluentValidation;
using GO.Commands.Budgets;
using GO.Domain.Constants;

namespace GO.Commands.Validators.Budgets
{
	public sealed class ChangeBudgetPaydayValidator
		: AbstractValidator<ChangeBudgetPaydayCommand>
	{
		public ChangeBudgetPaydayValidator()
		{
			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();

			RuleFor(item => item.Payday)
				.Must(payday => payday
					is >= ValidationConstants.MinPayday
					and <= ValidationConstants.MaxPayday);
		}
	}
}
