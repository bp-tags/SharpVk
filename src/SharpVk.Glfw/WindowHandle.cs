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

        /// <summary>
        /// A read-only field that represents a WindowHandle that has been
        /// inititalised to zero.
        /// </summary>
        public static readonly WindowHandle Zero = new WindowHandle(IntPtr.Zero);
    }
}
