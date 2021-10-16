using FluentValidation;
using GO.Commands.Budgets;

namespace GO.Commands.Validators.Budgets
{
	public sealed class DeleteBudgetValidator
		: AbstractValidator<DeleteBudgetCommand>
	{
		public DeleteBudgetValidator()
		{
			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();
		}
	}
}
