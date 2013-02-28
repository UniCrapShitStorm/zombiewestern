Shader "Custom/Glas"
{
	Properties
	{
		_Color ("Main Color", Color) = (0.1,0.1,0.1,0.1)
		_SpecFac ("Spec Fac", Range(0.0, 1.0)) = 0.5
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		
		Pass
		{
			Cull Front
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 200
			
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			
			float4 _Color;
			float4 _SpecFac;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 wpos : TEXCOORD1;
			};
			
			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;
				output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
				output.normal = mul(_Object2World, float4(input.normal.xyz, 0.0));
				output.wpos = mul(_Object2World, input.vertex);
				return output;
			}
			
			float4 frag(vertexOutput input) : COLOR 
			{
				float3 viewdir = normalize(input.wpos.xyz-_WorldSpaceCameraPos.xyz);
				float spec = 1.0-max(0.0, dot(normalize(input.normal), viewdir));
				float4 color = spec*_SpecFac+_Color;
				return color;
			}
			ENDCG
		}
		
		Pass
		{
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			LOD 200
			
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			
			float4 _Color;
			float _SpecFac;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 wpos : TEXCOORD1;
			};
			
			vertexOutput vert(vertexInput input) 
			{
				vertexOutput output;
				output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
				output.normal = mul(_Object2World, float4(input.normal.xyz, 0.0));
				output.wpos = mul(_Object2World, input.vertex);
				return output;
			}
			
			float4 frag(vertexOutput input) : COLOR 
			{
				float3 viewdir = normalize(input.wpos.xyz-_WorldSpaceCameraPos.xyz);
				float spec = 1.0-max(0.0, dot(normalize(input.normal), -viewdir));
				float4 color = spec*_SpecFac+_Color;
				return color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
