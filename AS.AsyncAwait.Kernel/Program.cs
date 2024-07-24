namespace AS.AsyncAwait.Kernel;

public class Program
{
    static async Task PrintSTask()
    {
        for (int i = 0;; i++)
        {
            await STask.Delay(1000);
            Console.WriteLine(i);
        }
    }
}

