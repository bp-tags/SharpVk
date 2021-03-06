// The MIT License (MIT)
// 
// Copyright (c) Andrew Armstrong/FacticiusVir 2018
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// This file was automatically generated and should not be edited directly.

using System;
using System.Runtime.InteropServices;

namespace SharpVk.Khronos.Experimental
{
    /// <summary>
    /// Structure specifying swapchain image memory to bind to.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct BindImageMemorySwapchainInfo
    {
        /// <summary>
        /// Null or a swapchain handle.
        /// </summary>
        public SharpVk.Khronos.Swapchain Swapchain
        {
            get;
            set;
        }
        
        /// <summary>
        /// An image index within swapchain.
        /// </summary>
        public uint ImageIndex
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        internal unsafe void MarshalTo(SharpVk.Interop.Khronos.Experimental.BindImageMemorySwapchainInfo* pointer)
        {
            pointer->SType = StructureType.BindImageMemorySwapchainInfo;
            pointer->Next = null;
            pointer->Swapchain = this.Swapchain?.handle ?? default(SharpVk.Interop.Khronos.Swapchain);
            pointer->ImageIndex = this.ImageIndex;
        }
    }
}
