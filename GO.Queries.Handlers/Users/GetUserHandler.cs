using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using GO.Queries.ResponseModels.Users;
using GO.Queries.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Queries.Handlers.Users
{
	public sealed class GetUserHandler
		: IRequestHandler<GetUserQuery, UserResponse>
	{
		private readonly ApplicationDbContext _context;

		public GetUserHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
		{
			var userEntity = await _context.Users
				.Include(user => user.Connections
					.Where(connection => request.ConnectionType == ConnectionType.Unsupported
															 || connection.Type == request.ConnectionType))
				.FirstOrDefaultAsync(
					user => !user.IsArchived
									&& user.Id == request.UserId, cancellationToken);

			if (userEntity == default
					|| request.ConnectionType != default
					&& userEntity.Connections.All(connection => connection.ConnectionId == default))
				throw new GoNotFoundException(nameof(User));

			if (userEntity.IsLocked)
				throw new GoForbiddenException();

			return new UserResponse
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Scopes = userEntity.Scopes,
				Nickname = userEntity.Connections.FirstOrDefault()?.NickName,
				ConnectionId = userEntity.Connections.FirstOrDefault()?.ConnectionId ?? default,
				CurrentScope = userEntity.Connections.FirstOrDefault()?.CurrentScope
			};
		}
	}
}
