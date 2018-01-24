using Remotion.Linq.Parsing.Structure;
using SharpVk.Spirv;
using System;
using System.IO;
using System.Linq;

namespace SharpVk.Shanq
{
    public interface IShanqFactory
    {
        IQueryable<T> GetInput<T>();

        IQueryable<T> GetBinding<T>(int binding);

        IQueryable<Sampler2d<T, V>> GetSampler2d<T, V>(int binding, int descriptorSet);
    }

    internal class ShanqFactory
        : IShanqFactory
    {
        private readonly ShanqQueryExecutor executor;

        public ShanqFactory(ExecutionModel model, Stream outputStream, IVectorTypeLibrary vectorLibrary, string entryPointName)
        {
            this.executor = new ShanqQueryExecutor(model, outputStream, vectorLibrary, entryPointName);
        }

        public IQueryable<T> GetBinding<T>(int binding)
        {
            return new ShanqQueryable<T>(QueryableOrigin.Binding, QueryParser.CreateDefault(), this.executor, binding);
        }

        public IQueryable<T> GetInput<T>()
        {
            return new ShanqQueryable<T>(QueryableOrigin.Input, QueryParser.CreateDefault(), this.executor);
        }

        public IQueryable<Sampler2d<T, V>> GetSampler2d<T, V>(int binding, int descriptorSet)
        {
            return new ShanqQueryable<Sampler2d<T, V>>(QueryableOrigin.Sampler, QueryParser.CreateDefault(), this.executor, binding, descriptorSet);
        }
    }

    public static class ShanqShader
    {
        public const string DefaultModuleEntryPoint = "main";

        public static void Create<TOutput>(ExecutionModel model, Stream outputStream, IVectorTypeLibrary vectorLibrary, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            var factory = new ShanqFactory(model, outputStream, vectorLibrary, entryPointName);

            shaderFunction(factory).ToArray();
        }

        public static void CreateVertex<TOutput>(Stream outputStream, IVectorTypeLibrary vectorLibrary, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            Create(ExecutionModel.Vertex, outputStream, vectorLibrary, shaderFunction);
        }

        public static void CreateFragment<TOutput>(Stream outputStream, IVectorTypeLibrary vectorLibrary, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            Create(ExecutionModel.Fragment, outputStream, vectorLibrary, shaderFunction);
        }

        public static ShaderModule CreateVertexModule<TOutput>(Device device, IVectorTypeLibrary vectorLibrary, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            return CreateModule(device, vectorLibrary, ExecutionModel.Vertex, shaderFunction);
        }

        public static ShaderModule CreateFragmentModule<TOutput>(Device device, IVectorTypeLibrary vectorLibrary, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            return CreateModule(device, vectorLibrary, ExecutionModel.Fragment, shaderFunction);
        }

        private static ShaderModule CreateModule<TOutput>(Device device, IVectorTypeLibrary vectorLibrary, ExecutionModel model, Func<IShanqFactory, IQueryable<TOutput>> shaderFunction, string entryPointName = DefaultModuleEntryPoint)
        {
            var shaderStream = new MemoryStream();

            Create(model, shaderStream, vectorLibrary, shaderFunction);

            int shaderLength = (int)shaderStream.Length;

            var shaderBytes = shaderStream.GetBuffer();

            var shaderData = LoadShaderData(shaderBytes, shaderLength);

            return device.CreateShaderModule(shaderLength, shaderData);
        }

        private static uint[] LoadShaderData(byte[] shaderBytes, int codeSize)
        {
            var shaderData = new uint[(int)Math.Ceiling(codeSize / 4f)];

            System.Buffer.BlockCopy(shaderBytes, 0, shaderData, 0, codeSize);

            return shaderData;
        }
    }
}
