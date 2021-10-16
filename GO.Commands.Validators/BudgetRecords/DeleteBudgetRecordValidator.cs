using FluentValidation;
using GO.Commands.BudgetRecords;

namespace GO.Commands.Validators.BudgetRecords
{
	public sealed class DeleteBudgetRecordValidator
		: AbstractValidator<DeleteBudgetRecordCommand>
	{
		public DeleteBudgetRecordValidator()
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
