using FluentValidation;
using GO.Commands.Management;

namespace GO.Commands.Validators.Management
{
	public sealed class RegisterTelegramUserValidator
		: AbstractValidator<RegisterTelegramUserCommand>
	{
		public RegisterTelegramUserValidator()
		{
			RuleFor(item => item.CurrentUserId)
				.NotEmpty();

			RuleFor(item => item.TelegramId)
				.NotEmpty();

			RuleFor(item => item.NickName)
				.NotEmpty();
		}
	}
}
