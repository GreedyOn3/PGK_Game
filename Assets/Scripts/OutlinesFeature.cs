using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering;

public class OutlinesFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public LayerMask layerMask;
        public Material depthNormalsMaterial;
        public Material outlineMaterial;
    }
    public Settings settings = new Settings();
    static string DepthNormalsName = "_OutlineDepthNormals";

    class PassData
    {
        public RendererListHandle rendererList;
    }

    class DepthNormalsPass : ScriptableRenderPass
    {
        Settings settings;

        public DepthNormalsPass(Settings settings)
        {
            this.settings = settings;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalRenderingData renderingData = frameData.Get<UniversalRenderingData>();
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
            UniversalLightData lightData = frameData.Get<UniversalLightData>();
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            // RENDERER LIST CREATION
            DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(
                new ShaderTagId("SRPDefaultUnlit"),
                renderingData,
                cameraData,
                lightData,
                SortingCriteria.CommonOpaque
            );
            drawingSettings.overrideMaterial = settings.depthNormalsMaterial;

            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque, settings.layerMask);
            RendererListParams rlParams = new RendererListParams( renderingData.cullResults, drawingSettings, filteringSettings);
            RendererListHandle rendererList = renderGraph.CreateRendererList(rlParams);

            // TEXTURE CREATION
            TextureDesc textureDesc = resourceData.activeColorTexture.GetDescriptor(renderGraph);
            textureDesc.name = DepthNormalsName;
            textureDesc.depthBufferBits = DepthBits.None;
            textureDesc.msaaSamples = MSAASamples.None;
            textureDesc.colorFormat = GraphicsFormat.R16G16B16A16_SFloat;

            TextureHandle texture = renderGraph.CreateTexture(textureDesc);

            // PASS
            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Outline DepthNormals Pass", out var passData))
            {
                passData.rendererList = rendererList;

                builder.UseRendererList(rendererList);
                builder.SetRenderAttachment(texture, 0);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                {
                    context.cmd.ClearRenderTarget(true, true, Color.black);
                    context.cmd.DrawRendererList(data.rendererList);
                });

                builder.SetGlobalTextureAfterPass(texture, Shader.PropertyToID(DepthNormalsName));
            }
        }
    }

    class EdgeDetectionPass : ScriptableRenderPass
    {
        Settings settings;

        public EdgeDetectionPass(Settings settings)
        {
            this.settings = settings;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            // OUTLINE TEXTURE CREATION
            TextureHandle source = resourceData.activeColorTexture;
            TextureDesc destDesc = renderGraph.GetTextureDesc(source);
            destDesc.name = "Outlines";
            destDesc.clearBuffer = false;

            TextureHandle dest = renderGraph.CreateTexture(destDesc);

            // BLITING TEXTURE
            RenderGraphUtils.BlitMaterialParameters param = new(source, dest, settings.outlineMaterial, 0);
            using (var builder = renderGraph.AddBlitPass(param, "Outline Blit", true))
            {
                builder.UseGlobalTexture(Shader.PropertyToID(DepthNormalsName));
            }
            resourceData.cameraColor = dest;
        }
    }

    DepthNormalsPass dnPass;
    EdgeDetectionPass sobelPass;

    public override void Create()
    {
        dnPass = new DepthNormalsPass(settings);
        dnPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;

        sobelPass = new EdgeDetectionPass(settings);
        sobelPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.depthNormalsMaterial == null || settings.outlineMaterial == null)
            return;

        renderer.EnqueuePass(dnPass);
        renderer.EnqueuePass(sobelPass);
    }
}