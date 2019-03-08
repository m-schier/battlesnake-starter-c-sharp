using System;

namespace StarterSnake
{
    /// <summary>
    /// This class contains the entry point of the assembly
    /// </summary>
    public class Startup
    {
        static void Main(string[] args)
        {
            // Get port from heroku environment or set to default of 5050
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5050";

            // Bind top level wild card domain name and port
            var root = "http://*:" + port + "/";

            // Replace starter controller with a smarter controller here
            // or just tweak StarterController
            var controller = new StarterController();

            using (var server = new Service.Server(root, controller)) {
                Console.Error.WriteLine($"[ INFO ] Starting server on {root}");
                server.Run();
            }
        }
    }
}
