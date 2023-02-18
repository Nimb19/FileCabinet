using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace CsTests;

[TestClass]
public class UnitTests
{
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
    //            $"Неверноей значение при сравнении (expectedResult != result): {expectedResult.val} != {result.val}.{Environment.NewLine}" +
    //            $"Итого было сравнено: [{string.Join(", ", resultList)}]");
    //        resultList.Add(expectedResult.val);
    //        expectedResult = expectedResult.next;
    //        result = result.next;
    //    }
    //}

    [TestMethod]
    public void testsdgsdgs()
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
        //list[0].x = 90;//ошибка компиляции
        // вместо этого присвой заново =>
        list[0] = p;

        var array = new Point[] { p };
        array[0].x = 90;//все ок

        // Потому что индексация в классах это вызов метода, а в массивах - прямое обращение к элементу
        // Но в массиве будет содержаться всё таки копия изначальной структуры

        Trace.WriteLine($"Success: array[0].x={array[0].x}; p={p.x}");
    }

    public class StatTest1
    {
        // argumentnullexception. Порядок имеет значение
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
        Assert.IsTrue(t.Wait(5000), "за 5 сек не завершился");
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
                Trace.WriteLine($"Отдал: {i}");

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
                Trace.WriteLine($"Начал ждать");
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    var prishlo = $"Пришёл: {item}";
                    Trace.WriteLine(prishlo);
                    i++;

                    if (i == max)
                    {
                        i = 0;
                        chet = !chet;
                        Trace.WriteLine($"5 штук готовы");
                        //bc.CompleteAdding();
                        if (chet)
                        {
                            Task.Delay(2000).Wait();
                            Trace.WriteLine("Закрываюсь, чётно");
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
        // только на этапе компиляции одни и те же строки будут интернированы
        // (или одни и те же, но полученные в текущей строке (pstr + "o" не считается за формирование в одной сроке, считается "hell" + "o"))

        // Интернирована ли строка можно проверить String.IsIntern, можно так же самому добавить если надо, но тогда они останутся до завершения приложения
        // (object)"Hi" == (object)"Hi" всегда истинно, спасибо интернированию.
        // Попробуйте проверить этот код в окне интерпретации, и результат будет отрицательным, так как отладчик не интернирует ваши строки.
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
    public void ТестНаЗамыкание()
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