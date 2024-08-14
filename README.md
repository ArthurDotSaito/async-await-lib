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

Into the `BlockingCollection`, we will store many `Actions`.

### Actions && Delegate

Before talking about the 'Action' let's understand what is a Delegate. 

Delegate is a type that represents references to methods with a particular parameter list and return type. Essentially,
is a pointer to a method that can be invoked. When we execute a delegate, we are invoking the method that it points to.
(https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/)


