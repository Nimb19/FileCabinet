namespace System.Runtime.CompilerServices
{
    // AsyncVoidMethodBuilder.cs in your project
    public class AsyncVoidMethodBuilder
    {
        public AsyncVoidMethodBuilder()
            => Console.WriteLine(".ctor");

        public static AsyncVoidMethodBuilder Create()
            => new AsyncVoidMethodBuilder();

        public void SetStateMachine(IAsyncStateMachine stateMachine) => Console.WriteLine("SetStateMachine");

        public void SetResult() => Console.WriteLine("SetResult");

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        public void SetException(Exception exception)
        {
            Console.WriteLine("SetException");
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
           ref TAwaiter awaiter, ref TStateMachine stateMachine)
           where TAwaiter : INotifyCompletion
           where TStateMachine : IAsyncStateMachine =>
           Console.WriteLine("AwaitOnCompleted");

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            Console.WriteLine("AwaitUnsafeOnCompleted");

        // AwaitOnCompleted, AwaitUnsafeOnCompleted, SetException 
        // and SetStateMachine are empty
    }
}
