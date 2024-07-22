using System.Collections.Concurrent;

namespace AS.AsyncAwait.Kernel;

public static class SThreadPool
{
    private static readonly BlockingCollection<(Action, ExecutionContext?)> s_workitems = new();
    public static void QueueUserWorkItem(Action task) => s_workitems.Add((task, ExecutionContext.Capture()));

    static SThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    (Action workItem, ExecutionContext? context) = s_workitems.Take();
                    workItem();
                }
            }){IsBackground = true}.Start();
        }
    }
}