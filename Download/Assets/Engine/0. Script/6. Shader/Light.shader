Shader "Custom/Light"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,0,0.5) // Slightly yellow with some transparency
        _Opacity("Opacity", Range(0,1)) = 0.5 // Controls the transparency
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            BlendOp Add

            ZWrite On
            ZTest Less
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 baseColor   : COLOR;
            };

            float4 _BaseColor;
            float _Opacity;

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.baseColor = _BaseColor;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 finalColor = input.baseColor;
                finalColor.a *= _Opacity;

                return finalColor;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Unlit"
}