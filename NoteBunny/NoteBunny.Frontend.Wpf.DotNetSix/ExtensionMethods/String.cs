using System.Linq;

namespace NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods
{
    public static class String
    {
        /// <summary>
        /// Replace all non-numeric characters in a string with the given character.
        /// </summary>
        public static string ReplaceAlphanumeric(this string phrase, char character)
        {
            return string.Concat(phrase.Select(x => char.IsNumber(x) ? x : character));
        }
    }
}
