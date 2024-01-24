Shader "Custom/CloudSurface" {
	Properties{
		_Glossiness("Smoothness", Range(-5,10)) = 0.5
		_Metallic("Metallic", Range(-5,10)) = 0.0
				_Offset("Offset", Range(-5,10)) = 0.0
		_AlphaOffset("Alpha Offset", Range(-1,1)) = -0.05
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent"}
		
		 Blend One One
		LOD 200
		Cull Off


			CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard  alpha vertex:vert fullforwardshadows 


		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input
		{				
			float vertexAlpha;
			float3 normal: NORMAL;
			float4 pos : SV_POSITION;

		};

		float4 _Color;
		half _Glossiness;
		half _Metallic;
		half _AlphaOffset;
		half _Offset;
		half vertexAlpha; //Alpha channel of the vertex color will be used as a cloud coverage level. 

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			v.vertex.xyz = normalize(v.vertex.xyz);
			o.pos = v.vertex;
			v.normal = normalize(v.vertex.xyz);

			o.vertexAlpha = v.color.a;

		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{

			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = saturate(c.a)*IN.vertexAlpha + _AlphaOffset;
		}
		ENDCG
		}
	
}

