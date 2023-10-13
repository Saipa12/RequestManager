namespace RequestManager.Core.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> GetOrEmpty<TSource>(this IEnumerable<TSource> collection) => collection ?? Enumerable.Empty<TSource>();

    public static IEnumerable<TSource> NullIfEmpty<TSource>(this IEnumerable<TSource> source) => source.HasItems() ? source : null;

    public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source) => source ?? Enumerable.Empty<TSource>();

    public static bool HasItems<TSource>(this IEnumerable<TSource> source) => source?.Any() ?? false;

    public static bool HasItems<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) => source?.Any(predicate) ?? false;
}