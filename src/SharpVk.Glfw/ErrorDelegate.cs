using System.Runtime.InteropServices;

namespace SharpVk.Glfw
{
    /// <summary>
    /// The function signature for error callbacks.
    /// </summary>
    /// <param name="error">An error code giving the general category of the error.</param>
    /// <param name="description">A string description of the error.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ErrorDelegate(ErrorCode error, [MarshalAs(UnmanagedType.LPStr)] string description);
}
