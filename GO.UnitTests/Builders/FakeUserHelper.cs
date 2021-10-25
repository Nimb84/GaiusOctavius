using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Management;
using GO.Domain.Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace GO.UnitTests.Builders
{
	internal sealed class FakeUserHelper
	{
		public static User CreateUser(
			bool isLocked = default,
			bool isArchived = default) =>
			CreateDefault(Scopes.Budget, isLocked, isArchived);

		public static User CreateAdmin(
			bool isLocked = default,
			bool isArchived = default) =>
			CreateDefault(Scopes.Management | Scopes.Administration, isLocked, isArchived);

		public static User CreateDefault(
			Scopes scopes = Scopes.None,
			bool isLocked = default,
			bool isArchived = default,
			long connectionId = default,
			ConnectionType connectionType = default)
		{
			var userId = Guid.NewGuid();

			return new User
			{
				Id = userId,
				CreatedBy = Guid.NewGuid(),
				CreatedDate = DateTimeOffset.UtcNow,
				FirstName = "TestUserFirstName",
				LastName = "TestUserLastName",
				Scopes = scopes,
				Connections = CreateCommunicationList(userId, connectionId, connectionType),
				IsLocked = isLocked,
				IsArchived = isArchived
			};
		}

		public static UserConnection CreateCommunication(
			Guid userId,
			long connectionId,
			ConnectionType connectionType = default) =>
			new()
			{
				CreatedBy = userId,
				CreatedDate = DateTimeOffset.UtcNow,
				NickName = "@FakeUserNickname",
				ConnectionId = connectionId,
				Type = connectionType
			};

		public static List<UserConnection> CreateCommunicationList(
			Guid userId,
			long connectionId = -1,
			ConnectionType connectionType = default) =>
			connectionType == default
				? default
				: new List<UserConnection>
				{
					CreateCommunication(userId, connectionId, connectionType)
				};

		public static Task AddToDbAsync(
			ApplicationDbContextTest contextHandler,
			User fakeUser,
			CancellationToken cancellationToken) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				context.Users.Add(fakeUser);

				return context.SaveChangesAsync(cancellationToken);
			});

		public static Task<User> GetEntityAsync(
			ApplicationDbContextTest contextHandler,
			Guid userId,
			CancellationToken cancellationToken) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
				context.Users
					.Include(user => user.Connections)
					.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken));
	}
}
