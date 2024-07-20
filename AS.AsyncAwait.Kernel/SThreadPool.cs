using System.Collections.Concurrent;

namespace AS.AsyncAwait.Kernel;

public static class SThreadPool
{
    private static readonly BlockingCollection<Action> _tasks = new();
    public static void QueueTask(Action task) => _tasks.Add(task);

    static SThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    
                }
            }){IsBackground = true}.Start();
        }
    }
}