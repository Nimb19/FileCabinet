using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace CsTests;

[TestClass]
public class UnitTests
{
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