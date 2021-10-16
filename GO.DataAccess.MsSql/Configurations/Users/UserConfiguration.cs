using GO.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GO.DataAccess.MsSql.Configurations.Users
{
	internal sealed class UserConfiguration
		: IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(user => user.Id);

			builder.Property(user => user.FirstName)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(user => user.LastName)
				.HasMaxLength(50)
				.IsRequired();

			builder.HasMany(user => user.Connections)
				.WithOne(service => service.User)
				.HasForeignKey(service => service.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
