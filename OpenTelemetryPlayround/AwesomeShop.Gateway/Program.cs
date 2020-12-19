using SS.WebHostBuilder;

namespace AwesomeShop.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostBuilder.Create<Startup>(args)
                .UseContentRoot()
                .UseDefaultAppConfiguration()
                .UseOcelot()
                .UseSeriLog()
                .Build()
                .Run();
        }
    }
}
