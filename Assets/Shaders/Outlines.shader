Shader "Custom/Outlines Shader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 1

        _NormalThreshold ("Normal Threshold", Float) = 0.2
        _DepthThreshold  ("Depth Threshold", Float) = 0.01

        _FadeStart ("Fade Start Distance", Float) = 20.0
        _FadeEnd ("Fade End Distance", Float) = 50.0
    }

    SubShader
    {
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        ENDHLSL

        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off

        Pass
        {
            Name "Outlines"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            TEXTURE2D(_OutlineDepthNormals);
            SAMPLER(sampler_OutlineDepthNormals);

            float4 _OutlineColor;
            float _OutlineThickness;

            float _NormalThreshold;
            float _DepthThreshold;

            float _FadeStart;
            float _FadeEnd;

            float RobertsCross(float3 samples[4])
            {
                const float3 diff1 = samples[1] - samples[2];
                const float3 diff2 = samples[0] - samples[3];
                return sqrt(dot(diff1, diff1) + dot(diff2, diff2));
            }
            float RobertsCross(float samples[4])
            {
                const float diff1 = samples[1] - samples[2];
                const float diff2 = samples[0] - samples[3];
                return sqrt(diff1 * diff1 + diff2 * diff2);
            }

            float SampleEdge(float2 uv)
            {
                float2 texel = _BlitTexture_TexelSize.xy;
                
                // CALCULATING PIXEL POSITIONS
                const float halfWidthFloor = floor(_OutlineThickness * 0.5);
                const float halfWidthCeil = ceil(_OutlineThickness * 0.5);

                float2 uvs[4];
                uvs[0] = uv + texel * float2(halfWidthFloor, halfWidthCeil) * float2(-1, 1);  // top left
                uvs[1] = uv + texel * float2(halfWidthCeil, halfWidthCeil) * float2(1, 1);   // top right
                uvs[2] = uv + texel * float2(halfWidthFloor, halfWidthFloor) * float2(-1, -1); // bottom left
                uvs[3] = uv + texel * float2(halfWidthCeil, halfWidthFloor) * float2(1, -1);  // bottom right

                // SAMPLING DEPTH_NORMALS TEXTURE
                float3 normalSamples[4];
                float depthSamples[4];
                half minDepth = 999;

                for (int i = 0; i < 4; i++) 
                {
                    half4 dn = SAMPLE_TEXTURE2D(_OutlineDepthNormals, sampler_OutlineDepthNormals, uvs[i]);
                    normalSamples[i] = dn.rgb;
                    depthSamples[i] = dn.a;

                    minDepth = min(minDepth, dn.a);
                }

                // CALCULATING FADE BASED ON DISTANCE
                float linearDepth = minDepth * _ProjectionParams.z;
                float distanceFade = saturate((_FadeEnd - linearDepth) / max(0.0001, _FadeEnd - _FadeStart));

                // PERFORMING AND COMBINING EDGE DETECTION RESULTS
                float nMask = smoothstep(_NormalThreshold, 1, RobertsCross(normalSamples));
                float dMask = step(_DepthThreshold, RobertsCross(depthSamples));

                return max(nMask, dMask) * distanceFade;
            }

            half4 Frag (Varyings i) : SV_Target
            {
                float edge = SampleEdge(i.texcoord);

                float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, i.texcoord).rgba;
                return lerp(color, _OutlineColor, edge);
            }

            ENDHLSL
        }
    }
}