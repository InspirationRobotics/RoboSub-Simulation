Shader "vdev/FX/Wobble (Screen)"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Intensity("Intensity", Range(0.0, 360.0)) = 20.0
		_Speed("Speed", Range(0.0, 20.0)) = 0.0
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				uniform float4 _Color;
				float _Intensity;
				const float PI = 3.14;
				float _Speed;

				float screen(float a, float b)
				{
					float c = 1 - (2 * (1 - a) *(1 - b));
					return c;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float sine = sin((i.uv.y + _Speed*_Time[0])* _Intensity);
					i.uv.x += sine * 0.01;

					fixed4 col = tex2D(_MainTex, i.uv);
					col.r = screen(col.r, _Color.r);
					col.g = screen(col.g, _Color.g);
					col.b = screen(col.b, _Color.b);
					return col;
				}
			ENDCG
		}
		}
}
