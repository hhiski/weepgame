﻿// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/SkyboxCubemap" {
	Properties{
		_Tint("Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
		_Rotation("Rotation", Range(0, 360)) = 0
		_StarMulti("Star Texture Multiplier", Range(0, 10)) = 0.5
		[NoScaleOffset] _TextureNebulae("Cubemap Nebulae", Cube) = "grey" {}
		[NoScaleOffset] _TextureStars("Cubemap Stars", Cube) = "grey" {}
	}

		SubShader{
			Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
			Cull Off ZWrite Off

			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"

				samplerCUBE _TextureNebulae;
				samplerCUBE _TextureStars;	
				half4 _Tex_HDR;
				half4 _Tint;
				half _Exposure;
				float _Rotation;
				half _StarMulti;

				float3 RotateAroundYInDegrees(float3 vertex, float degrees)
				{
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float3(mul(m, vertex.xz), vertex.y).xzy;
				}

				struct appdata_t {
					float4 vertex : POSITION;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float3 texcoord : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
					o.vertex = UnityObjectToClipPos(rotated);
					o.texcoord = v.vertex.xyz;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					half4 texNebulae = texCUBE(_TextureNebulae, i.texcoord);
					half4 texStars = texCUBE(_TextureStars, i.texcoord);
				//	half3 c = DecodeHDR(tex, _Tex_HDR);

					half3 c = (( texNebulae * _Tint.rgb) + (_StarMulti*texStars)) * _Exposure;
					return half4(c, 1);
				}
				ENDCG
			}
	}


		Fallback Off

}