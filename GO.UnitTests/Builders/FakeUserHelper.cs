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
		public static async Task<Guid> AddCorrectAsync(
			ApplicationDbContextTest contextHandler,
			long connectionId,
			ConnectionType connectionType,
			CancellationToken cancellationToken)
		{
			var userId = Guid.NewGuid();

			var fakeCommunications = CreateCommunicationList(userId, connectionId, connectionType);
			var fakeUser = Create(userId, fakeCommunications);

			await AddToDbAsync(contextHandler, fakeUser, cancellationToken);

			return fakeUser.Id;
		}

		public static async Task<Guid> AddLockedAsync(
			ApplicationDbContextTest contextHandler,
			long connectionId,
			ConnectionType connectionType,
			CancellationToken cancellationToken)
		{
			var userId = Guid.NewGuid();

			var fakeCommunications = CreateCommunicationList(userId, connectionId, connectionType);
			var fakeUser = Create(userId, fakeCommunications);

			fakeUser.IsLocked = true;

			await AddToDbAsync(contextHandler, fakeUser, cancellationToken);

			return fakeUser.Id;
		}

		public static async Task<Guid> AddArchivedAsync(
			ApplicationDbContextTest contextHandler,
			long connectionId,
			ConnectionType connectionType,
			CancellationToken cancellationToken)
		{
			var userId = Guid.NewGuid();

			var fakeCommunications = CreateCommunicationList(userId, connectionId, connectionType);
			var fakeUser = Create(userId, fakeCommunications);

			fakeUser.IsArchived = true;

			await AddToDbAsync(contextHandler, fakeUser, cancellationToken);

			return fakeUser.Id;
		}

		public static async Task<Guid> AddNoRightsAsync(
			ApplicationDbContextTest contextHandler,
			long connectionId,
			ConnectionType connectionType,
			CancellationToken cancellationToken)
		{
			var userId = Guid.NewGuid();

			var fakeCommunications = CreateCommunicationList(userId, connectionId, connectionType);
			var fakeUser = Create(userId, fakeCommunications);

			fakeUser.Scopes = default;

			await AddToDbAsync(contextHandler, fakeUser, cancellationToken);

			return fakeUser.Id;
		}

		public static Task AddToDbAsync(
			ApplicationDbContextTest contextHandler,
			User fakeUser,
			CancellationToken cancellationToken) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
			{
				context.Users.Add(fakeUser);
				return context.SaveChangesAsync(cancellationToken);
			});

		public static User Create(
			Guid id,
			List<UserConnection> communications = default) =>
			new()
			{
				Id = id,
				CreatedBy = Guid.NewGuid(),
				CreatedDate = DateTimeOffset.UtcNow,
				FirstName = "TestUserFirstName",
				LastName = "TestUserLastName",
				Scopes = Scopes.Budget,
				Connections = communications,
				IsLocked = false,
				IsArchived = false
			};

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

		public static Task<User> GetEntityAsync(
			ApplicationDbContextTest contextHandler,
			Guid userId,
			CancellationToken cancellationToken) =>
			contextHandler.ExecuteWithTestContextAsync(context =>
				context.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken));
	}
}
