using System.Runtime.ExceptionServices;

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

    public static STask Run(Action action)
    {
        STask t = new STask();

        SThreadPool.QueueUserWorkItem(() =>
        {
            try
            {
                action();
            }
            catch(Exception e)
            {
                t.SetException(e);
                return;
            }
            
            t.SetResult();
        });
        
        return t;
    }

    public static STask WhenAll(List<STask> tasks)
    {
        STask t = new();
        
        if(tasks.Count == 0)
        {
            t.SetResult();
            return t;
        }
        
        int remainingTasks = tasks.Count;
        Action continuation = () =>
        {
            if (Interlocked.Decrement(ref remainingTasks) == 0)
            {
                t.SetResult();
            }
        };
        
        foreach (var task in tasks)
        {
            task.ContinueWith(continuation);
        }
        
        return t;
    }

    public void SetResult() => Complete(null);
    
    public void SetException(Exception exception) => Complete(exception);

    public void Wait()
    {
        ManualResetEventSlim? mres = null;
        lock (this)
        {
            if (!_completed)
            {
                mres = new ManualResetEventSlim();
                ContinueWith(mres.Set);
            }
        }

        mres?.Wait();

        if (_exception is not null)
            ExceptionDispatchInfo.Throw(_exception);
    }

    public void ContinueWith(Action continuation)
    {
        lock (this)
        {
            if (_completed)
            {
                SThreadPool.QueueUserWorkItem(continuation);
            }
            else
            {
                _continuation = continuation;
                _context = ExecutionContext.Capture();
            }
        }
    }
    
    private void Complete(Exception? exception)
    {
        lock (this)
        {
            if(_completed) throw new InvalidOperationException(" Task already completed");

            _completed = true;
            _exception = exception;

            if (_continuation is not null)
            {
                SThreadPool.QueueUserWorkItem(delegate
                {
                    if (_context is null)
                    {
                        _continuation();
                    }
                    else
                    {
                        ExecutionContext.Run(_context, state => ((Action)state!).Invoke(), _continuation);
                    }
                });
            }
        }   
    }
}