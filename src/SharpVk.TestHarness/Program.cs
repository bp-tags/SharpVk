using SharpVk.Glfw;
using System;

namespace SharpVk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().Run();
        }

        private unsafe void Run()
        {
            try
            {
                Glfw3.Init();

                var monitor = Monitor.GetPrimary();

                foreach(var mode in monitor.GetVideoModes())
                {
                    Console.WriteLine(mode);
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                Glfw3.Terminate();
            }
        }
    }
}
