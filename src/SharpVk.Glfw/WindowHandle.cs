using System;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Opaque window handle.
    /// </summary>
    public struct WindowHandle
    {
        internal WindowHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        private IntPtr handle;

        /// <summary>
        /// Gets the underlying native pointer to the window object.
        /// </summary>
        public IntPtr RawHandle
        {
            get
            {
                return this.handle;
            }
        }
    }
}
