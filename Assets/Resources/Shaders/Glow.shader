// Compiled shader for WebGL

//////////////////////////////////////////////////////////////////////////
// 
// NOTE: This is *not* a valid shader file, the contents are provided just
// for information and for debugging purposes only.
// 
//////////////////////////////////////////////////////////////////////////
// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/Glow" {
	Properties{
	 _MainTex("Particle Texture", 2D) = "white" { }
	}
		SubShader{
		 Tags { "QUEUE" = "Background" "IGNOREPROJECTOR" = "true" "RenderType" = "Background" "PreviewType" = "Background" }
		 Pass {
		  Tags { "QUEUE" = "Background" "IGNOREPROJECTOR" = "true" "RenderType" = "Background" "PreviewType" = "Background" }
		  ZWrite Off
		  Cull Off
		  Blend SrcAlpha One
		//////////////////////////////////
		//                              //
		//      Compiled programs       //
		//                              //
		//////////////////////////////////
	  //////////////////////////////////////////////////////
	  Global Keywords : <none>
	  Local Keywords : <none>
	  --Hardware tier variant : Tier 1
	  --Vertex shader for "gles" :
	  Shader Disassembly :
	  #ifdef VERTEX
	  #version 100

	  uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
	  uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
	  uniform 	vec4 _MainTex_ST;
	  attribute highp vec3 in_POSITION0;
	  attribute mediump vec4 in_COLOR0;
	  attribute highp vec3 in_TEXCOORD0;
	  varying mediump vec4 vs_COLOR0;
	  varying highp vec2 vs_TEXCOORD0;
	  vec4 u_xlat0;
	  vec4 u_xlat1;
	  void main()
	  {
		  vs_COLOR0 = in_COLOR0;
		  vs_COLOR0 = clamp(vs_COLOR0, 0.0, 1.0);
		  vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
		  u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
		  u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
		  u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
		  u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
		  u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
		  u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
		  gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
		  return;
	  }

	  #endif
	  #ifdef FRAGMENT
	  #version 100

	  #ifdef GL_FRAGMENT_PRECISION_HIGH
		  precision highp float;
	  #else
		  precision mediump float;
	  #endif
	  precision highp int;
	  uniform lowp sampler2D _MainTex;
	  varying mediump vec4 vs_COLOR0;
	  varying highp vec2 vs_TEXCOORD0;
	  #define SV_Target0 gl_FragData[0]
	  vec4 u_xlat0;
	  void main()
	  {
		  u_xlat0 = texture2D(_MainTex, vs_TEXCOORD0.xy);
		  SV_Target0 = u_xlat0 * vs_COLOR0;
		  return;
	  }

	  #endif


	  //////////////////////////////////////////////////////
	  Global Keywords : <none>
	  Local Keywords : <none>
	  --Hardware tier variant : Tier 1
	  --Vertex shader for "gles3" :
	  Set 2D Texture "_MainTex" to slot 0

	  Constant Buffer "$Globals" (144 bytes) {
		Matrix4x4 unity_ObjectToWorld at 0
		Matrix4x4 unity_MatrixVP at 64
		Vector4 _MainTex_ST at 128
	  }

	  Shader Disassembly :
	  #ifdef VERTEX
	  #version 300 es

	  #define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
	  #if HLSLCC_ENABLE_UNIFORM_BUFFERS
	  #define UNITY_UNIFORM
	  #else
	  #define UNITY_UNIFORM uniform
	  #endif
	  #define UNITY_SUPPORTS_UNIFORM_LOCATION 1
	  #if UNITY_SUPPORTS_UNIFORM_LOCATION
	  #define UNITY_LOCATION(x) layout(location = x)
	  #define UNITY_BINDING(x) layout(binding = x, std140)
	  #else
	  #define UNITY_LOCATION(x)
	  #define UNITY_BINDING(x) layout(std140)
	  #endif
	  uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
	  uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
	  uniform 	vec4 _MainTex_ST;
	  in highp vec3 in_POSITION0;
	  in mediump vec4 in_COLOR0;
	  in highp vec3 in_TEXCOORD0;
	  out mediump vec4 vs_COLOR0;
	  out highp vec2 vs_TEXCOORD0;
	  vec4 u_xlat0;
	  vec4 u_xlat1;
	  void main()
	  {
		  vs_COLOR0 = in_COLOR0;
	  #ifdef UNITY_ADRENO_ES3
		  vs_COLOR0 = min(max(vs_COLOR0, 0.0), 1.0);
	  #else
		  vs_COLOR0 = clamp(vs_COLOR0, 0.0, 1.0);
	  #endif
		  vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
		  u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
		  u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
		  u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
		  u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
		  u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
		  u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
		  gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
		  return;
	  }

	  #endif
	  #ifdef FRAGMENT
	  #version 300 es

	  precision highp float;
	  precision highp int;
	  #define UNITY_SUPPORTS_UNIFORM_LOCATION 1
	  #if UNITY_SUPPORTS_UNIFORM_LOCATION
	  #define UNITY_LOCATION(x) layout(location = x)
	  #define UNITY_BINDING(x) layout(binding = x, std140)
	  #else
	  #define UNITY_LOCATION(x)
	  #define UNITY_BINDING(x) layout(std140)
	  #endif
	  UNITY_LOCATION(0) uniform mediump sampler2D _MainTex;
	  in mediump vec4 vs_COLOR0;
	  in highp vec2 vs_TEXCOORD0;
	  layout(location = 0) out mediump vec4 SV_Target0;
	  vec4 u_xlat0;
	  void main()
	  {
		  u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
		  SV_Target0 = u_xlat0 * vs_COLOR0;
		  return;
	  }

	  #endif


	   }
	}
}