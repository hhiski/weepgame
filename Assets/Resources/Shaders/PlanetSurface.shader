Shader "Custom/PlanetSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
			//_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
			_RimColor("Rim Color", Color) = (1,1,1,0.0)
			_RimPower("Rim Power", Range(0,84.0)) = 3.0

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard vertex:vert fullforwardshadows Planet
			#pragma target 4.0
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // for _LightColor0

			struct Input {
				float2 uv_MainTex;
				float3 vertexColor; // Vertex color stored here by vert() method
				float3 viewDir;
				float3 lightDir;
				float2 uv_BumpMap;
				float3 worldRefl; INTERNAL_DATA
			};

			struct v2f {
			  float4 pos : SV_POSITION;
			  fixed4 color : COLOR;
			  // nointerpolation fixed4 color : COLOR;
			 };

			 void vert(inout appdata_full v, out Input o)
			 {
				 UNITY_INITIALIZE_OUTPUT(Input,o);
				 o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method


			 }

			 sampler2D _MainTex;
			 float4 _RimColor;
			 float _RimPower;
			 sampler2D _BumpMap;
			 half _Glossiness;
			 half _Metallic;
			 fixed4 _Color;


			 void surf(Input IN, inout SurfaceOutputStandard o)
			 {

				 // half scatter = pow(saturate(dot(normalize(IN.lightDir), o.Normal) + 0.5f), 1.2f);
				 // scatter = clamp(scatter, 0, 1);

				  half rim = (1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)));

				  // Albedo comes from a texture tinted by color
				 // fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				  o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * IN.vertexColor + _RimColor.rgb * pow(rim, _RimPower); // Combine normal color with the vertex color

				  o.Metallic = _Metallic;
				  o.Smoothness = _Glossiness;
				  o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
				  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));



				  //	 half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
					   //half rim = 1.0 - clamp(saturate(dot(normalize(IN.lightDir), o.Normal)),0,1);
					   //half rim = ((1 - (dot(normalize(o.lightDir), (o.Normal)))) + ((dot(normalize(IN.viewDir), (o.Normal)))));


					  // o.Emission = _RimColor.rgb * pow(rim, _RimPower)  ;
					   //o.Emission = float3(nl, 1, 0);
				   }
				   ENDCG
		}
			FallBack "Diffuse"
}