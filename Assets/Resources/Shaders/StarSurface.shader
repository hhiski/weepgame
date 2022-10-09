// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StarSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

	    _BumpMap("Bumpmap", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		//_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimColor("Rim Color", Color) = (1,1,1,1.0)
	    _RimPower("Rim Power", Range(0.0,10.0)) = 0.75
		_ExtraEmission("Extra Brightness", Range(0.0,10.0)) = 0.75
	}
		SubShader{

	



			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 200

			CGPROGRAM
			#pragma   surface surf StandardSpecular      noshadow nometa keepalpha 
			#pragma target 4.0
			struct Input {
				float2 uv_MainTex;
				float3 viewDir;
				float2 uv_BumpMap;
			};

			struct v2f {
			  float4 pos : SV_POSITION;
			  fixed4 color : COLOR;
			 // nointerpolation fixed4 color : COLOR;
			};


			sampler2D _MainTex;
			float4 _RimColor;
			float _RimPower;
			float _ExtraEmission;
			sampler2D _BumpMap;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutputStandardSpecular o)
			{


				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _RimColor;
				//o.Albedo = c.rgb
				o.Albedo = _RimColor; // Combine normal color with the vertex color
				// Metallic and smoothness come from slider variables

				o.Occlusion = _Metallic;
				o.Specular = _Glossiness;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				half rim =  saturate(o.Alpha = _Color.a*dot(normalize(IN.viewDir), o.Normal));
				o.Emission = (_Color.rgb - (_ExtraEmission*c.rgb)) * pow(rim, _RimPower) + (_ExtraEmission*c.rgb);

			}
			ENDCG
		}
			FallBack "Diffuse"
}