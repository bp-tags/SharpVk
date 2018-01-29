using System;
using System.Runtime.InteropServices;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Interop functions for the GLFW3 API.
    /// </summary>
    public unsafe static class Glfw3
    {
        /// <summary>
        /// The base name for the GLFW3 library.
        /// </summary>
        public const string GlfwDll = "glfw3";

        /// <summary>
        /// Initializes the GLFW library.
        /// </summary>
        /// <returns>
        /// True if successful, otherwise false.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwInit")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Init();

        /// <summary>
        /// This function destroys all remaining windows and cursors, restores
        /// any modified gamma ramps and frees any other allocated resources.
        /// </summary>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwTerminate")]
        public static extern void Terminate();

        /// <summary>
        /// This function retrieves the major, minor and revision numbers of
        /// the GLFW library.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="rev">The revision number.</param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVersion")]
        public static extern void GetVersion(out int major, out int minor, out int rev);

        /// <summary>
        /// Creates a window and its associated OpenGL or OpenGL ES context.
        /// Most of the options controlling how the window and its context
        /// should be created are specified with window hints.
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
        /// <param name="monitor">
        /// The monitor to use for full screen mode, or Null for windowed mode.
        /// </param>
        /// <param name="share">
        /// The window whose context to share resources with, or Null to not share resources.
        /// </param>
        /// <returns>
        /// The handle of the created window, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateWindow")]
        public static extern WindowHandle CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, IntPtr monitor, IntPtr share);

        /// <summary>
        /// Processes events in the event queue and then returns immediately.
        /// Processing events will cause the window and input callbacks
        /// associated with those events to be called.
        /// </summary>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwPollEvents")]
        public static extern void PollEvents();

        /// <summary>
        /// Sets hints for the next call to CreateWindow. The hints, once set,
        /// retain their values until changed by a call to WindowHint or
        /// DefaultWindowHints, or until the library is terminated.
        /// </summary>
        /// <param name="hint">
        /// The window hint to set.
        /// </param>
        /// <param name="value">
        /// The new value of the window hint.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowHint")]
        public static extern void WindowHint(WindowAttribute hint, int value);

        /// <summary>
        /// Returns the value of the close flag of the specified window.
        /// </summary>
        /// <param name="window">
        /// The window to query.
        /// </param>
        /// <returns>
        /// The value of the close flag.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwWindowShouldClose")]
        public static extern bool WindowShouldClose(WindowHandle window);

        /// <summary>
        /// Creates a Vulkan surface for the specified window.
        /// </summary>
        /// <param name="instance">
        /// The Vulkan instance to create the surface in.
        /// </param>
        /// <param name="window">
        /// The window to create the surface for.
        /// </param>
        /// <param name="pAllocator">
        /// The allocator to use, or NULL to use the default allocator.
        /// </param>
        /// <param name="surface">
        /// Where to store the handle of the surface. This is set to
        /// VK_NULL_HANDLE if an error occurred.
        /// </param>
        /// <returns>
        /// Result.Success if successful, or a Vulkan error code if an error
        /// occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwCreateWindowSurface")]
        public static extern Result CreateWindowSurface(Interop.Instance instance, WindowHandle window, Interop.AllocationCallbacks* pAllocator, out ulong surface);

        /// <summary>
        /// Returns an array of names of Vulkan instance extensions required by
        /// GLFW for creating Vulkan surfaces for GLFW windows. If successful,
        /// the list will always contains VK_KHR_surface, so if you don't
        /// require any additional extensions you can pass this list directly
        /// to the InstanceCreateInfo struct.
        /// </summary>
        /// <param name="count">
        /// Where to store the number of extensions in the returned array. This
        /// is set to zero if an error occurred.
        /// </param>
        /// <returns>
        /// An array of extension names, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetRequiredInstanceExtensions")]
        public static extern byte** GetRequiredInstanceExtensions(out int count);

        /// <summary>
        /// Sets the size callback of the specified window, which is called
        /// when the window is resized. The callback is provided with the size,
        /// in screen coordinates, of the client area of the window.
        /// </summary>
        /// <param name="window">
        /// The window whose callback to set.
        /// </param>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns></returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetWindowSizeCallback")]
        public static extern WindowSizeDelegate SetWindowSizeCallback(WindowHandle window, WindowSizeDelegate callback);

        /// <summary>
        /// Sets the error callback, which is called with an error code and a
        /// human-readable description each time a GLFW error occurs.
        /// </summary>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns>
        /// The previously set callback, or Null if no callback was set.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetErrorCallback")]
        public static extern ErrorDelegate SetErrorCallback(ErrorDelegate callback);

        /// <summary>
        /// Returns an array of handles for all currently connected monitors.
        /// The primary monitor is always first in the returned array. If no
        /// monitors were found, this function returns Null.
        /// </summary>
        /// <param name="count">
        /// Where to store the number of monitors in the returned array. This
        /// is set to zero if an error occurred.
        /// </param>
        /// <returns>
        /// An array of monitor handles, or Null if no monitors were found or
        /// if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitors")]
        public static extern MonitorHandle* GetMonitors(out int count);

        /// <summary>
        /// Returns the primary monitor. This is usually the monitor where
        /// elements like the task bar or global menu bar are located.
        /// </summary>
        /// <returns>
        /// The primary monitor, or Null if no monitors were found or if an
        /// error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetPrimaryMonitor")]
        public static extern MonitorHandle GetPrimaryMonitor();

        /// <summary>
        /// Returns the position, in screen coordinates, of the upper-left
        /// corner of the specified monitor.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="xPos">
        /// Returns the monitor x-coordinate.
        /// </param>
        /// <param name="yPos">
        /// Returns the monitor y-coordinate.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPos")]
        public static extern void GetMonitorPos(MonitorHandle monitor, out int xPos, out int yPos);

        /// <summary>
        /// Returns the size, in millimetres, of the display area of the
        /// specified monitor.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="widthMm">
        /// The width, in millimetres, of the monitor's display area.
        /// </param>
        /// <param name="heightMm">
        /// The width, in millimetres, of the monitor's display area.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorPhysicalSize")]
        public static extern void GetMonitorPhysicalSize(MonitorHandle monitor, out int widthMm, out int heightMm);

        /// <summary>
        /// Returns a human-readable name, of the specified monitor. The name
        /// typically reflects the make and model of the monitor and is not
        /// guaranteed to be unique among the connected monitors.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// The name of the monitor, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetMonitorName")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string GetMonitorName(MonitorHandle monitor);

        /// <summary>
        /// Sets the monitor configuration callback, or removes the currently
        /// set callback. This is called when a monitor is connected to or
        /// disconnected from the system.
        /// </summary>
        /// <param name="callback">
        /// The new callback, or Null to remove the currently set callback.
        /// </param>
        /// <returns>
        /// The previously set callback, or NULL if no callback was set or the
        /// library had not been initialized.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetMonitorCallback")]
        public static extern MonitorEventDelegate SetMonitorCallback(MonitorEventDelegate callback);

        /// <summary>
        /// Returns an array of all video modes supported by the specified
        /// monitor. The returned array is sorted in ascending order, first by
        /// color bit depth (the sum of all channel depths) and then by
        /// resolution area (the product of width and height).
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <param name="count">
        /// Tthe number of video modes in the returned array. This is set to
        /// zero if an error occurred.
        /// </param>
        /// <returns>
        /// An array of video modes, or Null if an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVideoModes")]
        public static extern VideoMode* GetVideoModes(MonitorHandle monitor, out int count);

        /// <summary>
        /// Returns the current video mode of the specified monitor. If you
        /// have created a full screen window for that monitor, the return
        /// value will depend on whether that window is iconified.
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// A wrapped pointer to the current mode of the monitor, or Null if
        /// an error occurred.
        /// </returns>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwGetVideoMode")]
        public static extern Pointer<VideoMode> GetVideoMode(MonitorHandle monitor);

        /// <summary>
        /// Generates a 256-element gamma ramp from the specified exponent and
        /// then calls glfwSetGammaRamp with it. The value must be a finite
        /// number greater than zero.
        /// </summary>
        /// <param name="monitor">
        /// The monitor whose gamma ramp to set.
        /// </param>
        /// <param name="gamma">
        /// The desired exponent.
        /// </param>
        [DllImport(GlfwDll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "glfwSetGamma")]
        public static extern void SetGamma(MonitorHandle monitor, float gamma);

        /// <summary>
        /// Returns an array of names of Vulkan instance extensions required by
        /// GLFW for creating Vulkan surfaces for GLFW windows. If successful,
        /// the list will always contains VK_KHR_surface, so if you don't
        /// require any additional extensions you can pass this list directly
        /// to the InstanceCreateInfo struct.
        /// </summary>
        /// <returns>
        /// An array of extension names, or Null if an error occurred.
        /// </returns>
        public static string[] GetRequiredInstanceExtensions()
        {
            byte** namePointer = GetRequiredInstanceExtensions(out int count);

            var result = new string[count];

            for (int nameIndex = 0; nameIndex < count; nameIndex++)
            {
                result[nameIndex] = Marshal.PtrToStringAnsi(new IntPtr(namePointer[nameIndex]));
            }

            return result;
        }

        /// <summary>
        /// Returns an array of handles for all currently connected monitors.
        /// The primary monitor is always first in the returned array. If no
        /// monitors were found, this function returns Null.
        /// </summary>
        /// <returns>
        /// An array of monitor handles, or Null if no monitors were found or
        /// if an error occurred.
        /// </returns>
        public static MonitorHandle[] GetMonitors()
        {
            MonitorHandle* monitorPointer = GetMonitors(out int count);

            var result = new MonitorHandle[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = monitorPointer[i];
            }

            return result;
        }

        /// <summary>
        /// Returns an array of all video modes supported by the specified
        /// monitor. The returned array is sorted in ascending order, first by
        /// color bit depth (the sum of all channel depths) and then by
        /// resolution area (the product of width and height).
        /// </summary>
        /// <param name="monitor">
        /// The monitor to query.
        /// </param>
        /// <returns>
        /// An array of video modes, or Null if an error occurred.
        /// </returns>
        public static VideoMode[] GetVideoModes(MonitorHandle monitor)
        {
            VideoMode* videoModePointer = GetVideoModes(monitor, out int count);

            var result = new VideoMode[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = videoModePointer[i];
            }

            return result;
        }

        /// <summary>
        /// This function retrieves the version number of the GLFW library.
        /// </summary>
        /// <returns>
        /// The version number of the GLFW library.
        /// </returns>
        public static Version GetVersion()
        {
            GetVersion(out int major, out int minor, out int revision);

            return new Version(major, minor, revision);
        }
    }
}
