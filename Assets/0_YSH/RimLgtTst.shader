Shader "Custom/RiimLgtTst" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		//_MainTint("Main Tint", Color) = (0.5,0.5,0.5,0.5);
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Shininess("Shininess", Float) = 10
		_RimColor("Rim Color", Color) = (1.0, 1.0 , 1.0, 1.0)
		_RimPower("Rim Power", RAnge(0.1, 10.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf HalfLambertRim	

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0



		inline fixed4 LightingHalfLambertRim(SurfaceOutput s, fixed3 lightDir, fixed3 veiwDir, fixed atten)
		{
			fixed3 halfVector = normalize(lightDir + veiwDir);

			fixed NdotL = max(0, dot(s.Normal, lightDir));

			//fixed EdotH = max(0, dot(veiwDir, halfVector));
			//fixed NdotH = max(0, dot(s.Normal, halfVector));

			fixed halfLambert = pow((NdotL * 0.5 + 0.5), 2.0);

			fixed4 finalColor;
			finalColor.rgb = halfVector;//fixed3(EdotH, EdotH, EdotH);
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

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
