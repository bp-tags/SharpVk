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

        /// <summary>
        /// A read-only field that represents a MonitorHandle that has been
        /// inititalised to zero.
        /// </summary>
        public static readonly MonitorHandle Zero = new MonitorHandle(IntPtr.Zero);
    }
}
