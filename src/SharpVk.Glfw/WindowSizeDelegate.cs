using System.Runtime.InteropServices;

namespace SharpVk.Glfw
{
    /// <summary>
    /// The function signature for window size callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that was resized.
    /// </param>
    /// <param name="width">
    /// The new width, in screen coordinates, of the window.
    /// </param>
    /// <param name="height">
    /// The new height, in screen coordinates, of the window.
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowSizeDelegate(WindowHandle window, int width, int height);
}