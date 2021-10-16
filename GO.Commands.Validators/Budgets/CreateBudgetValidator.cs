using FluentValidation;
using GO.Commands.Budgets;

namespace GO.Commands.Validators.Budgets
{
	public sealed class CreateBudgetValidator
		: AbstractValidator<CreateBudgetCommand>
	{
		public CreateBudgetValidator()
		{
			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();
		}
	}
}
