using FluentValidation;
using GO.Queries.Users;

namespace GO.Queries.Validators.Users
{
	public sealed class GetTelegramUserValidator
		: AbstractValidator<GetTelegramUserQuery>
	{
		public GetTelegramUserValidator()
		{
			RuleFor(item => item.TelegramUserId)
				.NotEmpty();
		}
	}
}
