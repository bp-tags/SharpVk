using SharpVk.Khronos;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Provides a set of static extensions for Vulkan Instances to support
    /// GLFW3 surfaces.
    /// </summary>
    public static class InstanceExtensions
    {
        /// <summary>
        /// Create a Surface object for a GLFW3 window.
        /// </summary>
        /// <param name="instance">
        /// The Vulkan instance to create the surface in.
        /// </param>
        /// <param name="windowHandle">
        /// The window to create the surface for.
        /// </param>
        /// <returns></returns>
        public unsafe static Surface CreateGlfw3Surface(this Instance instance, WindowHandle windowHandle)
        {
            Result result = Glfw3.CreateWindowSurface(instance.RawHandle, windowHandle, null, out ulong surfaceHandle);

            if (SharpVkException.IsError(result))
            {
                throw SharpVkException.Create(result);
            }

            return Surface.CreateFromHandle(instance, surfaceHandle);
        }

        /// <summary>
        /// Create a Surface object for a GLFW3 window.
        /// </summary>
        /// <param name="instance">
        /// The Vulkan instance to create the surface in.
        /// </param>
        /// <param name="window">
        /// The window to create the surface for.
        /// </param>
        /// <returns></returns>
        public unsafe static Surface CreateGlfw3Surface(this Instance instance, Window window)
        {
            Result result = Glfw3.CreateWindowSurface(instance.RawHandle, window.handle, null, out ulong surfaceHandle);

            if (SharpVkException.IsError(result))
            {
                throw SharpVkException.Create(result);
            }

            return Surface.CreateFromHandle(instance, surfaceHandle);
        }
    }
}
