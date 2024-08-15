The definition of a asynchronous method may be understood as a method which the control is expected to
be returned to the caller before the workload itself associated with the whole method operation is finished. 

When we start to think about 'async' methods, one of the first things that comes to our mind is the concept of concurrency.
We start something and go off to do something else at the same time, and when it completes, perhaps we need to do 
something with the result, or perhaps not.
So, this leads to the problem of how to handle with multiple pieces of work at same time, which leads to a Thread Pool.

So, first let's understand the SThreadPool class.

## SThreadPool

So, the SThreadPool class is a class that is responsible for managing parallel tasks using many threads as we need. 

For now, it's basically have a `QueueUserWorkItem` method that allows us to add a new task into the thread pool.

So, as we need to Queue things, obviously we need some sort of structure to store the data. There's a lot of structures 
that may be used (Concurrent Stack, Concurrent Bag, Concurrent Queue, etc), but for now, we are using a simple 
`BlockingCollection`.

The `BlockingCollection` is a thread-safe collection that supports adding and removing elements from the collection 
in a thread-safe manner - which means that when we need to take something out, it will simply return this 'something',
however if there's nothing to take out, so the thread is put into 'waiting' state
until there's something to take out (Essentially, the thread is blocked). We want all the threads into a state where it 
will try to take things from this collection to process it, and if there's nothing, it will wait until there's something
to process.

Into the `BlockingCollection`, we will store many `Actions` and the correspondent `ExecutionContext`.

### Actions && Delegate

Before talking about the 'Action' let's understand what is a Delegate. 

Delegate is a type that represents references to methods with a particular parameter list and return type. Essentially,
is a pointer to a method that can be invoked. When we execute a delegate, we are invoking the method that it points to.
(https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/)

The `Action` is just a kind of delegate that represents a method that takes no parameters and returns void. Simple like
this.

### ExecutionContext

The ExecutionContext is a class that provides a way to capture and transfer the execution context. The data captured are
things like current culture, and the thread call stack. Each thread has its call stack to register the functions executed 
and the ExecutionContext captures this information and allows us to transfer it to another thread, using the Thread Local
Storage. When we call the `ExecutionContext.Capture` method, it will return a new instance of the ExecutionContext with information
of the call stack of the current thread and store it into the Thread Local Storage. At the moment whe call `ExecutionContext.Run`,
the system will search method into the current thread TLS the `ExecutionContext` passed as an argument to the `Run()`.
So, we restore the call stack of the current thread with the information stored into the Thread Local Storage. The code 
inside the Lambda expression passed as an argument to the `Run()` method will be executed with the call stack of the 
`ExecutionContext` passed as an argument. After the execution of the code inside the Lambda expression, the system will
restore the call stack of the current thread with the information stored into the Thread Local Storage before the call.

## Back to SThreadPool

Okay,so the SThreadPool is just a `while(true)` loop that will take things from the `BlockingCollection` and execute it.

