using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading;

internal static class TaskExt
{
    public static async Task<T[]> WhenAll<T>(params Task<T>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            return await allTasks;
        }
        catch (Exception)
        {
            //ignore
        }

        throw allTasks.Exception ??
              throw new UnreachableException("This can't happen");
    }

    public static async Task WhenAll(this IEnumerable<Task> tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            await allTasks;

            return;
        }
        catch (Exception)
        {
            //ignore
        }

        throw allTasks.Exception ??
              throw new UnreachableException("This can't happen");
    }

    public static async Task WhenAll(params Task[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            await allTasks;
        }
        catch (Exception)
        {
            //ignore
        }

        throw allTasks.Exception ??
              throw new UnreachableException("This can't happen");
    }

    public static TaskAwaiter<(T, T)> GetAwaiter<T>(this (Task<T>, Task<T>) tasksTuple)
    {
        async Task<(T, T)> CombineTasks()
        {
            var (task1, task2) = tasksTuple;
            await WhenAll(task1, task2);

            return (task1.Result, task2.Result);
        }

        return CombineTasks().GetAwaiter();
    }

    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>) tasksTuple)
    {
        return WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3).GetAwaiter();
    }

    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>, Task<T>) tasksTuple)
    {
        return WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3, tasksTuple.Item4).GetAwaiter();
    }
}