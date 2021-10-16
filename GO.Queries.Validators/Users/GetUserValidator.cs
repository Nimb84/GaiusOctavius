using FluentValidation;
using GO.Queries.Users;

namespace GO.Queries.Validators.Users
{
	public sealed class GetUserValidator
		: AbstractValidator<GetUserQuery>
	{
		public GetUserValidator()
		{
			RuleFor(item => item.UserId)
				.NotEmpty();

			RuleFor(item => item.CurrentUserId)
				.NotEmpty()
				.Equal(item => item.UserId);

			RuleFor(item => item.ConnectionType)
				.NotEmpty();
		}
	}
}
