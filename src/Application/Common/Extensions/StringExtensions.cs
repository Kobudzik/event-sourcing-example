using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EventSourcingExample.Application.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// ToLower, replace spaces with dashes, normalize polish, remove special characters
        /// </summary>
        public static string ToPrestaUrlFormat(this string input)
        {
            return input?.ToLower().Replace(" ", "-")?.NormalizePolishCharacters()?.RemoveSpecialCharactersExceptDashAndSpaces();
        }

        public static string RemoveSpecialCharactersExceptDashAndSpaces(this string input)
        {
            var r = new Regex("(?:[^a-z0-9 -]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, string.Empty);
        }

        public static string NormalizePolishCharacters(this string input)
        {
            var original = new List<string> { "Ą", "ą", "Ć", "ć", "Ę", "ę", "Ł", "ł", "Ń", "ń", "Ó", "ó", "Ś", "ś", "Ź", "ź", "Ż", "ż" };
            var target = new List<string> { "A", "a", "C", "c", "E", "e", "L", "l", "N", "n", "O", "o", "S", "s", "Z", "z", "Z", "z" };

            var result = input.Select(x =>
            {
                var indexInOriginal = original.IndexOf(x.ToString());
                if (indexInOriginal != -1)
                    x = target[indexInOriginal][0];
                return x;
            });

            return string.Join("", result);
        }
    }
}
