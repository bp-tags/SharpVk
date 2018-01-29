using System;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Opaque monitor handle.
    /// </summary>
    public struct MonitorHandle
    {
        internal MonitorHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        private IntPtr handle;

        /// <summary>
        /// Gets the underlying native pointer to the monitor object.
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
