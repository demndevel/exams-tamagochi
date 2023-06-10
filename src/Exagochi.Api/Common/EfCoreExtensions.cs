namespace Exagochi.Api.Common;

public static class EfCoreExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
        var elements = source.ToArray();
        for (var i = elements.Length - 1; i >= 0; i--)
        {
            var swapIndex = rng.Next(i + 1);
            yield return elements[swapIndex];
            elements[swapIndex] = elements[i];
        }
    }
}