using System;

namespace EP.Services.Extensions
{
    public static class StringExtensions
    {
        public static string TrimNull(this string value)
            => string.IsNullOrEmpty(value) ? value : value.Trim();
    }
}