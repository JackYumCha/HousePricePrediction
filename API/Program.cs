using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MvcAngular.Generator;



namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (AngularGenerator.ShouldRunMvc(args))
            {
                BuildWebHost(args).Run();
            }
        }

        public static void LoadDynamic()
        {

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:80")
                .Build();
    }

}