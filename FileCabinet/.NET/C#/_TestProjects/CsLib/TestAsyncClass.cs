namespace CsLib
{
    public class TestAsyncStaticClass
    {
        public static async Task Method1()
        {
            await Task.Delay(444);
        }

        public static Task Method2()
        {
            return Task.Delay(444);
        }

        public static async Task<string> Method3()
        {
            return await Task.Run(() => { Thread.Sleep(444); return "asfasf"; });
        }

        public static Task<string> Method4()
        {
            return Task.Run(() => { Thread.Sleep(444); return "asfasf"; });
        }
    }

    public class TestAsyncClass
    {
        public async Task Method1()
        {
            await Task.Delay(444);
        }

        public Task Method2()
        {
            return Task.Delay(444);
        }

        public async Task<string> Method3()
        {
            return await Task.Run(() => { Thread.Sleep(444); return "asfasf"; });
        }

        public Task<string> Method4()
        {
            return Task.Run(() => { Thread.Sleep(444); return "asfasf"; });
        }
    }
}