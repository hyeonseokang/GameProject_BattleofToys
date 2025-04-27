Shader "Custom/RimLightTest" {
	Properties{
		//_Color("Color", Color) = (0,0,0,1)
		//_Glossiness("Smoothness", Range(0,1)) = 0.5
		//_Metallic("Metallic", Range(0,1)) = 0.0
		//_Shininess("Shininess", Float) = 10
		//_RimColor("Rim Color", Color) = (1.0, 1.0 , 1.0, 1.0)
		_RimPower("Rim Power", Range(0.01, 3.0)) = 1.5
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MainTint("Main Tint", Color) = (0.5,0.5,0.5,0.5)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf HalfLambertRim	

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			fixed _RimPower;

		inline fixed4 LightingHalfLambertRim(SurfaceOutput s, fixed3 lightDir, fixed3 veiwDir, fixed atten)
	{
		
		fixed3 halfVector = normalize(lightDir + veiwDir);

		fixed NdotL = max(0, dot(s.Normal, lightDir));

		fixed EdotH = max(0, dot(veiwDir, halfVector));
		fixed NdotH = max(0, dot(s.Normal, halfVector));
		fixed NdotE = max(0, dot(s.Normal, veiwDir));

		fixed halfLambert = pow((NdotL * 0.5 + 0.5), 2.0);

		fixed rimLight = 1 - NdotE;
		rimLight = pow(rimLight, _RimPower) * NdotH;

		fixed4 finalColor;
		finalColor.rgb = (s.Albedo * _LightColor0.rgb + rimLight) * (halfLambert * atten * 2);
		finalColor.a = 0.0;
		return finalColor;
	}

	sampler2D _MainTex;
	fixed4 _MainTint;
	struct Input {
		float2 uv_MainTex;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color

		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb * _MainTint;
		// Metallic and smoothness come from slider variables
		//o.Metallic = _Metallic;
		//o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
