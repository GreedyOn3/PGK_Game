Shader "Custom/DepthNormals Layer"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }

            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD0;
                float  depth      : TEXCOORD1;
            };
            
            Varyings vert (Attributes v)
            {
                Varyings o;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.depth = o.positionCS.w;

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // [-1,1] -> [0,1]
                float3 normal = i.normalWS * 0.5 + 0.5;
                // Linear depth
                float depth = i.depth / _ProjectionParams.z;

                return half4(normal, depth);
            }

            ENDHLSL
        }
    }
}