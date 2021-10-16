using System.Linq;
using GO.Domain.Entities.Users;
using GO.Domain.Enums.Management;

namespace GO.Domain.Extensions
{
	public static class UserExtensions
	{
		public static string FullName(this User user) =>
			$"{user.FirstName} {user.LastName}";

		//public static bool HasScope(this User user, Scopes scope) =>
		//	user.Scopes.HasFlag(scope);

		public static bool HasAccessTo(this User user, params Scopes[] scopes) =>
			!user.IsArchived && !user.IsLocked && scopes.All(scope => user.Scopes.HasFlag(scope));
	}
}
