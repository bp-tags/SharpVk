using System.Runtime.InteropServices;

namespace SharpVk.Glfw
{
    /// <summary>
    /// The function signature for monitor configuration callback functions.
    /// </summary>
    /// <param name="monitor">
    /// The monitor that was connected or disconnected.
    /// </param>
    /// <param name="eventStatus">
    /// The event that was raised.
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MonitorEventDelegate(MonitorHandle monitor, MonitorEvent eventStatus);
}
