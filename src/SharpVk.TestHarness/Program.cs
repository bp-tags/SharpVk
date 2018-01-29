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

using GlmSharp;
using SharpVk.Glfw;
using SharpVk.Khronos;
using SharpVk.Multivendor;
using SharpVk.Shanq;
using SharpVk.Shanq.GlmSharp;
using SharpVk.Spirv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpVk.VertexBuffers
{
    public class Program
    {
        private const int SurfaceWidth = 1920;
        private const int SurfaceHeight = 1080;

        private readonly Vertex[] vertices =
        {
            new Vertex(new vec2(0, 0), new vec2(0, 0)),
            new Vertex(new vec2(1, 0), new vec2(1, 0)),
            new Vertex(new vec2(1, 1), new vec2(1, 1)),
            new Vertex(new vec2(0, 1), new vec2(0, 1))
        };

        private readonly ushort[] indices = { 0, 1, 2, 2, 3, 0 };

        private WindowHandle window;
        private Instance instance;
        private Surface surface;
        private PhysicalDevice physicalDevice;
        private Device device;
        private Queue graphicsQueue;
        private Queue presentQueue;
        private Queue transferQueue;
        private Swapchain swapChain;
        private Image[] swapChainImages;
        private ImageView[] swapChainImageViews;
        private RenderPass renderPass;
        private PipelineLayout pipelineLayout;
        private Pipeline pipeline;
        private DescriptorSet descriptorSet;
        private ShaderModule vertShader;
        private ShaderModule fragShader;
        private Framebuffer[] frameBuffers;
        private CommandPool transientCommandPool;
        private CommandPool commandPool;
        private Image stagingImage;
        private DeviceMemory stagingImageMemory;
        private Image textureImage;
        private DeviceMemory textureImageMemory;
        private ImageView textureImageView;
        private Sampler textureSampler;
        private Buffer vertexBuffer;
        private DeviceMemory vertexBufferMemory;
        private Buffer indexBuffer;
        private DeviceMemory indexBufferMemory;
        private CommandBuffer[] commandBuffers;
        private Semaphore imageAvailableSemaphore;
        private Semaphore renderFinishedSemaphore;

        private Format swapChainFormat;
        private Extent2D swapChainExtent;
        private DescriptorPool descriptorPool;
        private DescriptorSetLayout descriptorSetLayout;

        public static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            this.InitialiseWindow();
            this.InitialiseVulkan();
            this.MainLoop();
            this.TearDown();
        }

        private void InitialiseWindow()
        {
            Glfw3.Init();


            Glfw3.WindowHint(WindowAttribute.ClientApi, 0);
            this.window = Glfw3.CreateWindow(SurfaceWidth, SurfaceHeight, "Vulkan", IntPtr.Zero, IntPtr.Zero);

            Glfw3.SetWindowSizeCallback(this.window, (x, y, z) => this.RecreateSwapChain());
        }

        private void InitialiseVulkan()
        {
            this.CreateInstance();
            this.CreateSurface();
            this.PickPhysicalDevice();
            this.CreateLogicalDevice();
            this.CreateSwapChain();
            this.CreateImageViews();
            this.CreateCommandPools();
            this.CreateTextureImage();
            this.CreateTextureImageView();
            this.CreateTextureSampler();
            this.CreateDescriptorPool();
            this.CreateRenderPass();
            this.CreateShaderModules();
            this.CreateGraphicsPipeline();
            this.CreateFrameBuffers();
            this.CreateVertexBuffers();
            this.CreateIndexBuffer();
            this.CreateCommandBuffers();
            this.CreateSemaphores();
        }

        private void MainLoop()
        {
            while (!Glfw3.WindowShouldClose(this.window))
            {
                this.DrawFrame();

                Glfw3.PollEvents();
            }
        }

        private void RecreateSwapChain()
        {
            this.device.WaitIdle();

            this.commandPool.FreeCommandBuffers(commandBuffers);

            foreach (var frameBuffer in this.frameBuffers)
            {
                frameBuffer.Dispose();
            }
            this.frameBuffers = null;

            this.pipeline.Dispose();
            this.pipeline = null;

            this.pipelineLayout.Dispose();
            this.pipelineLayout = null;

            foreach (var imageView in this.swapChainImageViews)
            {
                imageView.Dispose();
            }
            this.swapChainImageViews = null;

            this.renderPass.Dispose();
            this.renderPass = null;

            this.swapChain.Dispose();
            this.swapChain = null;

            this.CreateSwapChain();
            this.CreateImageViews();
            this.CreateRenderPass();
            this.CreateGraphicsPipeline();
            this.CreateFrameBuffers();
            this.CreateCommandBuffers();
        }

        private void TearDown()
        {
            device.WaitIdle();

            this.renderFinishedSemaphore.Dispose();
            this.renderFinishedSemaphore = null;

            this.imageAvailableSemaphore.Dispose();
            this.imageAvailableSemaphore = null;

            this.vertexBufferMemory.Free();
            this.vertexBufferMemory = null;

            this.vertexBuffer.Dispose();
            this.vertexBuffer = null;

            this.commandPool.Dispose();
            this.commandPool = null;

            foreach (var frameBuffer in this.frameBuffers)
            {
                frameBuffer.Dispose();
            }
            this.frameBuffers = null;

            this.fragShader.Dispose();
            this.fragShader = null;

            this.vertShader.Dispose();
            this.vertShader = null;

            this.pipeline.Dispose();
            this.pipeline = null;

            this.pipelineLayout.Dispose();
            this.pipelineLayout = null;

            foreach (var imageView in this.swapChainImageViews)
            {
                imageView.Dispose();
            }
            this.swapChainImageViews = null;

            this.renderPass.Dispose();
            this.renderPass = null;

            this.swapChain.Dispose();
            this.swapChain = null;

            this.device.Dispose();
            this.device = null;

            this.surface.Dispose();
            this.surface = null;

            this.instance.Dispose();
            this.instance = null;
        }

        private void DrawFrame()
        {
            uint nextImage = this.swapChain.AcquireNextImage(uint.MaxValue, this.imageAvailableSemaphore, null);

            this.graphicsQueue.Submit(
                new SubmitInfo
                {
                    CommandBuffers = new[] { this.commandBuffers[nextImage] },
                    SignalSemaphores = new[] { this.renderFinishedSemaphore },
                    WaitDestinationStageMask = new[] { PipelineStageFlags.ColorAttachmentOutput },
                    WaitSemaphores = new[] { this.imageAvailableSemaphore }
                }, null);

            this.presentQueue.Present(this.renderFinishedSemaphore, this.swapChain, nextImage, new Result[1]);
        }

        private void CreateInstance()
        {
            var enabledLayers = new List<string>();

            if (Instance.EnumerateLayerProperties().Any(x => x.LayerName == "VK_LAYER_LUNARG_standard_validation"))
            {
                enabledLayers.Add("VK_LAYER_LUNARG_standard_validation");
            }

            var glfwExtensions = Glfw3.GetRequiredInstanceExtensions();

            this.instance = Instance.Create(
                enabledLayers.ToArray(),
                glfwExtensions.Append(ExtExtensions.DebugReport).ToArray(),
                applicationInfo: new ApplicationInfo
                {
                    ApplicationName = "Vertex Buffers",
                    ApplicationVersion = new Version(1, 0, 0),
                    EngineName = "SharpVk",
                    EngineVersion = new Version(0, 4, 0),
                    ApiVersion = new Version(1, 0, 0)
                });

            instance.CreateDebugReportCallback(DebugReportDelegate, DebugReportFlags.Error | DebugReportFlags.Warning | DebugReportFlags.Information | DebugReportFlags.Debug);
        }

        private static readonly DebugReportCallbackDelegate DebugReportDelegate = DebugReport;

        private static Bool32 DebugReport(DebugReportFlags flags, DebugReportObjectType objectType, ulong @object, HostSize location, int messageCode, string layerPrefix, string message, IntPtr userData)
        {
            Debug.WriteLine(message);

            return false;
        }

        private void CreateSurface()
        {
            this.surface = this.instance.CreateGlfw3Surface(this.window);
        }

        private void PickPhysicalDevice()
        {
            var availableDevices = this.instance.EnumeratePhysicalDevices();

            this.physicalDevice = availableDevices.First(IsSuitableDevice);
        }

        private void CreateLogicalDevice()
        {
            QueueFamilyIndices queueFamilies = FindQueueFamilies(this.physicalDevice);

            this.device = physicalDevice.CreateDevice(queueFamilies.Indices
                                                                        .Select(index => new DeviceQueueCreateInfo
                                                                        {
                                                                            QueueFamilyIndex = index,
                                                                            QueuePriorities = new[] { 1f }
                                                                        }).ToArray(),
                                                        null,
                                                        KhrExtensions.Swapchain);

            this.graphicsQueue = this.device.GetQueue(queueFamilies.GraphicsFamily.Value, 0);
            this.presentQueue = this.device.GetQueue(queueFamilies.PresentFamily.Value, 0);
            this.transferQueue = this.device.GetQueue(queueFamilies.TransferFamily.Value, 0);
        }

        private void CreateSwapChain()
        {
            SwapChainSupportDetails swapChainSupport = this.QuerySwapChainSupport(this.physicalDevice);

            uint imageCount = swapChainSupport.Capabilities.MinImageCount + 1;
            if (swapChainSupport.Capabilities.MaxImageCount > 0 && imageCount > swapChainSupport.Capabilities.MaxImageCount)
            {
                imageCount = swapChainSupport.Capabilities.MaxImageCount;
            }

            SurfaceFormat surfaceFormat = this.ChooseSwapSurfaceFormat(swapChainSupport.Formats);

            QueueFamilyIndices queueFamilies = this.FindQueueFamilies(this.physicalDevice);

            var indices = queueFamilies.Indices.ToArray();

            Extent2D extent = this.ChooseSwapExtent(swapChainSupport.Capabilities);

            this.swapChain = device.CreateSwapchain(surface,
                                                    imageCount,
                                                    surfaceFormat.Format,
                                                    surfaceFormat.ColorSpace,
                                                    extent,
                                                    1,
                                                    ImageUsageFlags.ColorAttachment,
                                                    indices.Length == 1
                                                        ? SharingMode.Exclusive
                                                        : SharingMode.Concurrent,
                                                    indices,
                                                    swapChainSupport.Capabilities.CurrentTransform,
                                                    CompositeAlphaFlags.Opaque,
                                                    this.ChooseSwapPresentMode(swapChainSupport.PresentModes),
                                                    true,
                                                    this.swapChain);

            this.swapChainImages = this.swapChain.GetImages();
            this.swapChainFormat = surfaceFormat.Format;
            this.swapChainExtent = extent;
        }

        private void CreateImageViews()
        {
            this.swapChainImageViews = this.swapChainImages
                                                .Select(image => device.CreateImageView(image,
                                                                                        ImageViewType.ImageView2d,
                                                                                        this.swapChainFormat,
                                                                                        ComponentMapping.Identity,
                                                                                        new ImageSubresourceRange
                                                                                        {
                                                                                            AspectMask = ImageAspectFlags.Color,
                                                                                            BaseMipLevel = 0,
                                                                                            LevelCount = 1,
                                                                                            BaseArrayLayer = 0,
                                                                                            LayerCount = 1
                                                                                        }))
                                                .ToArray();
        }

        private unsafe void CreateTextureImage()
        {
            uint textureWidth = 1024;
            uint textureHeight = 1024;
            DeviceSize imageSize = textureWidth * textureHeight * 4;

            byte[] rgbaBytes = new byte[imageSize];

            int i = 0;

            for (var y = 0; y < textureHeight; y++)
            {
                for (var x = 0; x < textureWidth; x++)
                {
                    rgbaBytes[i++] = (byte)(x % 256);
                    rgbaBytes[i++] = 0;
                    rgbaBytes[i++] = (byte)(y % 256);
                    rgbaBytes[i++] = 255;
                }
            }

            this.CreateImage(textureWidth, textureHeight, Format.R8G8B8A8UNorm, ImageTiling.Linear, ImageUsageFlags.TransferSource, MemoryPropertyFlags.HostVisible | MemoryPropertyFlags.HostCoherent, out this.stagingImage, out this.stagingImageMemory);

            IntPtr memoryBuffer = this.stagingImageMemory.Map(0, imageSize);

            fixed (byte* rgbaPointer = rgbaBytes)
            {
                System.Buffer.MemoryCopy(rgbaPointer, memoryBuffer.ToPointer(), imageSize, imageSize);
            }

            this.stagingImageMemory.Unmap();

            this.CreateImage(textureWidth, textureHeight, Format.R8G8B8A8UNorm, ImageTiling.Optimal, ImageUsageFlags.TransferDestination | ImageUsageFlags.Sampled, MemoryPropertyFlags.DeviceLocal, out this.textureImage, out this.textureImageMemory);

            this.TransitionImageLayout(this.stagingImage, Format.R8G8B8A8UNorm, ImageLayout.Preinitialized, ImageLayout.TransferSourceOptimal);
            this.TransitionImageLayout(this.textureImage, Format.R8G8B8A8UNorm, ImageLayout.Preinitialized, ImageLayout.TransferDestinationOptimal);

            this.CopyImage(this.stagingImage, this.textureImage, textureWidth, textureHeight);
            this.TransitionImageLayout(this.textureImage, Format.R8G8B8A8UNorm, ImageLayout.TransferDestinationOptimal, ImageLayout.ShaderReadOnlyOptimal);
        }

        private void CreateTextureImageView()
        {
            this.textureImageView = this.CreateImageView(this.textureImage, Format.R8G8B8A8UNorm);
        }

        private void CreateTextureSampler()
        {
            this.textureSampler = this.device.CreateSampler(Filter.Nearest,
                                                            Filter.Nearest,
                                                            SamplerMipmapMode.Nearest,
                                                            SamplerAddressMode.ClampToBorder,
                                                            SamplerAddressMode.ClampToBorder,
                                                            SamplerAddressMode.ClampToBorder,
                                                            0,
                                                            false,
                                                            1,
                                                            false,
                                                            CompareOp.Always,
                                                            0,
                                                            0,
                                                            BorderColor.FloatTransparentBlack,
                                                            false);
        }

        private void CreateDescriptorPool()
        {
            this.descriptorPool = device.CreateDescriptorPool(2,
                new[]
                {
                    new DescriptorPoolSize
                    {
                        DescriptorCount = 2,
                        Type = DescriptorType.UniformBuffer
                    },
                    new DescriptorPoolSize
                    {
                        DescriptorCount = 2,
                        Type = DescriptorType.CombinedImageSampler
                    }
                });
        }

        private void CreateRenderPass()
        {
            this.renderPass = device.CreateRenderPass(
                                                    new AttachmentDescription
                                                    {
                                                        Format = this.swapChainFormat,
                                                        Samples = SampleCountFlags.SampleCount1,
                                                        LoadOp = AttachmentLoadOp.Clear,
                                                        StoreOp = AttachmentStoreOp.Store,
                                                        StencilLoadOp = AttachmentLoadOp.DontCare,
                                                        StencilStoreOp = AttachmentStoreOp.DontCare,
                                                        InitialLayout = ImageLayout.Undefined,
                                                        FinalLayout = ImageLayout.PresentSource
                                                    },
                                                    new SubpassDescription
                                                    {
                                                        DepthStencilAttachment = new AttachmentReference(Constants.AttachmentUnused, ImageLayout.Undefined),
                                                        PipelineBindPoint = PipelineBindPoint.Graphics,
                                                        ColorAttachments = new[]
                                                        {
                                                            new AttachmentReference(0, ImageLayout.ColorAttachmentOptimal)
                                                        }
                                                    },
                                                    new[]
                                                    {
                                                        new SubpassDependency
                                                        {
                                                            SourceSubpass = Constants.SubpassExternal,
                                                            DestinationSubpass = 0,
                                                            SourceStageMask = PipelineStageFlags.BottomOfPipe,
                                                            SourceAccessMask = AccessFlags.MemoryRead,
                                                            DestinationStageMask = PipelineStageFlags.ColorAttachmentOutput,
                                                            DestinationAccessMask = AccessFlags.ColorAttachmentRead | AccessFlags.ColorAttachmentWrite
                                                        },
                                                        new SubpassDependency
                                                        {
                                                            SourceSubpass = 0,
                                                            DestinationSubpass = Constants.SubpassExternal,
                                                            SourceStageMask = PipelineStageFlags.ColorAttachmentOutput,
                                                            SourceAccessMask = AccessFlags.ColorAttachmentRead | AccessFlags.ColorAttachmentWrite,
                                                            DestinationStageMask = PipelineStageFlags.BottomOfPipe,
                                                            DestinationAccessMask = AccessFlags.MemoryRead
                                                        }
                                                    });
        }
        
        private void CreateShaderModules()
        {
            this.vertShader = this.device.CreateVertexModule(shanq => from input in shanq.GetInput<Vertex>()
                                                                      select new VertexOutput
                                                                      {
                                                                          Uv = input.Uv,
                                                                          Position = new vec4(input.Position, (float)input.Test, 1)
                                                                      });

            this.fragShader = this.device.CreateFragmentModule(shanq => from input in shanq.GetInput<FragmentInput>()
                                                                        from texture in shanq.GetSampler2d<vec4>(1, 0)
                                                                        select new FragmentOutput
                                                                        {
                                                                            Colour = texture.Sample(input.Uv),
                                                                            FragDepth = input.FragCoord.z
                                                                        });
        }

        public struct SpriteData
        {
            public mat4 Transform;
            public vec2 AtlasPosition;
            public vec2 Size;
        };

        private void CreateGraphicsPipeline()
        {
            this.descriptorSetLayout = this.device.CreateDescriptorSetLayout(new[]
                {
                    new DescriptorSetLayoutBinding
                    {
                        Binding = 1,
                        DescriptorCount = 1,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        StageFlags = ShaderStageFlags.Fragment
                    }
                });

            var bindingDescription = Vertex.GetBindingDescription();
            var attributeDescriptions = Vertex.GetAttributeDescriptions();

            this.pipelineLayout = device.CreatePipelineLayout(this.descriptorSetLayout, null);

            this.pipeline = device.CreateGraphicsPipelines(null, new[]
            {
                    new GraphicsPipelineCreateInfo
                    {
                        Layout = this.pipelineLayout,
                        RenderPass = this.renderPass,
                        Subpass = 0,
                        VertexInputState = new PipelineVertexInputStateCreateInfo()
                        {
                            VertexBindingDescriptions = new [] { bindingDescription },
                            VertexAttributeDescriptions = attributeDescriptions
                        },
                        InputAssemblyState = new PipelineInputAssemblyStateCreateInfo
                        {
                            PrimitiveRestartEnable = false,
                            Topology = PrimitiveTopology.TriangleList
                        },
                        ViewportState = new PipelineViewportStateCreateInfo
                        {
                            Viewports = new[]
                            {
                                new Viewport(0f, 0f, this.swapChainExtent.Width, this.swapChainExtent.Height, 0f, 1f)
                            },
                            Scissors = new[]
                            {
                                new Rect2D(this.swapChainExtent)
                            }
                        },
                        RasterizationState = new PipelineRasterizationStateCreateInfo
                        {
                            DepthClampEnable = false,
                            RasterizerDiscardEnable = false,
                            PolygonMode = PolygonMode.Fill,
                            LineWidth = 1,
                            CullMode = CullModeFlags.Back,
                            FrontFace = FrontFace.Clockwise,
                            DepthBiasEnable = false
                        },
                        MultisampleState = new PipelineMultisampleStateCreateInfo
                        {
                            SampleShadingEnable = false,
                            RasterizationSamples = SampleCountFlags.SampleCount1,
                            MinSampleShading = 1
                        },
                        ColorBlendState = new PipelineColorBlendStateCreateInfo
                        {
                            Attachments = new[]
                            {
                                new PipelineColorBlendAttachmentState
                                {
                                    ColorWriteMask = ColorComponentFlags.R
                                                        | ColorComponentFlags.G
                                                        | ColorComponentFlags.B
                                                        | ColorComponentFlags.A,
                                    BlendEnable = false,
                                    SourceColorBlendFactor = BlendFactor.One,
                                    DestinationColorBlendFactor = BlendFactor.Zero,
                                    ColorBlendOp = BlendOp.Add,
                                    SourceAlphaBlendFactor = BlendFactor.One,
                                    DestinationAlphaBlendFactor = BlendFactor.Zero,
                                    AlphaBlendOp = BlendOp.Add
                                }
                            },
                            LogicOpEnable = false,
                            LogicOp = LogicOp.Copy,
                            BlendConstants = (0, 0, 0, 0)
                        },
                        Stages = new[]
                        {
                            new PipelineShaderStageCreateInfo
                            {
                                Stage = ShaderStageFlags.Vertex,
                                Module = this.vertShader,
                                Name = "main"
                            },
                            new PipelineShaderStageCreateInfo
                            {
                                Stage = ShaderStageFlags.Fragment,
                                Module = this.fragShader,
                                Name = "main"
                            }
                        }
                    }
                }).Single();

            this.descriptorSet = this.device.AllocateDescriptorSet(descriptorPool, this.descriptorSetLayout);

            this.device.UpdateDescriptorSets(new[]
            {
                new WriteDescriptorSet
                {
                    ImageInfo = new []
                    {
                        new DescriptorImageInfo
                        {
                            ImageView = textureImageView,
                            Sampler = textureSampler,
                            ImageLayout = ImageLayout.ShaderReadOnlyOptimal
                        }
                    },
                    DescriptorCount = 1,
                    DestinationSet = descriptorSet,
                    DestinationBinding = 1,
                    DestinationArrayElement = 0,
                    DescriptorType = DescriptorType.CombinedImageSampler
                }
            }, null);
        }

        private void CreateFrameBuffers()
        {
            Framebuffer Create(ImageView imageView) => device.CreateFramebuffer(renderPass,
                                                                                imageView,
                                                                                this.swapChainExtent.Width,
                                                                                this.swapChainExtent.Height,
                                                                                1);

            this.frameBuffers = this.swapChainImageViews.Select(Create).ToArray();
        }

        private void CreateCommandPools()
        {
            QueueFamilyIndices queueFamilies = FindQueueFamilies(this.physicalDevice);

            this.transientCommandPool = device.CreateCommandPool(queueFamilies.TransferFamily.Value, CommandPoolCreateFlags.Transient);

            this.commandPool = device.CreateCommandPool(queueFamilies.GraphicsFamily.Value);
        }

        private void CreateVertexBuffers()
        {
            int vertexSize = Unsafe.SizeOf<Vertex>();

            uint bufferSize = (uint)(vertexSize * vertices.Length);

            this.CreateBuffer(bufferSize, BufferUsageFlags.TransferSource, MemoryPropertyFlags.HostVisible | MemoryPropertyFlags.HostCoherent, out var stagingBuffer, out var stagingBufferMemory);

            IntPtr memoryBuffer = stagingBufferMemory.Map(0, bufferSize, MemoryMapFlags.None);

            for (int index = 0; index < vertices.Length; index++)
            {
                Marshal.StructureToPtr(vertices[index], memoryBuffer + (vertexSize * index), false);
            }

            stagingBufferMemory.Unmap();

            this.CreateBuffer(bufferSize, BufferUsageFlags.TransferDestination | BufferUsageFlags.VertexBuffer, MemoryPropertyFlags.DeviceLocal, out this.vertexBuffer, out this.vertexBufferMemory);

            this.CopyBuffer(stagingBuffer, this.vertexBuffer, bufferSize);

            stagingBuffer.Dispose();
            stagingBufferMemory.Free();
        }

        private void CreateIndexBuffer()
        {
            int indexSize = Unsafe.SizeOf<ushort>();

            ulong bufferSize = (uint)(indexSize * this.indices.Length);

            this.CreateBuffer(bufferSize, BufferUsageFlags.TransferSource, MemoryPropertyFlags.HostVisible | MemoryPropertyFlags.HostCoherent, out var stagingBuffer, out var stagingBufferMemory);

            IntPtr memoryBuffer = stagingBufferMemory.Map(0, bufferSize, MemoryMapFlags.None);

            for (int index = 0; index < indices.Length; index++)
            {
                Marshal.StructureToPtr(indices[index], memoryBuffer + (indexSize * index), false);
            }

            stagingBufferMemory.Unmap();

            this.CreateBuffer(bufferSize, BufferUsageFlags.TransferDestination | BufferUsageFlags.IndexBuffer, MemoryPropertyFlags.DeviceLocal, out this.indexBuffer, out this.indexBufferMemory);

            this.CopyBuffer(stagingBuffer, this.indexBuffer, bufferSize);

            stagingBuffer.Dispose();
            stagingBufferMemory.Free();
        }

        private void CreateCommandBuffers()
        {
            this.commandPool.Reset(CommandPoolResetFlags.ReleaseResources);

            this.commandBuffers = device.AllocateCommandBuffers(this.commandPool, CommandBufferLevel.Primary, (uint)this.frameBuffers.Length);

            for (int index = 0; index < this.frameBuffers.Length; index++)
            {
                var commandBuffer = this.commandBuffers[index];

                commandBuffer.Begin(CommandBufferUsageFlags.SimultaneousUse);

                commandBuffer.BeginRenderPass(this.renderPass,
                                                this.frameBuffers[index],
                                                new Rect2D(this.swapChainExtent),
                                                new ClearValue[1],
                                                SubpassContents.Inline);

                commandBuffer.BindPipeline(PipelineBindPoint.Graphics, this.pipeline);

                commandBuffer.BindVertexBuffers(0, this.vertexBuffer, (DeviceSize)0);

                commandBuffer.BindIndexBuffer(this.indexBuffer, 0, IndexType.Uint16);

                commandBuffer.BindDescriptorSets(PipelineBindPoint.Graphics, pipelineLayout, 0, descriptorSet, null);

                commandBuffer.DrawIndexed((uint)this.indices.Length, 1, 0, 0, 0);

                commandBuffer.EndRenderPass();

                commandBuffer.End();
            }
        }

        private void CreateSemaphores()
        {
            this.imageAvailableSemaphore = device.CreateSemaphore();
            this.renderFinishedSemaphore = device.CreateSemaphore();
        }

        private uint FindMemoryType(uint typeFilter, MemoryPropertyFlags flags)
        {
            var memoryProperties = this.physicalDevice.GetMemoryProperties();

            for (int i = 0; i < memoryProperties.MemoryTypes.Length; i++)
            {
                if ((typeFilter & (1u << i)) > 0
                        && memoryProperties.MemoryTypes[i].PropertyFlags.HasFlag(flags))
                {
                    return (uint)i;
                }
            }

            throw new Exception("No compatible memory type.");
        }

        private QueueFamilyIndices FindQueueFamilies(PhysicalDevice device)
        {
            QueueFamilyIndices indices = new QueueFamilyIndices();

            var queueFamilies = device.GetQueueFamilyProperties();

            for (uint index = 0; index < queueFamilies.Length && !indices.IsComplete; index++)
            {
                if (queueFamilies[index].QueueFlags.HasFlag(QueueFlags.Graphics))
                {
                    indices.GraphicsFamily = index;
                }

                if (device.GetSurfaceSupport(index, this.surface))
                {
                    indices.PresentFamily = index;
                }

                if (queueFamilies[index].QueueFlags.HasFlag(QueueFlags.Transfer) && !queueFamilies[index].QueueFlags.HasFlag(QueueFlags.Graphics))
                {
                    indices.TransferFamily = index;
                }
            }

            if (!indices.TransferFamily.HasValue)
            {
                indices.TransferFamily = indices.GraphicsFamily;
            }

            return indices;
        }

        private SurfaceFormat ChooseSwapSurfaceFormat(SurfaceFormat[] availableFormats)
        {
            if (availableFormats.Length == 1 && availableFormats[0].Format == Format.Undefined)
            {
                return new SurfaceFormat
                {
                    Format = Format.B8G8R8A8UNorm,
                    ColorSpace = ColorSpace.SrgbNonlinear
                };
            }

            foreach (var format in availableFormats)
            {
                if (format.Format == Format.B8G8R8A8UNorm && format.ColorSpace == ColorSpace.SrgbNonlinear)
                {
                    return format;
                }
            }

            return availableFormats[0];
        }

        private PresentMode ChooseSwapPresentMode(PresentMode[] availablePresentModes)
        {
            return availablePresentModes.Contains(PresentMode.Mailbox)
                    ? PresentMode.Mailbox
                    : PresentMode.Fifo;
        }

        public Extent2D ChooseSwapExtent(SurfaceCapabilities capabilities)
        {
            if (capabilities.CurrentExtent.Width != uint.MaxValue)
            {
                return capabilities.CurrentExtent;
            }
            else
            {
                return new Extent2D
                {
                    Width = Math.Max(capabilities.MinImageExtent.Width, Math.Min(capabilities.MaxImageExtent.Width, SurfaceWidth)),
                    Height = Math.Max(capabilities.MinImageExtent.Height, Math.Min(capabilities.MaxImageExtent.Height, SurfaceHeight))
                };
            }
        }

        SwapChainSupportDetails QuerySwapChainSupport(PhysicalDevice device)
        {
            return new SwapChainSupportDetails
            {
                Capabilities = device.GetSurfaceCapabilities(this.surface),
                Formats = device.GetSurfaceFormats(this.surface),
                PresentModes = device.GetSurfacePresentModes(this.surface)
            };
        }

        private bool IsSuitableDevice(PhysicalDevice device)
        {
            return device.EnumerateDeviceExtensionProperties(null).Any(extension => extension.ExtensionName == KhrExtensions.Swapchain)
                    && FindQueueFamilies(device).IsComplete;
        }

        private static uint[] LoadShaderData(string filePath, out int codeSize)
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var shaderData = new uint[(int)Math.Ceiling(fileBytes.Length / 4f)];

            System.Buffer.BlockCopy(fileBytes, 0, shaderData, 0, fileBytes.Length);

            codeSize = fileBytes.Length;

            return shaderData;
        }


        private CommandBuffer[] BeginSingleTimeCommand()
        {
            var result = device.AllocateCommandBuffers(this.transientCommandPool, CommandBufferLevel.Primary, 1);

            result[0].Begin(CommandBufferUsageFlags.OneTimeSubmit);

            return result;
        }

        private void EndSingleTimeCommand(CommandBuffer[] transferBuffers)
        {
            transferBuffers[0].End();

            this.transferQueue.Submit(new SubmitInfo { CommandBuffers = transferBuffers }, null);
            this.transferQueue.WaitIdle();

            this.transientCommandPool.FreeCommandBuffers(transferBuffers);
        }

        private void CreateBuffer(ulong size, BufferUsageFlags usage, MemoryPropertyFlags properties, out Buffer buffer, out DeviceMemory bufferMemory)
        {
            buffer = device.CreateBuffer(size, usage, SharingMode.Exclusive, null);

            var memRequirements = buffer.GetMemoryRequirements();

            bufferMemory = device.AllocateMemory(memRequirements.Size, FindMemoryType(memRequirements.MemoryTypeBits, properties));

            buffer.BindMemory(bufferMemory, 0);
        }

        public void CreateImage(uint width, uint height, Format format, ImageTiling imageTiling, ImageUsageFlags usage, MemoryPropertyFlags properties, out Image image, out DeviceMemory imageMemory)
        {
            image = this.device.CreateImage(ImageType.Image2d, format, new Extent3D(width, height, 1), 1, 1, SampleCountFlags.SampleCount1, imageTiling, usage, SharingMode.Exclusive, null, ImageLayout.Preinitialized);

            var memoryRequirements = image.GetMemoryRequirements();

            imageMemory = this.device.AllocateMemory(memoryRequirements.Size, this.FindMemoryType(memoryRequirements.MemoryTypeBits, properties));

            image.BindMemory(imageMemory, 0);
        }

        private void CopyImage(Image sourceImage, Image destinationImage, uint width, uint height)
        {
            var transferBuffers = this.BeginSingleTimeCommand();

            ImageSubresourceLayers subresource = new ImageSubresourceLayers
            {
                AspectMask = ImageAspectFlags.Color,
                BaseArrayLayer = 0,
                LayerCount = 1,
                MipLevel = 0
            };

            ImageCopy region = new ImageCopy
            {
                DestinationSubresource = subresource,
                SourceSubresource = subresource,
                SourceOffset = new Offset3D(),
                DestinationOffset = new Offset3D(),
                Extent = new Extent3D
                {
                    Width = width,
                    Height = height,
                    Depth = 1
                }
            };

            transferBuffers[0].CopyImage(sourceImage, ImageLayout.TransferSourceOptimal, destinationImage, ImageLayout.TransferDestinationOptimal, region);

            this.EndSingleTimeCommand(transferBuffers);
        }

        private void TransitionImageLayout(Image image, Format format, ImageLayout oldLayout, ImageLayout newLayout)
        {
            var commandBuffer = this.BeginSingleTimeCommand();

            var barrier = new ImageMemoryBarrier
            {
                OldLayout = oldLayout,
                NewLayout = newLayout,
                SourceQueueFamilyIndex = Constants.QueueFamilyIgnored,
                DestinationQueueFamilyIndex = Constants.QueueFamilyIgnored,
                Image = image,
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = ImageAspectFlags.Color,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1
                }
            };

            if (oldLayout == ImageLayout.Preinitialized && newLayout == ImageLayout.TransferSourceOptimal)
            {
                barrier.SourceAccessMask = AccessFlags.HostWrite;
                barrier.DestinationAccessMask = AccessFlags.TransferRead;
            }
            else if (oldLayout == ImageLayout.Preinitialized && newLayout == ImageLayout.TransferDestinationOptimal)
            {
                barrier.SourceAccessMask = AccessFlags.HostWrite;
                barrier.DestinationAccessMask = AccessFlags.TransferWrite;
            }
            else if (oldLayout == ImageLayout.TransferDestinationOptimal && newLayout == ImageLayout.ShaderReadOnlyOptimal)
            {
                barrier.SourceAccessMask = AccessFlags.TransferWrite;
                barrier.DestinationAccessMask = AccessFlags.ShaderRead;
            }
            else
            {
                throw new Exception("Unsupported layout transition");
            }

            commandBuffer[0].PipelineBarrier(PipelineStageFlags.TopOfPipe,
                                                PipelineStageFlags.TopOfPipe,
                                                null,
                                                null,
                                                barrier);

            this.EndSingleTimeCommand(commandBuffer);
        }

        public ImageView CreateImageView(Image image, Format format)
        {
            return device.CreateImageView(image, ImageViewType.ImageView2d, format, ComponentMapping.Identity, new ImageSubresourceRange(ImageAspectFlags.Color, 0, 1, 0, 1));
        }

        private void CopyBuffer(Buffer sourceBuffer, Buffer destinationBuffer, ulong size)
        {
            var transferBuffers = device.AllocateCommandBuffers(this.transientCommandPool, CommandBufferLevel.Primary, 1);

            transferBuffers[0].Begin(CommandBufferUsageFlags.OneTimeSubmit);

            transferBuffers[0].CopyBuffer(sourceBuffer, destinationBuffer, new BufferCopy { Size = size });

            transferBuffers[0].End();

            this.transferQueue.Submit(new SubmitInfo { CommandBuffers = transferBuffers }, null);
            this.transferQueue.WaitIdle();

            this.transientCommandPool.FreeCommandBuffers(transferBuffers);
        }

        private struct QueueFamilyIndices
        {
            public uint? GraphicsFamily;
            public uint? PresentFamily;
            public uint? TransferFamily;

            public IEnumerable<uint> Indices
            {
                get
                {
                    if (this.GraphicsFamily.HasValue)
                    {
                        yield return this.GraphicsFamily.Value;
                    }

                    if (this.PresentFamily.HasValue && this.PresentFamily != this.GraphicsFamily)
                    {
                        yield return this.PresentFamily.Value;
                    }

                    if (this.TransferFamily.HasValue && this.TransferFamily != this.PresentFamily && this.TransferFamily != this.GraphicsFamily)
                    {
                        yield return this.TransferFamily.Value;
                    }
                }
            }

            public bool IsComplete
            {
                get
                {
                    return this.GraphicsFamily.HasValue
                        && this.PresentFamily.HasValue
                        && this.TransferFamily.HasValue;
                }
            }
        }

        private struct SwapChainSupportDetails
        {
            public SurfaceCapabilities Capabilities;
            public SurfaceFormat[] Formats;
            public PresentMode[] PresentModes;
        }

        private struct VertexOutput
        {
            [Location(0)]
            public vec2 Uv;

            [BuiltIn(BuiltIn.Position)]
            public vec4 Position;
        }

        private struct FragmentInput
        {
            [Location(0)]
            public vec2 Uv;

            [BuiltIn(BuiltIn.FragCoord)]
            public vec4 FragCoord;
        }

        private struct FragmentOutput
        {
            [Location(0)]
            public vec4 Colour;

            [BuiltIn(BuiltIn.FragDepth)]
            public float FragDepth;
        }

        private struct Vertex
        {
            public Vertex(vec2 position, vec2 uv)
            {
                this.Position = position;
                this.Uv = uv;
                this.Test = 0;
            }

            [Location(0)]
            public vec2 Position;

            [Location(1)]
            public vec2 Uv;

            [Location(3)]
            public int Test;

            public static VertexInputBindingDescription GetBindingDescription()
            {
                return new VertexInputBindingDescription()
                {
                    Binding = 0,
                    Stride = (uint)Marshal.SizeOf<Vertex>(),
                    InputRate = VertexInputRate.Vertex
                };
            }

            public static VertexInputAttributeDescription[] GetAttributeDescriptions()
            {
                return new VertexInputAttributeDescription[]
                {
                    new VertexInputAttributeDescription
                    {
                        Binding = 0,
                        Location = 0,
                        Format = Format.R32G32SFloat,
                        Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position))
                    },
                    new VertexInputAttributeDescription
                    {
                        Binding = 0,
                        Location = 1,
                        Format = Format.R32G32SFloat,
                        Offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Uv))
                    }
                };
            }
        }
    }
}
