using System.Diagnostics;

namespace CsTests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void Test()
    {
        Exception exc = new ArgumentNullException("dasdas");
        if (exc is ArgumentNullException)
            Trace.WriteLine($"Success");
        if (exc is Exception)
            Trace.WriteLine($"Success 2");
        if (exc is AccessViolationException)
            Trace.WriteLine($"Success 3");
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