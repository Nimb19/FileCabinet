using System.Diagnostics;
using System.Text;

namespace CsTests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void test()
    {
        var str1 = "hello";
        var pstr = "hell";
        var str2 = pstr + "o";// new StringBuilder().Append("hell").Append("o").ToString();
        var oStr1 = (object)str1;
        var oStr2 = (object)str2;
        Trace.WriteLine(str1 == str2);
        Trace.WriteLine(oStr1 == oStr2);
        Trace.WriteLine(str1 == oStr2);

        // https://www.youtube.com/watch?v=M32SEu0hY7w 56 min primerno
        // только на этапе компил€ции одни и те же строки (или одни и те же, но полученные в текущей строке) будут интернированы
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
    public void “естЌа«амыкание()
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