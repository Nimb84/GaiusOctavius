using FluentValidation;
using GO.Commands.Management;

namespace GO.Commands.Validators.Management
{
	public sealed class LockUserValidator
		: AbstractValidator<LockUserCommand>
	{
		public LockUserValidator()
		{
			RuleFor(item => item.UserId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty()
				.NotEqual(item => item.UserId);
		}
	}
}
