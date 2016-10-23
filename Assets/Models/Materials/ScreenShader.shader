Shader "Custom/ScreenShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

			_Seed1("Seed1 (RGB)", 2D) = "white" {}
		_Seed2("Seed2 (RGB)", 2D) = "white" {}

		_EffectAmount("Effect Amount", Range(0,2)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _Seed1, _Seed2;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _EffectAmount;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float2 offset = float2(0, 0);

			

			offset = tex2D(_Seed1, IN.uv_MainTex) * 0.1 * _EffectAmount;

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + offset) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			o.Emission = o.Albedo;
			float flashMultiplier = abs(sin(_Time * 800));
			flashMultiplier = clamp(flashMultiplier, 0.72f, 1.25f);

			o.Emission.rgb *= flashMultiplier;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
