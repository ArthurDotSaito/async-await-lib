The definition of a asynchronous method may be understood as a method which the control is expected to
be returned to the caller before the workload itself associated with the whole method operation is finished. 

When we start to think about 'async' methods, one of the first things that comes to our mind is the concept of concurrency.
We start something and go off to do something else at the same time, and when it completes, perhaps we need to do something with the result, or perhaps not.
So, this leads to the problem of how to handle with multiple pieces of work at same time, which leads to a Thread Pool.