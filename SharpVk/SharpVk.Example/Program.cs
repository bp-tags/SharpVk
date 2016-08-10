﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpVk.Example
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var instance = Instance.Create(new InstanceCreateInfo
            {
                ApplicationInfo = new ApplicationInfo
                {
                    ApplicationName = "Example Application",
                    EngineName = "SharpVK"
                },
                EnabledExtensionNames = new[] { "VK_KHR_surface", "VK_KHR_win32_surface" }
            }, null);

            var surfaceForm = new Form
            {
                ClientSize = new Size(1280, 720)
            };

            var surfaceCreateInfo = new Win32SurfaceCreateInfo
            {
                Flags = Win32SurfaceCreateFlags.None,
                Hinstance = IntPtr.Zero,
                Hwnd = surfaceForm.Handle
            };

            var surface = instance.CreateWin32Surface(surfaceCreateInfo, null);

            var physicalDevice = instance.EnumeratePhysicalDevices().First();

            var surfaceCapabilities = physicalDevice.GetSurfaceCapabilities(surface);

            var queueFamilies = physicalDevice.GetQueueFamilyProperties();

            var presentableQueues = Enumerable.Range(0, queueFamilies.Length)
                                                .Select(x => (uint)x)
                                                .Where(x => physicalDevice.GetSurfaceSupport(x, surface))
                                                .ToArray();

            var graphicsQueues = presentableQueues
                                            .Where(x => queueFamilies[x].QueueFlags.HasFlag(QueueFlags.Graphics))
                                            .ToArray();

            var surfaceFormats = physicalDevice.GetSurfaceFormats(surface);

            var device = physicalDevice.CreateDevice(new DeviceCreateInfo
            {
                QueueCreateInfos = new[]
                {
                    new DeviceQueueCreateInfo
                    {
                        QueueFamilyIndex = graphicsQueues.First(),
                        QueuePriorities = new float[] { 1 }
                    }
                }
            }, null);

            var presentQueue = device.GetQueue(graphicsQueues.First(), 0);

            var swapchain = device.CreateSwapchain(new SwapchainCreateInfo
            {
                Surface = surface,
                Flags = SwapchainCreateFlags.None,
                PresentMode = PresentMode.Immediate,
                MinImageCount = 2,
                ImageExtent = surfaceCapabilities.CurrentExtent,
                ImageUsage = ImageUsageFlags.ColorAttachment,
                PreTransform = surfaceCapabilities.CurrentTransform,
                ImageArrayLayers = 1,
                ImageSharingMode = SharingMode.Exclusive,
                ImageFormat = surfaceFormats.First().Format,
                ImageColorSpace = surfaceFormats.First().ColorSpace,
                Clipped = true,
                CompositeAlpha = surfaceCapabilities.SupportedCompositeAlpha
            }, null);

            var images = swapchain.GetImages();

            var presentCompleteSemaphore = device.CreateSemaphore(new SemaphoreCreateInfo
            {
                Flags = SemaphoreCreateFlags.None
            }, null);

            var imageViews = images.Select(image => device.CreateImageView(new ImageViewCreateInfo
            {
                Components = new ComponentMapping
                {
                    R = ComponentSwizzle.R,
                    G = ComponentSwizzle.G,
                    B = ComponentSwizzle.B,
                    A = ComponentSwizzle.A
                },
                Format = surfaceFormats.First().Format,
                Image = image,
                Flags = ImageViewCreateFlags.None,
                ViewType = ImageViewType.ImageView2d,
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = ImageAspectFlags.Color,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1
                }
            }, null)).ToArray();

            uint nextImage = swapchain.AcquireNextImage(uint.MaxValue, presentCompleteSemaphore, null);

            presentQueue.Present(new PresentInfo
            {
                ImageIndices = new uint[] { nextImage },
                Results = null,
                WaitSemaphores = null,
                Swapchains = new[] { swapchain }
            });

            surfaceForm.ShowDialog();

            device.WaitIdle();

            foreach (var imageView in imageViews)
            {
                imageView.Destroy(null);
            }

            foreach (var image in images)
            {
                image.Destroy(null);
            }

            presentCompleteSemaphore.Destroy(null);

            //swapchain.Destroy(null);

            device.Destroy(null);

            surface.Destroy(null);

            instance.Destroy(null);
        }
    }
}