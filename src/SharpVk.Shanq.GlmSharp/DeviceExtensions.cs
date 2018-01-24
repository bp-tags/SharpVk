using System;
using System.Linq;

namespace SharpVk.Shanq.GlmSharp
{
    public static class DeviceExtensions
    {
        public static ShaderModule CreateVertexModule<TOutput>(this Device device, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = ShanqShader.DefaultModuleEntryPoint)
        {
            return ShanqShader.CreateVertexModule(device, VectorTypeLibrary.Instance, shaderFunction, entryPointName);
        }

        public static ShaderModule CreateFragmentModule<TOutput>(this Device device, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = ShanqShader.DefaultModuleEntryPoint)
        {
            return ShanqShader.CreateFragmentModule(device, VectorTypeLibrary.Instance, shaderFunction, entryPointName);
        }
    }
}
