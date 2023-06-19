using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsTests;

[TestClass]
public class AsyncTests
{
    [TestMethod]
    public async Task AsyncLockTest()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        WriteThreadId("AsyncLockTest before foo");
        await Foo();
        WriteThreadId("AsyncLockTest after foo");
    }

    private static void WriteThreadId(string context)
    {
        Trace.WriteLine($"[{context}] TID: {Thread.CurrentThread.ManagedThreadId}; Name: {Thread.CurrentThread.Name}");
    }

    private static async Task Foo()
    {
        var _lock = new AsyncLock();
        var unlock = await _lock.Lock();

        for (var i = 0; i < 15; i++)
        {
            WriteThreadId("Foo before Bar");
            Bar(_lock);
        }

        unlock();
    }

    private static async void Bar(AsyncLock _lock)
    {
        var unlock = await _lock.Lock();

        WriteThreadId("Bar after geted lock");

        // do something sync
        unlock();
    }

    class AsyncLock
    {
        private Task unlockedTask = Task.CompletedTask;

        public async Task<Action> Lock()
        {
            var tcs = new TaskCompletionSource<object>();

            await Interlocked.Exchange(ref unlockedTask, tcs.Task);

            WriteThreadId("Lock after exchange");

            return () => tcs.SetResult(null);
        }
    }
}
