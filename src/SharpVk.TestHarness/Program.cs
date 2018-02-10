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

                using (var window = new Window(1024, 768, "Test"))
                {
                    while (window.GetKeyState(Key.Space) == InputAction.Release)
                    {
                        Glfw3.PollEvents();
                    }
                }
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
