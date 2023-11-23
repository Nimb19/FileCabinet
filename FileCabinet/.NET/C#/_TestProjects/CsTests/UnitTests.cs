using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Net.NetworkInformation;

namespace CsTests;

[TestClass]
public class UnitTests
{
    private int _test;

    [TestMethod]
    public async Task testsdgsd123123sas123()
    {
        Console.WriteLine(_test);
        Console.WriteLine(Interlocked.CompareExchange(ref _test, 1, 0));
        Console.WriteLine(_test);
        Console.WriteLine(Interlocked.CompareExchange(ref _test, 0, 1));
        Console.WriteLine(_test);
    }

    [TestMethod]
    public async Task testsdgsd123123sas()
    {
        using var rwl = new ReaderWriterLockSlim();
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        var task = Task.Run(() =>
        {
            Thread.Sleep(1000);
            rwl.EnterReadLock();
            WriteLineWithTime("���� Enter �� 2 ���");

            tcs.SetResult(true);
            WriteLineWithTime("�������� ������");
            Thread.Sleep(2000);

            WriteLineWithTime("�������� ��������");
            rwl.ExitReadLock();
            WriteLineWithTime("�������� ��� ��������");
        });

        WriteLineWithTime("������, ��� Read ��� ����� ����");
        await tcs.Task;
        WriteLineWithTime("�������� �����");
        
        rwl.EnterWriteLock();
        WriteLineWithTime("���� Write ���, ��� 1 ���");

        Thread.Sleep(3000);

        WriteLineWithTime("������� �������� Write ���");
        rwl.ExitWriteLock();
        WriteLineWithTime("������� Write ���");

    }

    private void WriteLineWithTime(string text)
    {
        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm ss.fff")}] {text}");
    }

    //[TestMethod]
    //public void testsdgdfyhsdgs()
    //{
    //    // [[1,4,5],[1,3,4],[2,6]]
    //    var testNodes = new List<ListNode> { new(1, new(4, new(5))), new(1, new(3, new(4))), new(2, new(6)) };
    //    // [1,1,2,3,4,4,5,6]
    //    var expectedResult = new ListNode(1, new(1, new(2, new(3, new(4, new(4, new(5, new(6))))))));

    //    var sln = new Solution();
    //    var result = sln.MergeKLists(testNodes.ToArray());

    //    var resultList = new List<int>();
    //    while (expectedResult != null)
    //    {
    //        Assert.IsTrue(expectedResult.val == result.val, 
    //            $"��������� �������� ��� ��������� (expectedResult != result): {expectedResult.val} != {result.val}.{Environment.NewLine}" +
    //            $"����� ���� ��������: [{string.Join(", ", resultList)}]");
    //        resultList.Add(expectedResult.val);
    //        expectedResult = expectedResult.next;
    //        result = result.next;
    //    }
    //}

    [TestMethod]
    public async Task testsdgsd123123sasfasf()
    {
        Trace.WriteLine($"SyncContext: {SynchronizationContext.Current?.ToString() ?? "null"}");
        Trace.WriteLine($"TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        //ThreadPool.SetMinThreads(10, 10);
        //ThreadPool.SetMaxThreads(30, 30);
        //Trace.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        //var cts = new CancellationTokenSource(60000);
        //var cancToken = cts.Token;

        //var tasks = new List<Task>();
        //for (int i = 0; i < 50; i++)
        //{
            //cancToken.ThrowIfCancellationRequested();

            //var iInternal = i;
            //var task = Task.Factory.StartNew(() =>
            //{
                //Trace.WriteLine($"{DateTime.Now.ToString("hh:mm ss.fffff")} [{iInternal:000}] Start");

                //Task.Delay(30000).Wait(cancToken);
                //if (cancToken.IsCancellationRequested)
                //{
                    //Trace.WriteLine($"{DateTime.Now.ToString("hh:mm ss.fffff")} [{iInternal:000}] Canceled");
                    //return;
                //}

                //Trace.WriteLine($"{DateTime.Now.ToString("hh:mm ss.fffff")} [{iInternal:000}]  Complete");
            //}, cancToken);

            //Task.Delay(50).Wait(cancToken);

            //tasks.Add(task);
            //Trace.WriteLine($"ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");
        //}

        //Task.WaitAll(tasks.ToArray());

        Trace.WriteLine($"o 0 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"o 0 ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        await StartAsyncMethod();

        //Task.Delay(1500).Wait();

        Trace.WriteLine($"o 1 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"o 1 ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        var task = StartAsyncMethod().ConfigureAwait(false);

        //await task;

        Trace.WriteLine($"o 2 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"o 2 ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        Task.Delay(500).Wait();

        Trace.WriteLine($"o 3 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"o 3 ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");

        await task;

        Trace.WriteLine($"o 4 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"o 4 ThreadPool.ThreadCount: {ThreadPool.ThreadCount}");
    }

    public async Task StartAsyncMethod()
    {
        Trace.WriteLine($"s 0 TID: {Environment.CurrentManagedThreadId}");

        await Task.Delay(TimeSpan.FromSeconds(1));
        Trace.WriteLine($"s 1 TID: {Environment.CurrentManagedThreadId}");
        await Task.Delay(TimeSpan.FromSeconds(1.1));
        Trace.WriteLine($"s 2 TID: {Environment.CurrentManagedThreadId}");
        await Task.Delay(TimeSpan.FromSeconds(1.2));

        Trace.WriteLine($"s 3 TID: {Environment.CurrentManagedThreadId}");
        Trace.WriteLine($"s complete");
    }

    public delegate void testdelegate();

    public class test1123
    {
        public event testdelegate testdelegate;
    }

    [TestMethod]
    public void testsdgsd123123s()
    {
        var test = new test1123();
        test.testdelegate -= () => { };
        test.testdelegate -= testhandler;
        test.testdelegate += testhandler;
        test.testdelegate -= testhandler;
        test.testdelegate -= testhandler;
    }

    public void testhandler()
    {
        Trace.WriteLine($"testhandler invoked");
    }

    [TestMethod]
    public void testsdgsdgs()
    {
        var ipProps = IPGlobalProperties.GetIPGlobalProperties();
        var tcpConnections = ipProps.GetActiveTcpConnections();

        Trace.WriteLine($"����� {tcpConnections.Length} �������� TCP-�����������");
        Trace.WriteLine("");
        foreach (var connection in tcpConnections)
        {
            Trace.WriteLine("=============================================");
            Trace.WriteLine($"��������� �����: {connection.LocalEndPoint.Address}:{connection.LocalEndPoint.Port}");
            Trace.WriteLine($"����� ���������� �����: {connection.RemoteEndPoint.Address}:{connection.RemoteEndPoint.Port}");
            Trace.WriteLine($"��������� �����������: {connection.State}");
        }
    }

    [TestMethod]
    public void test����()
    {
        var messages = new List<string>();
        var messages2 = new List<int>();
        var result = messages.Max(x => x.Length);
        var result2 = messages2.Max(x => x);

        Trace.WriteLine(result);
        Trace.WriteLine(result2);
    }

    [TestMethod]
    public void testlinq()
    {
        var list = Enumerable.Range(0, 10).ToList();

        var arrRes1 = list.Where(x => x % 2 == 0);
        Trace.WriteLine($"arrRes1.Count() == {arrRes1.Count()}");

        list.Add(11);
        list.Add(12);
        Trace.WriteLine("added 11 i 12");

        var arrRes2 = list.Where(x => x % 2 == 0);
        Trace.WriteLine($"arrRes1.Count() == {arrRes1.Count()}");
        Trace.WriteLine($"arrRes2.Count() == {arrRes2.Count()}");

        Trace.WriteLine($"Success");
    }


    public struct Point
    {
        public int x;
        public int y;
    }

    [TestMethod]
    public void teststructinarray()
    {
        var p = new Point() { x = 5, y = 9 };
        var list = new List<Point>(10);
        list.Add(p);
        //list[0].x = 90;//������ ����������
        // ������ ����� ������� ������ =>
        list[0] = p;

        var array = new Point[] { p };
        array[0].x = 90;//��� ��

        // ������ ��� ���������� � ������� ��� ����� ������, � � �������� - ������ ��������� � ��������
        // �� � ������� ����� ����������� �� ���� ����� ����������� ���������

        Trace.WriteLine($"Success: array[0].x={array[0].x}; p={p.x}");
    }

    public class StatTest1
    {
        // argumentnullexception. ������� ����� ��������
        private static bool b = a.Any();
        private static string a = "5";

        static StatTest1()
        {
            Trace.WriteLine("stat");
        }
    }

    [TestMethod]
    public void teststatic()
    {
        new StatTest1();

        Trace.WriteLine("Success");
    }

    [TestMethod]
    public void testdeadlock()
    {
        int complete = 0;
        var t = new Task(() =>
        {
            bool toggle = false;
            while (complete == 0)
                toggle = !toggle;
            Trace.WriteLine("Success");
        });
        t.Start();
        Thread.Sleep(1000);
        Interlocked.Increment(ref complete);
        Assert.IsTrue(t.Wait(5000), "�� 5 ��� �� ����������");
    }

    public enum testen
    {
        one = 0x0001,
        two = 0x0010,
        three = 0x0100,
        fo = 0x1000
    }

    [TestMethod]
    public void testenum()
    {
        var test = testen.one | testen.two | testen.fo;
        switch (test)
        {
            case testen.one:
                Trace.WriteLine("testen.one");
                break;
            case testen.two:
                Trace.WriteLine("testen.two");
                break;
            case testen.three:
                Trace.WriteLine("testen.three");
                break;
            case testen.fo:
                Trace.WriteLine("testen.fo");
                break;
            default:
                Trace.WriteLine("def");
                break;
        }
    }

    [TestMethod]
    public void testblocking()
    {
        var bc = new BlockingCollection<string>();
        var task1 = Task.Run(() =>
        {
            for (int i = 0; i < 30; i++)
            {
                bc.Add($"{i}");
                Trace.WriteLine($"�����: {i}");

                Task.Delay(500).Wait();
            }
        });
        var task2 = Task.Run(() =>
        {
            var i = 0;
            var chet = true;
            const int max = 5;
            var action = () =>
            {
                Trace.WriteLine($"����� �����");
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    var prishlo = $"������: {item}";
                    Trace.WriteLine(prishlo);
                    i++;

                    if (i == max)
                    {
                        i = 0;
                        chet = !chet;
                        Trace.WriteLine($"5 ���� ������");
                        //bc.CompleteAdding();
                        if (chet)
                        {
                            Task.Delay(2000).Wait();
                            Trace.WriteLine("����������, �����");
                            return;
                        }
                    }
                }
            };
            for (int g = 0; g < 3; g++)
            {
                action();
            }
        });
        task2.Wait();
    }

    [TestMethod]
    public void testintern()
    {
        var str1 = "hello";
        var str2 = "hell" + "o";// new StringBuilder().Append("hell").Append("o").ToString();
        var oStr1 = (object)str1;
        var oStr2 = (object)str2;
        Trace.WriteLine(str1 == str2);
        Trace.WriteLine(oStr1 == oStr2);
        Trace.WriteLine(str1 == oStr2);

        // https://www.youtube.com/watch?v=M32SEu0hY7w 56 min primerno
        // ������ �� ����� ���������� ���� � �� �� ������ ����� �������������
        // (��� ���� � �� ��, �� ���������� � ������� ������ (pstr + "o" �� ��������� �� ������������ � ����� �����, ��������� "hell" + "o"))

        // ������������� �� ������ ����� ��������� String.IsIntern, ����� ��� �� ������ �������� ���� ����, �� ����� ��� ��������� �� ���������� ����������
        // (object)"Hi" == (object)"Hi" ������ �������, ������� ��������������.
        // ���������� ��������� ���� ��� � ���� �������������, � ��������� ����� �������������, ��� ��� �������� �� ����������� ���� ������.
    }

    [TestMethod]
    public void testtimerthrds()
    {
        Trace.WriteLine($"INIT id = {Thread.CurrentThread.ManagedThreadId}");
        var timer = new Timer((state) => Trace.WriteLine(Thread.CurrentThread.ManagedThreadId), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        Task.Delay(60000).Wait();
        Trace.WriteLine("Dispose");
        timer.Dispose();
    }

    [TestMethod]
    public void ���������������()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++)
        {
            var ic = i;
            tasks.Add(new Task(() => { Trace.WriteLine($"ic_{ic}: {i}"); }));
        }
        tasks.ForEach(x => x.Start());
        Task.WaitAll(tasks.ToArray());
    }
}

//public struct test
//{
//    public int MyProperty { get; set; }
//    //public test Test { get; set; }
//    public test2 Test2 { get; set; }
//}

//public struct test2
//{
//    public int MyProperty { get; set; }
//    public test Test { get; set; }
//}