Shader "Custom/Glas" {
	Properties {
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Cull Off
		ZWrite Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong alpha

		sampler2D _MainTex;

		struct Input {
			float4 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = float3(0.5, 0.5, 0.5);
			o.Alpha = 0.5;
			o.Specular = 0.08;
			o.Gloss = 1.0;
			o.Normal = float4(0.0, 1.0, 0.0, 0.0);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
