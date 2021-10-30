using FluentValidation;
using GO.Queries.Budgets;

namespace GO.Queries.Validators.Budgets
{
	public sealed class GetShareBudgetTokenValidator
		: AbstractValidator<GetShareBudgetTokenQuery>
	{
		public GetShareBudgetTokenValidator()
		{
			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();
		}
	}
}
