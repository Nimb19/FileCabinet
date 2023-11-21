using CsLib;
using System.Security.Principal;

namespace CsConsole;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Start");

        await TestAsyncStaticClass.Method1();
        await TestAsyncStaticClass.Method2();
        await TestAsyncStaticClass.Method3();
        await TestAsyncStaticClass.Method4();

        Console.WriteLine("Complete 1");

        var testAsyncClass = new TestAsyncClass();
        await testAsyncClass.Method1();
        await testAsyncClass.Method2();
        await testAsyncClass.Method3();
        await testAsyncClass.Method4();

        Console.WriteLine("Complete 2");

        //Console.WriteLine("Before VoidAsync");
        //await VoidAsync();
        //Console.WriteLine("After VoidAsync");

        //async Task<string> VoidAsync()
        //{
        //    Console.WriteLine($"VoidAsync Start");
        //    var task = Task.Run(() => Thread.Sleep(1000));
        //    Console.WriteLine("VoidAsync Task started");
        //    await task;
        //    Console.WriteLine("VoidAsync Task completed");
        //    return await Task.Run(() => "str");
        //}

        //try
        //{
        //    var cts = new CancellationTokenSource();
        //    var cancToken = cts.Token;
        //    Console.CancelKeyPress += (s, e) =>
        //    {
        //        cts.Cancel();
        //        e.Cancel = true;
        //    };

        //    //Method().Wait();

        //    //testsdgsd123123sasfasf(cancToken).Wait(cancToken);

        //    TestDeadLock();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex);
        //}
    }

    //static void TestDeadLock()
    //{
    //    const int tc = 10;
    //    const int maxtc = 20;
    //    ThreadPool.SetMaxThreads(maxtc, maxtc);
    //    ThreadPool.SetMinThreads(tc, tc);

    //    WriteThreadInfo(0, "Start");
    //    for (int i = 0; i < tc; i++)
    //    {
    //        WriteThreadInfo(i, $"Queue {i}");
    //        ThreadPool.QueueUserWorkItem(CallBack);
    //    }

    //    WriteThreadInfo(0, "End");
    //    Console.ReadKey();
    //}

    //private static void CallBack(object state)
    //{
    //    WriteThreadInfo(0, "Callback");
    //    Thread.Sleep(1000);

    //    WriteThreadInfo(1, "Callback start Foo");
    //    var result = Foo().Result;
    //    WriteThreadInfo(1, "Callback end Foo");
    //}

    //public static HttpClient HttpClient { get; set; } = new HttpClient();

    //public static async Task<string> Foo()
    //{
    //    WriteThreadInfo(1, "Result 1");
    //    await HttpClient.GetAsync("https://www.google.com/");
    //    WriteThreadInfo(2, "Result 2");
    //    return "";
    //}

    ////public static async Task Method()
    ////{
    ////    Console.WriteLine("Before VoidAsync");
    ////    await VoidAsync().ConfigureAwait(false);
    ////    Console.WriteLine("After VoidAsync");
    ////}

    ////private async static Task VoidAsync()
    ////{
    ////    Console.WriteLine("Void async work");
    ////    using var httpClient = new HttpClient();
    ////    await httpClient.GetAsync($"https://www.google.com/");
    ////    //await Task.Delay(1000);
    ////    Console.WriteLine("Void async end");
    ////}

    //#region test threadpool deadlock

    ////public static async Task testsdgsd123123sasfasf(CancellationToken cancToken)
    ////{
    ////    Console.WriteLine($"SyncContext: {SynchronizationContext.Current?.ToString() ?? "null"}");
    ////    Console.WriteLine($"TID: {Environment.CurrentManagedThreadId}");
    ////    Console.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

    ////    ThreadPool.SetMinThreads(4, 4);
    ////    //ThreadPool.SetMaxThreads(10000, 10000);
    ////    Console.WriteLine($"Seted ThreadPool.SetMinThreads(4, 4);");
    ////    Console.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

    ////    ThreadPool.GetMinThreads(out var workerThreads, out var asyncIoThreads);
    ////    Console.WriteLine($"ThreadPool.GetMinThreads: workerThreads={workerThreads}; asyncIoThreads={asyncIoThreads}");

    ////    ThreadPool.GetMaxThreads(out workerThreads, out asyncIoThreads);
    ////    Console.WriteLine($"ThreadPool.GetMaxThreads: workerThreads={workerThreads}; asyncIoThreads={asyncIoThreads}");

    ////    var timer = new Timer(_ => WriteThreadInfo(-1, "Таймер click"), null, 10, 1000);
    ////    var tasks = await StartTasksAsync(cancToken);
    ////    var tasks2 = await StartTasksAsync(cancToken);

    ////    tasks.AddRange(tasks2);
    ////    Task.WaitAll(tasks.ToArray(), cancToken); // .Select(x => x.Unwrap())

    ////    //Console.WriteLine();
    ////    //var prefix = "        [main]";
    ////    //WriteThreadInfo(prefix, 0, "перед await async метода");
    ////    //await StartAsyncMethod(0);

    ////    ////Task.Delay(1500).Wait();

    ////    //WriteThreadInfo(prefix, 1, "после await async метода. Выполняю его без await");
    ////    //Console.WriteLine();
    ////    //var task = StartAsyncMethod(1);

    ////    ////await task;

    ////    //WriteThreadInfo(prefix, 2, "после активации async метода, но до await, жду 1.5 сек");

    ////    //Thread.Sleep(500);
    ////    //Thread.Sleep(500);
    ////    //Thread.Sleep(500);
    ////    ////Task.Delay(500).Wait();
    ////    ////Task.Delay(500).Wait();
    ////    ////Task.Delay(500).Wait();

    ////    //WriteThreadInfo(prefix, 3, "после активации async метода, но до await, после 1.5 секt");

    ////    //await task;
    ////    //Console.WriteLine();

    ////    //WriteThreadInfo(prefix, 4, "после await async method'а");
    ////}

    ////private static async Task<List<Task>> StartTasksAsync(CancellationToken cancToken)
    ////{
    ////    var tasks = new List<Task>();
    ////    for (int i = 0; i < 8; i++)
    ////    {
    ////        cancToken.ThrowIfCancellationRequested();

    ////        var iInternal = i;
    ////        WriteThreadInfo(iInternal, $"Create task {iInternal}...");

    ////        var task = await Task.Factory.StartNew(async () =>
    ////        {
    ////            WriteThreadInfo(iInternal, $"Start {iInternal}");

    ////            await Task.Delay(5000, cancToken).WaitAsync(cancToken);
    ////            WriteThreadInfo(iInternal, $"First await returned");

    ////            Thread.Sleep(10000);
    ////            WriteThreadInfo(iInternal, $"Thread Sleep returned");

    ////            var secondDelayTask = StartDelayTaskAsync(iInternal, cancToken);

    ////            WriteThreadInfo(iInternal, $"Вышел из StartDelayTaskAsync, таск должен быть создан. Включаю на 2 сек Thread.Sleep");
    ////            Thread.Sleep(2000);
    ////            WriteThreadInfo(iInternal, $"Thread.Sleep на 2 сек завершён, нажимаю await secondDelayTask");

    ////            await secondDelayTask;
    ////            WriteThreadInfo(iInternal, $"Second await returned");

    ////            Thread.Sleep(5000);
    ////            WriteThreadInfo(iInternal, $"Second thread sleep returned");

    ////            WriteThreadInfo(iInternal, $"Complete");
    ////        }, cancToken);

    ////        WriteThreadInfo(iInternal, $"Create task {iInternal} success");

    ////        await Task.Delay(10, cancToken);
    ////        tasks.Add(task);
    ////    }

    ////    return tasks;
    ////}

    ////private static async Task StartDelayTaskAsync(int iInternal, CancellationToken cancToken)
    ////{
    ////    await Task.Delay(5000, cancToken).WaitAsync(cancToken);
    ////    WriteThreadInfo(iInternal, $"StartDelayTaskAsync закончен внутри метода");
    ////}

    ////public static async Task StartAsyncMethod(int opid)
    ////{
    ////    var prefix = "[async method]";

    ////    WriteThreadInfo(prefix, opid, "(0) перед await");

    ////    await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
    ////    WriteThreadInfo(prefix, opid, "(1) после 1 await");

    ////    await Task.Delay(TimeSpan.FromSeconds(1.1)).ConfigureAwait(false);
    ////    WriteThreadInfo(prefix, opid, "(2) после 2 await");

    ////    await Task.Delay(TimeSpan.FromSeconds(1.2)).ConfigureAwait(false);
    ////    WriteThreadInfo(prefix, opid, "(3) complete: после 3 await");
    ////}

    //private static void WriteThreadInfo(int opid, string postfix)
    //{
    //    WriteThreadInfo(string.Empty, opid, postfix);
    //}

    //private static void WriteThreadInfo(string mainPrefix, int opid, string postfix)
    //{
    //    ThreadPool.GetAvailableThreads(out var workerThreads, out var asyncIoThreads);
    //    Console.WriteLine($"[OP= {opid:000}] {DateTime.Now.ToString("HH:mm ss.fff")} " +
    //        $"[TC={ThreadPool.ThreadCount:000}; " +
    //        $"TAWorkers={workerThreads}; TAAsyncIo={asyncIoThreads}] " +
    //        $"{mainPrefix} " +
    //        $"TID={Environment.CurrentManagedThreadId:00} " +
    //        $"OP= {opid:000} " +
    //        $"`Desc: {postfix}`");
    //}
    //#endregion test threadpool deadlock
}
