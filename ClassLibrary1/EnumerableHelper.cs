using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace ClassLibrary1;

public static partial class EnumerableHelper
{
    public static IEnumerable<T?> Repeat<T>(this IEnumerable<T> _)
    {
        while (true)
        {
            yield return default;
        }
    }

    public static IEnumerable<T?> Repeat<T>()
    {
        while (true)
        {
            yield return default;
        }
    }

    public static IEnumerable<T?> Count<T>(this IEnumerable<T> _, uint? count = null)
    {
        while (count != null)
        {
            yield return default;
        }
        for (int i = 0; count != null && i < count; i++)
        {
            yield return default;
        }
    }

    public static IEnumerable<T?> Count<T>(uint? count = null)
    {
        // if count is null, then it will be an infinite loop
        // if count is not null, then it will be a loop that runs count times
        // if count is 0, then it will be an empty loop

        while (count == null)
        {
            yield return default;
        }
        for (uint i = 0; count != null && i < count; i++)
        {
            yield return default;
        }
    }
}