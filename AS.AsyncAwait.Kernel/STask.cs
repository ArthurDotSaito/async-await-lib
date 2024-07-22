namespace AS.AsyncAwait.Kernel;

public class STask
{
    private bool _completed;
    private Exception? _exception;
    private Action? _continuation;
    private ExecutionContext? _context;

    public bool IsCompleted
    {
        get
        {
            lock (this)
            {
                return _completed;
            }
        }
    }

    public void SetResult(){}
    public void SetException(Exception exception){}
    public void Wait(){}
    public void ContinueWith(Action continuation){}
}