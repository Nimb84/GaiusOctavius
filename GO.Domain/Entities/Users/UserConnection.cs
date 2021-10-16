using System;
using System.ComponentModel.DataAnnotations.Schema;
using GO.Domain.Enums.Management;
using GO.Domain.Enums.Users;

namespace GO.Domain.Entities.Users
{
	[Table(nameof(UserConnection))]
	public sealed class UserConnection
		: BaseEntity
	{
		public string NickName { get; set; }

		public long ConnectionId { get; set; }

		public ConnectionType Type { get; set; }

		public Scopes CurrentScope { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }
	}
}
