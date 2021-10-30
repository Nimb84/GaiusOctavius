using System;
using System.Collections.Generic;
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

			var targetUser = userDictionary.GetValueOrDefault(request.UserId);
			var currentUser = userDictionary.GetValueOrDefault(request.CurrentUserId);

			if (targetUser == default)
				throw new GoNotFoundException(nameof(User));

			if (currentUser == default
					|| !currentUser.HasAccessTo(Scopes.Management, Scopes.Administration))
				throw new GoForbiddenException();

			if (!targetUser.IsLocked)
				return Unit.Value;

			targetUser.IsLocked = false;
			targetUser.UpdatedBy = request.CurrentUserId;
			targetUser.UpdatedDate = DateTimeOffset.UtcNow;

			_context.Users.Update(targetUser);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
