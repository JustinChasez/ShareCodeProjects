using System.Collections.Concurrent;

namespace System.Threading;

internal static class ParallelExtensions
{
    public static Task ParallelForEachAsync<T>(this IEnumerable<T> source,
                                               int                 degreeOfParallelism,
                                               Func<T, Task>       body)
    {
        async Task AwaitPartition(IEnumerator<T> partition)
        {
            using (partition)
            {
                while (partition.MoveNext())
                {
                    await body(partition.Current);
                }
            }
        }

        return Task.WhenAll(
                            Partitioner
                               .Create(source)
                               .GetPartitions(degreeOfParallelism)
                               .AsParallel()
                               .Select(AwaitPartition));
    }
}