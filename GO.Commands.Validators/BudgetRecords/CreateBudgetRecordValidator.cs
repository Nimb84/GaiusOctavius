using FluentValidation;
using GO.Commands.BudgetRecords;
using GO.Domain.Constants;

namespace GO.Commands.Validators.BudgetRecords
{
	public sealed class CreateBudgetRecordValidator
		: AbstractValidator<CreateBudgetRecordCommand>
	{
		public CreateBudgetRecordValidator()
		{
			RuleFor(item => item.RecordId)
				.NotEmpty();

			RuleFor(item => item.BudgetId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty();

			RuleFor(item => item.Amount)
				.NotEmpty();
		}
	}
}
