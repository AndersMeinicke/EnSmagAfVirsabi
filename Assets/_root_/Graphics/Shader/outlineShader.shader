Shader"Custom/OutlineShader" {
    Properties {
        _Color ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.05
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

sampler2D _MainTex;
float _OutlineWidth;
float4 _Color;

struct Input
{
    float2 uv_MainTex;
};

void surf(Input IN, inout SurfaceOutput o)
{
    o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
    o.Alpha = 1;
    float edge = tex2D(_MainTex, IN.uv_MainTex + float2(_OutlineWidth, 0)).a
                       + tex2D(_MainTex, IN.uv_MainTex - float2(_OutlineWidth, 0)).a
                       + tex2D(_MainTex, IN.uv_MainTex + float2(0, _OutlineWidth)).a
                       + tex2D(_MainTex, IN.uv_MainTex - float2(0, _OutlineWidth)).a;
    if (edge < 1)
    {
        o.Albedo = _Color.rgb;
    }
}
        ENDCG
    }
FallBack"Diffuse"
}
