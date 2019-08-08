﻿Shader "PA/ParticleField/CutOff" {
	Properties {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture Image", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}
	SubShader {		
	
		Tags{
			"Queue"="AlphaTest"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
		}
		
		Pass {
			ZWrite On
			
			CGPROGRAM
			
			#pragma vertex vertParticleFieldCube
			#pragma fragment frag 
			#pragma target 3.0
			
			
#pragma shader_feature _ DIRECTIONAL_ON SPIN_ON
#pragma shader_feature _ SOFTPARTICLES_ON
#pragma shader_feature _ WORLDSPACE_ON
#pragma shader_feature _ SHAPE_SPHERE SHAPE_CYLINDER
#pragma multi_compile _ EXCLUSION_ON
#pragma shader_feature _ TURBULENCE_SIMPLEX2D TURBULENCE_SIMPLEX	
			
			#include "UnityCG.cginc"
			#include "ParticleField.cginc"
			
			uniform sampler2D _MainTex;			
			fixed4 _Color;
			fixed _CutOff;
			
			fixed _Softness;
			sampler2D _CameraDepthTexture;
			
			float4 frag(v2fParticleField i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.tex.xy) * i.color * _Color;
				
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_Softness * (sceneZ-partZ));
				col.a *= fade;
				#endif
				
				clip (col.a - _CutOff);
				return col;   
			}
		
			ENDCG
		}
	}
}