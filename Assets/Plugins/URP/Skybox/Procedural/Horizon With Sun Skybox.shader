// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Virsabi/URP/Skybox/Procedural Horizon With Sun Skybox"
{
    Properties
    {
        _SkyColor1("Top Color", Color) = (0.37, 0.52, 0.73, 0)
        _SkyExponent1("Top Exponent", Float) = 8.5

        _SkyColor2("Horizon Color", Color) = (0.89, 0.96, 1, 0)

        _SkyColor3("Bottom Color", Color) = (0.89, 0.89, 0.89, 0)
        _SkyExponent2("Bottom Exponent", Float) = 3.0

        _SkyIntensity("Sky Intensity", Float) = 1.0

        _SunColor("Sun Color", Color) = (1, 0.99, 0.87, 1)
        _SunIntensity("Sun Intensity", float) = 2.0

        _SunAlpha("Sun Alpha", float) = 550
        _SunBeta("Sun Beta", float) = 1

        _SunVector("Sun Vector", Vector) = (0.269, 0.615, 0.740, 0)

        _SunAzimuth("Sun Azimuth (editor only)", float) = 20
        _SunAltitude("Sun Altitude (editor only)", float) = 38
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    #ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
	//only defining to not throw compilation error over Unity 5.5
	#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
	#endif

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;

        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
        
        UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
    };
    
    half3 _SkyColor1;
    half _SkyExponent1;

    half3 _SkyColor2;

    half3 _SkyColor3;
    half _SkyExponent2;

    half _SkyIntensity;

    half3 _SunColor;
    half _SunIntensity;

    half _SunAlpha;
    half _SunBeta;

    half3 _SunVector;
    
    v2f vert(appdata v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		UNITY_TRANSFER_INSTANCE_ID(v, o);
        o.position = UnityObjectToClipPos(v.position);
        o.texcoord = v.texcoord;
        return o;
    }
    
    half4 frag(v2f i) : COLOR
    {
        UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

        float3 v = normalize(i.texcoord);

        float p = v.y;
        float p1 = 1 - pow(min(1, 1 - p), _SkyExponent1);
        float p3 = 1 - pow(min(1, 1 + p), _SkyExponent2);
        float p2 = 1 - p1 - p3;

        half3 c_sky = _SkyColor1 * p1 + _SkyColor2 * p2 + _SkyColor3 * p3;
        half3 c_sun = _SunColor * min(pow(max(0, dot(v, _SunVector)), _SunAlpha) * _SunBeta, 1);

        return half4(c_sky * _SkyIntensity + c_sun * _SunIntensity, 0);
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
    CustomEditor "HorizonWithSunSkyboxInspector"
}
