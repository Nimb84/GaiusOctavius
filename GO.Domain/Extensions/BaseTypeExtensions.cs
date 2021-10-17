using System;

namespace GO.Domain.Extensions
{
	public static class BaseTypeExtensions
	{
		public static string ToAlphanumeric(this Guid value) =>
			value.ToString().Replace("-", string.Empty);
	}
}
