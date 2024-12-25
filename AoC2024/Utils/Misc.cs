namespace AoC2024.Utils;

public static class Misc
{
    public static void AddOrSet<T1>(this Dictionary<T1, int> dict, T1 key, int value) where T1 : notnull
    {
        dict[key] = dict.TryGetValue(key, out var result) ? result + value : value;
    }

    public static void AddOrSet<T1>(this Dictionary<T1, long> dict, T1 key, long value) where T1 : notnull
    {
        dict[key] = dict.TryGetValue(key, out var result) ? result + value : value;
    }

    public static List<T> RemoveAtIndex<T>(this List<T> list, int index)
    {
        var pre = list.GetRange(0, index);
        var post = list.GetRange(index + 1, list.Count - index - 1);
        return [..pre, ..post];
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
        {
            action(element);
        }
    }

    public static int ToInt(this string value) => int.Parse(value);
    public static long ToLong(this string value) => long.Parse(value);

    public static Tuple<T, T> ToPair<T>(this IEnumerable<T> list)
    {
        var e = list.GetEnumerator();
        e.MoveNext();
        var a = e.Current;
        e.MoveNext();
        var b = e.Current;
        e.Dispose();
        return new Tuple<T, T>(a, b);
    }

    public static (T, T, T) ToTriple<T>(this IEnumerable<T> list)
    {
        var e = list.GetEnumerator();
        e.MoveNext();
        var a = e.Current;
        e.MoveNext();
        var b = e.Current;
        e.MoveNext();
        var c = e.Current;
        e.Dispose();
        return new (a, b, c);
    }

    public static IEnumerable<List<T>> SplitBy<T>(this IList<T> list, T delimiter)
    {
        var result = new List<T>();
        foreach (var el in list)
        {
            if (el.Equals(delimiter))
            {
                yield return result;
                result = new List<T>();
            }
            else
            {
                result.Add(el);
            }
        }

        yield return result;
    }
}