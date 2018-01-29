using System;
using System.Runtime.InteropServices;

namespace SharpVk.Glfw
{
    /// <summary>
    /// Wraps a pointer to a know struct instance.
    /// </summary>
    /// <typeparam name="T">
    /// The type of struct referenced by the wrapped pointer.
    /// </typeparam>
    public struct Pointer<T>
        where T : struct
    {
        internal Pointer(IntPtr pointer)
        {
            this.pointer = pointer;
        }

        private IntPtr pointer;

        /// <summary>
        /// Gets the value at the referenced memory location.
        /// </summary>
        public T Value => this.IsNull ? throw new NullReferenceException() : Marshal.PtrToStructure<T>(this.pointer);

        /// <summary>
        /// Gets a value indicating whether the pointer wrapped by this
        /// instance is null.
        /// </summary>
        public bool IsNull => this.pointer == IntPtr.Zero;

        /// <summary>
        /// The underlying pointer wrapped by this instance.
        /// </summary>
        public IntPtr RawPointer => this.pointer;
    }
}
