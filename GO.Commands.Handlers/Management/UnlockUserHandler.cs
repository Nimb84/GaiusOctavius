using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Management;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Commands.Handlers.Management
{
	public sealed class UnlockUserHandler
		: IRequestHandler<UnlockUserCommand>
	{
		private readonly ApplicationDbContext _context;

		public UnlockUserHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
		{
			var userDictionary = await _context.Users
				.Where(user => !user.IsArchived
											 && (user.Id == request.UserId
													 || user.Id == request.CurrentUserId))
				.ToDictionaryAsync(user => user.Id, user => user, cancellationToken);

			var user = userDictionary[request.UserId];
			var currentUser = userDictionary[request.CurrentUserId];

			if (user == default || currentUser == default)
				throw new GoNotFoundException(nameof(User));

			if (!currentUser.HasAccessTo(Scopes.Management, Scopes.Administration))
				throw new GoForbiddenException();

			if (!user.IsLocked)
				return Unit.Value;

			user.IsLocked = false;
			user.UpdatedBy = request.CurrentUserId;
			user.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Users.Update(user);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
