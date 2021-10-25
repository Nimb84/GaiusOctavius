using System;
using GO.Domain.Enums.Users;
using GO.Queries.ResponseModels.Users;
using MediatR;

namespace GO.Queries.Users
{
	public sealed record GetUserQuery(Guid UserId, Guid CurrentUserId, ConnectionType ConnectionType = default)
		: IRequest<UserResponse>;
}
