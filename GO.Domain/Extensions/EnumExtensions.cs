using System;
using System.Linq;

namespace GO.Domain.Extensions
{
	public static class EnumExtensions
	{
		public static TEnum Parse<TEnum>(string value)
			where TEnum : Enum =>
			string.IsNullOrWhiteSpace(value) 
				? default
				: Enum.GetValues(typeof(TEnum))
						.Cast<TEnum>()
						.FirstOrDefault(type => type.ToString().ToLower().StartsWith(value));
	}
}
