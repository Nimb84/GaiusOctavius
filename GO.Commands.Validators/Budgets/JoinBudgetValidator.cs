using FluentValidation;
using GO.Commands.Budgets;
using GO.Domain.Helpers;

namespace GO.Commands.Validators.Budgets
{
	public sealed class JoinBudgetValidator
		: AbstractValidator<JoinBudgetCommand>
	{
		public JoinBudgetValidator()
		{
			RuleFor(item => item.Token)
				.NotEmpty()
				.Must(SecurityTokenHelper.Validate);

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();
		}
	}
}
