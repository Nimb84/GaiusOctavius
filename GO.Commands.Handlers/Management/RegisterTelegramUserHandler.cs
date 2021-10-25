using System;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Management;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Domain;
using GO.Domain.Enums.Management;
using GO.Domain.Enums.Users;
using GO.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GO.Commands.Handlers.Management
{
	public sealed class RegisterTelegramUserHandler
		: IRequestHandler<RegisterTelegramUserCommand>
	{
		private readonly ApplicationDbContext _context;

		public RegisterTelegramUserHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(RegisterTelegramUserCommand request, CancellationToken cancellationToken)
		{
			var collision = await _context.UserConnections
				.AnyAsync(connection => connection.Type == ConnectionType.Telegram
																&& connection.ConnectionId == request.TelegramId, cancellationToken);

			if (collision)
				throw new GoException(StatusCodes.Status409Conflict, ExceptionType.Conflict);

			var user = new User
			{
				Id = request.UserId,
				CreatedBy = request.UserId,
				CreatedDate = DateTimeOffset.UtcNow,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Scopes = Scopes.Budget,
				Connections = new()
				{
					new()
					{
						Id = Guid.NewGuid(),
						CreatedBy = request.UserId,
						CreatedDate = DateTimeOffset.UtcNow,
						NickName = request.NickName,
						ConnectionId = request.TelegramId,
						Type = ConnectionType.Telegram,
						UserId = request.UserId,
						CurrentScope = Scopes.Budget
					}
				}
			};

			await _context.Users.AddAsync(user, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
