Shader "Custom/Opacity"
{
	Properties
	{
		_BaseMap("Base Map", 2D) = "white" {}
		_BaseColor("Base Color", Color) = (1,1,1,1)


		_MetallicMap("Metallic Map", 2D) = "white" {}
		_MetallicScale("Metallic Scale", Range(0, 1)) = 0.0

		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 1.0


		_OpacityMap("Opacity Map", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			BlendOp Add
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;

				float3 T : TEXCOORD1;
				float3 B : TEXCOORD2;
				float3 N : TEXCOORD3;

				float3 lightDir : TEXCOORD4;
				half3 viewDir : TEXCOORD5;
			};

			sampler2D _BaseMap;
			float4 _BaseMap_ST;
			float4 _BaseColor;

			sampler2D _NormalMap;
			float4 _NormalMap_ST;
			float _NormalScale;

			sampler2D _OpacityMap;

			void Fuc_LocalNormal2TBN(half3 localnormal, float4 tangent, inout half3 T, inout half3  B, inout half3 N)
			{
				half fTangentSign = tangent.w * unity_WorldTransformParams.w;
				N = normalize(UnityObjectToWorldNormal(localnormal));
				T = normalize(UnityObjectToWorldDir(tangent.xyz));
				B = normalize(cross(N, T) * fTangentSign);
			}

			half3 Fuc_TangentNormal2WorldNormal(half3 fTangnetNormal, half3 T, half3  B, half3 N)
			{
				float3x3 TBN = float3x3(T, B, N);
				TBN = transpose(TBN);
				return mul(TBN, fTangnetNormal);
			}


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _BaseMap);

				o.lightDir = WorldSpaceLightDir(v.vertex);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

				Fuc_LocalNormal2TBN(v.normal, v.tangent, o.T, o.B, o.N);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_BaseMap, i.uv) * _BaseColor;
				half3 fTangnetNormal = UnpackNormal(tex2D(_NormalMap, i.uv * _NormalMap_ST.rg));
				fTangnetNormal.xy *= _NormalScale;
				float3 worldNormal = Fuc_TangentNormal2WorldNormal(fTangnetNormal, i.T, i.B, i.N);

				fixed fNDotL = dot(i.lightDir, worldNormal);

				float3 fReflection = reflect(i.lightDir, worldNormal);
				fixed fSpec_Phong = saturate(dot(fReflection, -normalize(i.viewDir)));
				fSpec_Phong = pow(fSpec_Phong, 20.0f);

				half opacity = tex2D(_OpacityMap, i.uv).r;
				half4 finalColor = float4(col.rgb * fNDotL + fSpec_Phong, opacity);

				return finalColor;
			}
			ENDCG
		}
	}
}