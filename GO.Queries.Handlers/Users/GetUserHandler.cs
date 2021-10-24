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
			var user = await _context.Users
				.Include(user => user.Connections
					.Where(connection => request.ConnectionType == ConnectionType.Unsupported
															 || connection.Type == request.ConnectionType))
				.FirstOrDefaultAsync(
					user => !user.IsArchived
									&& user.Id == request.UserId, cancellationToken);

			if (user == default
					|| request.ConnectionType != default
					&& user.Connections.All(connection => connection.ConnectionId == default))
				throw new GoNotFoundException(nameof(User));

			if(user.IsLocked)
				throw new GoForbiddenException();

			return new UserResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Scopes = user.Scopes,
				Nickname = user.Connections.FirstOrDefault()?.NickName,
				ConnectionId = user.Connections.FirstOrDefault()?.ConnectionId ?? default,
				CurrentScope = user.Connections.FirstOrDefault()?.CurrentScope
			};
		}
	}
}
