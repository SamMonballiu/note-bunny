using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.BLL.Extensions
{
    internal static class Extensions
    {
        internal static bool ContainsAny(this string me, IEnumerable<string> others)
        {
            return others.Any(x => me.ToLowerInvariant().Contains(x.ToLowerInvariant()));
        }
    }
}
