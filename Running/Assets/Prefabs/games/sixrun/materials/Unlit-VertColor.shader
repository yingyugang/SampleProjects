// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/VertColor" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_A("a", Range (0,1)) = 1
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

	
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float4 color:COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				float _A;
				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.color = v.color;
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					float4 c = i.color;
					float4 tc = tex2D(_MainTex, i.texcoord);

					c = lerp(tc,c,c.a);
					c.a *= _A;
					return c;
				}
			ENDCG
		}
	}
}
