Shader "Virsabi/URP/AR/AR Shadow Receiver With zWrite Block"
{
    Properties
    {
        _ShadowColor("Shadow Color", Color) = (0.35,0.4,0.45,0.5)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent-1"
            "Queue" = "Geometry-1"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }
        LOD 300

        Pass
        {
            Name "AR Proxy"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            //Blend SrcAlpha OneMinusSrcAlpha
            Blend DstColor Zero, Zero One
            ZWrite On
            Cull Off

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

        // -------------------------------------
        // Material Keywords
        #pragma shader_feature _ALPHATEST_ON
        #pragma shader_feature _ALPHAPREMULTIPLY_ON

        // -------------------------------------
        // Lightweight Pipeline keywords
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile_fog

        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ LIGHTMAP_ON

        //--------------------------------------
        // GPU Instancing
        #pragma multi_compile_instancing

        #pragma vertex HiddenVertex
        #pragma fragment HiddenFragment

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


        CBUFFER_START(UnityPerMaterial)
        float4 _ShadowColor;
        CBUFFER_END


        struct Attributes
        {
            UNITY_VERTEX_INPUT_INSTANCE_ID
            float4 positionOS   : POSITION;
        };

        struct Varyings
        {
            float4 positionCS   : SV_POSITION;
            float4 shadowCoord  : TEXCOORD0;
            float3 positionWS   : TEXCOORD1;
            float fogCoord : TEXCOORD2;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

        Varyings HiddenVertex(Attributes input)
        {
            Varyings output = (Varyings)0;

            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

            output.shadowCoord = GetShadowCoord(vertexInput);
            output.positionCS = vertexInput.positionCS;
            output.positionWS = vertexInput.positionWS;
            output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
            
            return output;
        }

        half4 HiddenFragment(Varyings input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
            
            half4 color;
            half s = MainLightRealtimeShadow(input.shadowCoord);

            VertexPositionInputs vertexInput = (VertexPositionInputs)0;
            vertexInput.positionWS = input.positionWS;

            float4 shadowCoord = GetShadowCoord(vertexInput);
            half shadowAttenutation = MainLightRealtimeShadow(input.shadowCoord);
            color = lerp(half4(1, 1, 1, 1), _ShadowColor, (1.0 - shadowAttenutation) * _ShadowColor.a);
            return color;
            //return half4(_ShadowColor.r, _ShadowColor.g, _ShadowColor.b, _ShadowColor.a - s);
            //return half4(0, 0, 0, 1 - s);
        }
        ENDHLSL
    }

    UsePass "Universal Render Pipeline/Lit/DepthOnly"
    }

        FallBack "Hidden/InternalErrorShader"
}