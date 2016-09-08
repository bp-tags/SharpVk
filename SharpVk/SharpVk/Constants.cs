










//The MIT License (MIT)
//
//Copyright (c) Andrew Armstrong/FacticiusVir 2016
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.


using System.Linq;
using System.Reflection;

namespace SharpVk
{

    /// <summary>
    /// -
    /// </summary>
	public static class KhrSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 25;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrSwapchain
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 68;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_swapchain";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrDisplay
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 21;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_display";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrDisplaySwapchain
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 9;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_display_swapchain";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrXlibSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 6;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_xlib_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrXcbSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 6;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_xcb_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrWaylandSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 5;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_wayland_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrMirSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 4;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_mir_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrAndroidSurface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 6;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_android_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrWin32Surface
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 5;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_win32_surface";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class ExtDebugReport
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 4;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_EXT_debug_report";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvGlslShader
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_glsl_shader";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class KhrSamplerMirrorClampToEdge
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_KHR_sampler_mirror_clamp_to_edge";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class ImgFilterCubic
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_IMG_filter_cubic";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class AmdRasterizationOrder
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_AMD_rasterization_order";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class AmdShaderTrinaryMinmax
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_AMD_shader_trinary_minmax";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class AmdShaderExplicitVertexParameter
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_AMD_shader_explicit_vertex_parameter";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class ExtDebugMarker
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 3;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_EXT_debug_marker";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class AmdGcnShader
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_AMD_gcn_shader";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvDedicatedAllocation
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_dedicated_allocation";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class AmdDrawIndirectCount
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint ExtensionDrawIndirectCountSpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionDrawIndirectCountExtensionName = "VK_AMD_draw_indirect_count";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class ImgFormatPvrtc
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_IMG_format_pvrtc";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvExternalMemoryCapabilities
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_external_memory_capabilities";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvExternalMemory
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_external_memory";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvExternalMemoryWin32
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_external_memory_win32";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class NvWin32KeyedMutex
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SpecVersion = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const string ExtensionName = "VK_NV_win32_keyed_mutex";

	}

    /// <summary>
    /// -
    /// </summary>
	public static class Constants
	{

	    /// <summary>
	    /// -
	    /// </summary>
		public const float LodClampNone = 1000.0f;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint RemainingMipLevels = ~0U;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint RemainingArrayLayers = ~0U;

	    /// <summary>
	    /// -
	    /// </summary>
		public const ulong WholeSize = ~0UL;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint AttachmentUnused = ~0U;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint True = 1;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint False = 0;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint QueueFamilyIgnored = ~0U;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint SubpassExternal = ~0U;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint MaxExtensionNameSize = 256;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint MaxDescriptionSize = 256;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint MaxMemoryTypes = 32;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint MaxMemoryHeaps = 16;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint MaxPhysicalDeviceNameSize = 256;

	    /// <summary>
	    /// -
	    /// </summary>
		public const uint UuidSize = 16;

	    /// <summary>
	    /// -
	    /// </summary>
		public static readonly Version ApiVersion10 = new Version(1, 0, 0);

        /// <summary>
        /// The version of the SharpVk bindings.
        /// </summary>
        public static readonly Version SharpVkVersion = ExtractBindingVersion();

        private static Version ExtractBindingVersion()
        {
            return new Version(0, 2, 4);
        }

	}

}