using GO.Domain.Constants;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GO.DataAccess.MsSql.Configurations.Users
{
	internal sealed class UserConnectionConfiguration
		: IEntityTypeConfiguration<UserConnection>
	{
		public void Configure(EntityTypeBuilder<UserConnection> builder)
		{
			builder.HasKey(connection => connection.Id);

			builder.HasIndex(connection => connection.ConnectionId)
				.IsUnique();

			builder.Property(connection => connection.NickName)
				.HasMaxLength(ValidationConstants.StringMaxLength);

			builder.Property(connection => connection.ConnectionId)
				.HasMaxLength(ValidationConstants.StringMaxLength)
				.IsRequired();
		}
	}
}
