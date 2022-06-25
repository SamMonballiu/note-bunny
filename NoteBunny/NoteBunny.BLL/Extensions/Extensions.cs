using NoteBunny.BLL.Enums;
using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.BLL.Extensions
{
    internal static class Extensions
    {
        internal static bool Contains(this string me, IEnumerable<string> others, MatchType match)
        {
            return match switch
            {
                MatchType.Any => me.ContainsAny(others),
                MatchType.All => me.ContainsAll(others),
                _ => throw new System.NotImplementedException(),
            };
        }

        internal static bool ContainsAny(this string me, IEnumerable<string> others)
        {
            return others.Any(x => me.ToLowerInvariant().Contains(x.ToLowerInvariant()));
        }

        internal static bool ContainsAll(this string me, IEnumerable<string> others)
        {
            return others.All(x => me.ToLowerInvariant().Contains(x.ToLowerInvariant()));
        }
    }
}
