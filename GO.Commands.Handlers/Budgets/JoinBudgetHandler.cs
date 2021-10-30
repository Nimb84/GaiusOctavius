using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GO.Commands.Budgets;
using GO.DataAccess.MsSql;
using GO.Domain.Entities.Budgets;
using GO.Domain.Enums.Management;
using GO.Domain.Exceptions;
using GO.Domain.Extensions;
using GO.Domain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GO.Commands.Handlers.Budgets
{
	public sealed class JoinBudgetHandler
		: IRequestHandler<JoinBudgetCommand>
	{
		private readonly ApplicationDbContext _context;

		public JoinBudgetHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(
			JoinBudgetCommand request,
			CancellationToken cancellationToken)
		{
			if (!SecurityTokenHelper.TryParse(request.Token, out var token))
				throw new GoForbiddenException();

			var targetBudgetEntity = await _context.Budgets
				.Include(budget => budget.Users
					.Where(relation => !relation.IsDisabled && relation.UserId == token.Issuer))
				.ThenInclude(relation => relation.User)
				.FirstOrDefaultAsync(budget => !budget.IsArchived
																			 && budget.Id == token.Key, cancellationToken);

			if (targetBudgetEntity == default)
				throw new GoNotFoundException(nameof(Budget));

			var validTokenTimeStamp = targetBudgetEntity.UpdatedDate
				.GetValueOrDefault(targetBudgetEntity.CreatedDate)
				.UtcDateTime;

			var invitingUser = targetBudgetEntity.Users.FirstOrDefault()?.User;
			if (invitingUser == default
					|| !invitingUser.HasAccessTo(Scopes.Budget)
					|| token.TimeStamp != validTokenTimeStamp)
				throw new GoForbiddenException();

			await ArchiveOwnedBudgetsAsync(request.CurrentUserId, cancellationToken);
			await DisableFollowedBudgetsAsync(request.CurrentUserId, cancellationToken);

			targetBudgetEntity.UpdatedDate = DateTimeOffset.UtcNow;
			targetBudgetEntity.UpdatedBy = request.CurrentUserId;
			targetBudgetEntity.Users.Add(new BudgetsUsersRelation
			{
				UserId = request.CurrentUserId,
				BudgetId = targetBudgetEntity.Id
			});

			_context.Budgets.Update(targetBudgetEntity);

			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private async Task ArchiveOwnedBudgetsAsync(Guid userId, CancellationToken cancellationToken)
		{
			var ownedBudgetEntities = await _context.Budgets
				.Where(budget => !budget.IsArchived
												 && budget.CreatedBy == userId)
				.ToListAsync(cancellationToken);

			if (ownedBudgetEntities.Any())
			{
				ownedBudgetEntities.ForEach(budget =>
				{
					budget.IsArchived = true;
					budget.UpdatedBy = userId;
					budget.UpdatedDate = DateTimeOffset.UtcNow;
				});

				_context.Budgets.UpdateRange(ownedBudgetEntities);
			}
		}

		private async Task DisableFollowedBudgetsAsync(Guid userId, CancellationToken cancellationToken)
		{
			var followedBudgetEntities = await _context.BudgetsUsers
				.Where(connection => !connection.IsDisabled
														 && connection.UserId == userId)
				.ToListAsync(cancellationToken);

			if (followedBudgetEntities.Any())
			{
				followedBudgetEntities.ForEach(connection =>
				{
					connection.IsDisabled = true;
				});

				_context.BudgetsUsers.UpdateRange(followedBudgetEntities);
			}
		}
	}
}
