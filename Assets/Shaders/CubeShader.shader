Shader "Custom/CubeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input 
        {
          float2 uv_MainTex;
          float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float colFactor = 1.0 - frac(IN.worldPos.z * 0.1);
            float3 col = float3(colFactor, colFactor, colFactor);

            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * col;
        }
        ENDCG
    }
    FallBack "Diffuse"
}