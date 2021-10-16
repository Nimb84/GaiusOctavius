using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Users;
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
					.Where(connection => connection.Type == request.ConnectionType))
				.FirstOrDefaultAsync(user => !user.IsArchived
																		 && user.Id == request.UserId, cancellationToken);

			if (user == default)
				throw new GoNotFoundException(nameof(User));

			return new UserResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Scopes = user.Scopes,
				Nickname = user.Connections.FirstOrDefault()?.NickName,
				CurrentScope = user.Connections.FirstOrDefault()?.CurrentScope
			};
		}
	}
}
