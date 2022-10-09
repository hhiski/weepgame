Shader "Custom/CloudSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_RimPower("Rim Power", Range(0,15.0)) = 3.0

		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			LOD 200
			   ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows alpha


			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float4 color : COLOR;
				float3 worldPos;

				float3 viewDir;
				float2 uv_BumpMap;
			};

			sampler2D _BumpMap;
			float _RimPower;

			void surf(Input IN, inout SurfaceOutputStandard o) {

				fixed4 c = IN.color;
				o.Albedo = c.rgb;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				half rim =  saturate(dot(normalize(IN.viewDir), o.Normal));
				half edge = 1;
				if (rim < 0.5) {
					edge = pow(2*rim, _RimPower); 
				};
				//o.Alpha = (2*c.a)*pow(rim, _RimPower)-1;
				o.Alpha = (2 * c.a)*edge - 1;
			}
			ENDCG
		}
			FallBack "Diffuse"
}

