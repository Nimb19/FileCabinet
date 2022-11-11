using System.Diagnostics;

namespace CsTests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void Test()
    {
        Trace.WriteLine($"{new Exception("test")}");
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