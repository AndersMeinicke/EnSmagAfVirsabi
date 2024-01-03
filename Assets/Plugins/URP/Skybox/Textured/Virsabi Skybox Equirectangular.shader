// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Virsabi/URP/Skybox/Equirectangular" {
    Properties{
        _Tint("Tint Color", Color) = (.5, .5, .5, .5)
        [Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
        _Rotation("Rotation", Range(0, 360)) = 0
        [NoScaleOffset] _Tex("Panorama (HDR)", 2D) = "grey" {}
    }

        SubShader{
            Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
            Cull Off ZWrite Off

            Pass {

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                #ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
	            //only defining to not throw compilation error over Unity 5.5
	            #define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
	            #endif

                #pragma multi_compile_instancing

                sampler2D _Tex;
                half4 _Tex_HDR;
                half4 _Tint;
                half _Exposure;
                float _Rotation;

                float4 RotateAroundYInDegrees(float4 vertex, float degrees)
                {
                    float alpha = degrees * UNITY_PI / 180.0;
                    float sina, cosa;
                    sincos(alpha, sina, cosa);
                    float2x2 m = float2x2(cosa, -sina, sina, cosa);
                    return float4(mul(m, vertex.xz), vertex.yw).xzyw;
                }

                struct appdata_t {
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    float4 vertex : POSITION;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float3 texcoord : TEXCOORD0;
                    
                    UNITY_VERTEX_INPUT_INSTANCE_ID
		            UNITY_VERTEX_OUTPUT_STEREO
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
		            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		            UNITY_TRANSFER_INSTANCE_ID(v, o);
                    o.vertex = UnityObjectToClipPos(RotateAroundYInDegrees(v.vertex, _Rotation));
                    o.texcoord = v.vertex.xyz;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    
                    UNITY_SETUP_INSTANCE_ID(i);
		            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                    float3 dir = normalize(i.texcoord);
                    float2 longlat = float2(atan2(dir.x, dir.z) + UNITY_PI, acos(-dir.y));
                    float2 uv = longlat / float2(2.0 * UNITY_PI, UNITY_PI);
                    half4 tex = tex2D(_Tex, uv);
                    half3 c = DecodeHDR(tex, _Tex_HDR);
                    c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
                    c *= _Exposure;

                    return half4(c, 1);
                }
                ENDCG
            }
    }


        Fallback Off

}
