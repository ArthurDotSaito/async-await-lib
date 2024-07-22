namespace AS.AsyncAwait.Kernel;

public class Program
{
    public static void Main(string[] args)
    {
        AsyncLocal<int> currentTask = new();
        for(int i = 0; i < 1000; i++)
        {
            currentTask.Value = i;
            SThreadPool.QueueUserWorkItem(delegate
            {
                Console.WriteLine($"Task {currentTask.Value} started");
                Thread.Sleep(1000);
            });
        }
        Console.ReadLine();
    }
}

