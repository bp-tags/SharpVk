using System;
using System.Collections.Generic;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Represents an instance of a GLFW3 Window.
    /// </summary>
    public class Window
    {
        internal readonly WindowHandle handle;

        /// <summary>
        /// Creates a window and its associated OpenGL or OpenGL ES context.
        /// </summary>
        /// <param name="width">
        /// The desired width, in screen coordinates, of the window. This must
        /// be greater than zero.
        /// </param>
        /// <param name="height">
        /// The desired height, in screen coordinates, of the window. This must
        /// be greater than zero.
        /// </param>
        /// <param name="title">
        /// The initial window title.
        /// </param>
        /// <param name="windowHints">
        /// The set of hints to set before creating the window.
        /// </param>
        public Window(int width, int height, string title, Dictionary<WindowAttribute, int> windowHints = null)
        {
            if (windowHints != null)
            {
                foreach (var hintPair in windowHints)
                {
                    Glfw3.WindowHint(hintPair.Key, hintPair.Value);
                }
            }

            this.handle = Glfw3.CreateWindow(width, height, title, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Returns the value of the close flag for this window.
        /// </summary>
        public bool ShouldClose => Glfw3.WindowShouldClose(this.handle);
    }
}
