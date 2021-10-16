using FluentValidation;
using GO.Commands.BudgetRecords;

namespace GO.Commands.Validators.BudgetRecords
{
	public sealed class ChangeBudgetRecordCategoryValidator
		: AbstractValidator<ChangeBudgetRecordCategoryCommand>
	{
		public ChangeBudgetRecordCategoryValidator()
		{
			RuleFor(item => item.RecordId)
				.NotEmpty();

			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();
		}
	}
}
