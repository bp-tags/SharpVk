using SharpVk.Glfw;
using System;
using System.Text;

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

                Console.WriteLine(Glfw3.GetMonitorName(Glfw3.GetPrimaryMonitor()).Value);

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
