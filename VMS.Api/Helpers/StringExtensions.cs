using System;

namespace VMS.Api.Helpers
{
    public static class StringExtensions
    {
        private static readonly char[] SplitChars = new char[] { ',', ' ' };

        public static string[]? SplitByCommonChars(this string text)
        {
            return text.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}