using System;
using GO.Domain.Enums.Users;
using GO.Queries.ResponseModels.Users;
using MediatR;

namespace GO.Queries.Users
{
	public sealed class GetUserQuery
		: IRequest<UserResponse>
	{
		public Guid UserId { get; set; }

		public Guid CurrentUserId { get; set; }

		public ConnectionType ConnectionType { get; set; }
	}
}
