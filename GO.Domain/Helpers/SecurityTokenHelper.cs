using GO.Domain.Models;
using System;
using System.Linq;

namespace GO.Domain.Helpers
{
	public static class SecurityTokenHelper
	{
		private const ushort GuidLength = 16;
		private const ushort DateTimeLength = 8;

		public static string Generate(SecurityToken model)
		{
			var expirationDate = BitConverter.GetBytes(model.TimeStamp.ToBinary());
			var issuer = model.Issuer.ToByteArray();
			var key = model.Key.ToByteArray();

			var tokenBytes = new byte[issuer.Length + key.Length + expirationDate.Length];

			Buffer.BlockCopy(expirationDate, default, tokenBytes, default, expirationDate.Length);
			Buffer.BlockCopy(key, default, tokenBytes, expirationDate.Length, key.Length);
			Buffer.BlockCopy(issuer, default, tokenBytes, expirationDate.Length + key.Length, issuer.Length);

			return Convert.ToBase64String(tokenBytes.ToArray());
		}

		public static bool Validate(string token) => 
			TryParse(token, out _);

		public static bool TryParse(string token, out SecurityToken model)
		{
			model = default;

			if (string.IsNullOrWhiteSpace(token) || (token.Length * 6) % 8 != default)
				return false;

			try
			{
				var tokenBytes = Convert.FromBase64String(token);

				model = new SecurityToken(GetKey(tokenBytes), GetIssuer(tokenBytes), GetExpirationDate(tokenBytes));
			}
			catch
			{
				return false;
			}

			return true;
		}

		private static DateTime GetExpirationDate(byte[] tokenBytes) =>
			DateTime.FromBinary(BitConverter.ToInt64(tokenBytes, default));

		private static Guid GetKey(byte[] tokenBytes) =>
			new(tokenBytes.Skip(DateTimeLength).Take(GuidLength).ToArray());

		private static Guid GetIssuer(byte[] tokenBytes) =>
			new(tokenBytes.Skip(DateTimeLength + GuidLength).ToArray());
	}
}
