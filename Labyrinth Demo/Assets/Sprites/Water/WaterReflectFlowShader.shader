Shader "Custom/WaterReflectFlowShader"
{
	Properties
	{
		_MainText ("Texture", 2D) = "white" {}
		_NoiseText("Noise Texture", 2D) = "white" {}
		_ReflectText("Reflection Texture", 2D) = "white" {}
		_ReflectionPoint("Reflection Point", Vector) = (0, 0, 0, 0)
		_LightAngle("LightAngle", Float) = 0
		_FlowRate("Water Flow Rate", Float) = 1
		_ReflectDist("ReflectDist", Float) = 5
		_ReflectCoef("Reflection Coefficient", Range(0.01, 0.99)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				float4 worldPos : TEXCOORD1;
			};

			sampler2D _MainText;
			sampler2D _NoiseText;
			sampler2D _ReflectText;
			Vector _ReflectionPoint;
			float4 _MainText_ST;
			float _FlowRate;
			float _ReflectCoef;
			float _ReflectDist;
			float _LightAngle;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainText);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				
				fixed2 flowOffset = fixed2(0, _Time[0] / _FlowRate);
				fixed4 offset = tex2D(_NoiseText, IN.uv);
				fixed2 adjusted = IN.uv.xy + (offset.rg - 0.5);
				
				fixed4 water = tex2D(_MainText, adjusted + flowOffset);
				

				//fixed2 reflectOffset = float2(0.0, 0.0);
				//fixed4 reflect = tex2D(_ReflectText, IN.uv + reflectOffset);
				//if (IN.worldPos.y < _ReflectionPoint.y) {
				//	reflect = fixed4(0, 0, 0, 0);
				//}

				//fixed4 col = reflect;
				fixed4 col = water;
				return col;
			}
			ENDCG
		}
	}
}
