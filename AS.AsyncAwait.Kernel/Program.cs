namespace AS.AsyncAwait.Kernel;

public class Program
{
    public static void Main(string[] args)
    {
        AsyncLocal<int> currentTask = new();
        List<STask> tasks = new();
        for(int i = 0; i < 100; i++)
        {
            currentTask.Value = i;
            tasks.Add(STask.Run(() =>
            {
                Console.WriteLine(currentTask.Value);
                Thread.Sleep(1000);
            }));
        }

        STask.WhenAll(tasks).Wait();
        
    }
}

