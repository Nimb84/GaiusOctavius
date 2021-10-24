using FluentValidation;
using GO.Queries.BudgetRecords;

namespace GO.Queries.Validators.BudgetRecords
{
	public sealed class GetBudgetRecordValidator
		: AbstractValidator<GetBudgetRecordQuery>
	{
		public GetBudgetRecordValidator()
		{
			RuleFor(item => item.BudgetRecordId)
				.NotEmpty();

			RuleFor(item => item.UserId)
				.NotEmpty();
		}
	}
}
