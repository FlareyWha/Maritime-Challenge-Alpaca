Shader "AllIn1SpriteLighting/AllIn1SpriteLightingTransparent"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {} //0
		_Color ("Color", Color) = (1,1,1,1) //1
		_Cutoff("Alpha", Range(0,1)) = 1.0 //2
		_LightBoost("Light Multiply", Range(0, 10)) = 1 //3

		_SpecBoost("Specular Boost", Range(0, 10)) = 1 //4
		_SpecularTex ("Specular Map", 2D) = "white" {} //5

		_NormalsTex ("Normals Map", 2D) = "bump" {} //6
		_NormalStrenght("Strength Multiplier", Range(0.1, 10)) = 1 //7

		_ToonShadowThresh("Shadow Threshold", Range(0, 1)) = 0.1 //8
		_ToonShadowSmooth("Shadow Smoothing", Range(0, 1)) = 0.1 //9
		[Header(Only used when Specular is active)]
		_ToonSpecThresh("Specular Threshold", Range(0, 1)) = 0.1 //10

		_GlowColor("Glow Color", Color) = (1,1,1,1) //11
		_GlowAmount("Glow Amount", Range(0,100)) = 10 //12
		_GlowTex("Glow Texture", 2D) = "white" {} //13
		_GlowLit("Glow Illumination", Range(0,1)) = 1 //14

		_OutlineColor("Outline Color", Color) = (1,1,1,1) //15
		_OutlineWidth("Outline Width", Range(0,0.2)) = 0.004
		_OutlineAlpha("Outline Alpha",  Range(0,1)) = 1
		_OutlineGlow("Outline Glow", Range(1,100)) = 1.5
		_OutlineLit("Outline Illumination", Range(0,1)) = 0.4 //19
		[MaterialToggle] _OutlineOnly ( "Outline Only", Float ) = 0 //20

		_FadeTex("Fade Texture", 2D) = "white" {} //21
		_FadeAmount("Fade Amount",  Range(-0.1,1)) = -0.1
		_FadeBurnWidth("Fade Burn Width",  Range(0,1)) = 0.025
		_FadeBurnTransition("Fade Burn Smooth Transition",  Range(0.01,0.5)) = 0.075
		_FadeBurnColor("Fade Burn Color", Color) = (1,1,0,1)
		_FadeBurnTex("Fade Burn Texture", 2D) = "white" {}
		_FadeBurnGlow("Fade Burn Glow",  Range(1,50)) = 2//27

		_HsvShift("Hue Shift", Range(0, 360)) = 180 //28
		_HsvSaturation("Hue Shift Saturation", Range(0, 2)) = 1
		_HsvBright("Hue Shift Bright", Range(0, 2)) = 1 //30

		_HitEffectColor("Hit Effect Color", Color) = (1,1,1,1) //31
		_HitEffectGlow("Hit Effect Glow Intensity", Range(1,100)) = 5
		[Space]
		[Header((Tip) Animate the following property)]
		_HitEffectBlend("Hit Effect Blend", Range(0,1)) = 1 //33

		_ZWrite ("Depth Write", float) = 1 // 34
        _ZTestMode ("Z Test Mode", float) = 4 // 35

		_GrassSpeed("Speed", Range(0,50)) = 2 //36
		_GrassWind("Bend amount", Range(0,50)) = 20
		_GrassRadialBend("Radial Bend", Range(0.0, 5.0)) = 0.1 //38

		_LightAdd("Light Add", Range(-1.0, 1.0)) = 0.0 //39
	}

	SubShader
	{
		Tags{"Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" "IgnoreProjector" = "True"}

		Cull Off
		Lighting On
		ZWrite [_ZWrite]
		ZTest [_ZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Custom addshadow fullforwardshadows alpha:fade

		#pragma shader_feature TOON_ON
		#pragma shader_feature NORMALMAP_ON
		#pragma shader_feature SPECULAR_ON
		#pragma shader_feature GLOW_ON
		#pragma shader_feature OUTLINE_ON
		#pragma shader_feature FADE_ON
		#pragma shader_feature HSV_ON
		#pragma shader_feature HITEFFECT_ON
		#pragma shader_feature WIND_ON

		sampler2D _MainTex;
		half4 _Color;
		half _LightBoost, _Cutoff, _LightAdd;

		#if TOON_ON
		half _ToonShadowThresh, _ToonSpecThresh, _ToonShadowSmooth, _ToonSpecSmooth;
		#endif

		#if NORMALMAP_ON
		sampler2D _NormalsTex;
		half _NormalStrenght;
		#endif

		#if SPECULAR_ON
		sampler2D _SpecularTex;
		half _SpecBoost;
		#endif

		#if GLOW_ON
		sampler2D _GlowTex;
		half4 _GlowColor;
		half _GlowAmount, _GlowLit;
		#endif

		#if OUTLINE_ON
		half4 _OutlineColor;
		half _OutlineWidth, _OutlineAlpha, _OutlineGlow, _OutlineLit, _OutlineOnly;
		#endif

		#if FADE_ON
		sampler2D _FadeTex, _FadeBurnTex;
		half4 _FadeBurnColor;
		half _FadeAmount, _FadeBurnWidth, _FadeBurnTransition, _FadeBurnGlow;
		#endif

		#if COLORSWAP_ON
		sampler2D _ColorSwapTex;
		half4 _ColorSwapRed, _ColorSwapGreen, _ColorSwapBlue;
		half _ColorSwapRedLuminosity, _ColorSwapGreenLuminosity, _ColorSwapBlueLuminosity;
		#endif

		#if HSV_ON
		half _HsvShift, _HsvSaturation, _HsvBright;
		#endif

		#if HITEFFECT_ON
		half4 _HitEffectColor;
		half _HitEffectGlow, _HitEffectBlend;
		#endif

		#if WIND_ON
		half _GrassSpeed, _GrassWind, _GrassRadialBend;
		#endif

		struct Input
		{
			half2 uv_MainTex;
			half4 vertColor : COLOR;
			half3 normal;
		};

		struct SurfaceOutputCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Alpha;
			half2 uv;
			half IsLit;
		};

		half4 LightingCustom(SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten) {
			half NdotL = dot(s.Normal, lightDir) * s.IsLit;

			atten = saturate(_LightAdd + atten);

			#if SPECULAR_ON
			half3 h = normalize(lightDir + viewDir);
			float nh = saturate(dot(s.Normal, h));
			#if TOON_ON
			nh = step(1 - ((1 - _ToonSpecThresh) / 10.0), nh);
			#endif
			float spec = pow(nh, 48.0);
			#endif

			#if TOON_ON
			NdotL = smoothstep(saturate(_ToonShadowThresh - _ToonShadowSmooth), _ToonShadowThresh, NdotL);
			#endif

			half4 c;
			#if SPECULAR_ON
			c.rgb = ((s.Albedo * _LightColor0.rgb * NdotL) + (_LightColor0.rgb * (spec * _SpecBoost * tex2D(_SpecularTex, s.uv).r))) * (atten * _LightBoost);
			#else
			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * _LightBoost;
			#endif
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			#if WIND_ON
			half windOffset = sin(_Time * _GrassSpeed * 10);
			half2 windCenter = float2(0.5, 0.1);
			IN.uv_MainTex.x = fmod(abs(lerp(IN.uv_MainTex.x, IN.uv_MainTex.x + (_GrassWind * 0.01 * windOffset), IN.uv_MainTex.y)), 1);
			half2 delta = IN.uv_MainTex - windCenter;
			half delta2 = dot(delta.xy, delta.xy);
			half2 delta_offset = delta2 * windOffset;
			IN.uv_MainTex = IN.uv_MainTex + half2(delta.y, -delta.x) * delta_offset * _GrassRadialBend;
			#endif

			o.IsLit = 1;
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb * IN.vertColor.rgb;
			o.Alpha = c.a * IN.vertColor.a;

			#if HSV_ON
			half3 resultHsv = half3(o.Albedo);
			half cosHsv = _HsvBright * _HsvSaturation * cos(_HsvShift * 3.14159265 / 180);
			half sinHsv = _HsvBright * _HsvSaturation * sin(_HsvShift * 3.14159265 / 180);
			resultHsv.x = (.299 * _HsvBright + .701 * cosHsv + .168 * sinHsv) * c.x
				+ (.587 * _HsvBright - .587 * cosHsv + .330 * sinHsv) * c.y
				+ (.114 * _HsvBright - .114 * cosHsv - .497 * sinHsv) * c.z;
			resultHsv.y = (.299 * _HsvBright - .299 * cosHsv - .328 * sinHsv) *c.x
				+ (.587 * _HsvBright + .413 * cosHsv + .035 * sinHsv) * c.y
				+ (.114 * _HsvBright - .114 * cosHsv + .292 * sinHsv) * c.z;
			resultHsv.z = (.299 * _HsvBright - .3 * cosHsv + 1.25 * sinHsv) * c.x
				+ (.587 * _HsvBright - .588 * cosHsv - 1.05 * sinHsv) * c.y
				+ (.114 * _HsvBright + .886 * cosHsv - .203 * sinHsv) * c.z;
			o.Albedo = resultHsv;
			#endif

			#if HITEFFECT_ON
			o.Albedo = lerp(o.Albedo, _HitEffectColor.rgb * _HitEffectGlow, _HitEffectBlend);
			o.IsLit = 1 - _HitEffectBlend;
			#endif

			half outlineAlpha = 0;
			#if OUTLINE_ON
			half2 destUv = half2(_OutlineWidth, _OutlineWidth);
			half spriteLeft = tex2D(_MainTex, IN.uv_MainTex + half2(destUv.x, 0)).a;
			half spriteRight = tex2D(_MainTex, IN.uv_MainTex - half2(destUv.x, 0)).a;
			half spriteBottom = tex2D(_MainTex, IN.uv_MainTex + half2(0, destUv.y)).a;
			half spriteTop = tex2D(_MainTex, IN.uv_MainTex - half2(0, destUv.y)).a;
			outlineAlpha = spriteLeft + spriteRight + spriteBottom + spriteTop;
			half spriteTopLeft = tex2D(_MainTex, IN.uv_MainTex + half2(destUv.x, destUv.y)).a;
			half spriteTopRight = tex2D(_MainTex, IN.uv_MainTex + half2(-destUv.x, destUv.y)).a;
			half spriteBotLeft = tex2D(_MainTex, IN.uv_MainTex + half2(destUv.x, -destUv.y)).a;
			half spriteBotRight = tex2D(_MainTex, IN.uv_MainTex + half2(-destUv.x, -destUv.y)).a;
			outlineAlpha = outlineAlpha + spriteTopLeft + spriteTopRight + spriteBotLeft + spriteBotRight;
			outlineAlpha = step(0.05, saturate(outlineAlpha));
			outlineAlpha *= (1 - c.a) *_OutlineAlpha;
			o.Albedo = lerp(o.Albedo, _OutlineColor.rgb * _OutlineGlow, outlineAlpha > 0.1);
			o.Alpha = saturate((c.a * (1 - _OutlineOnly)) + outlineAlpha);
			o.IsLit = lerp(o.IsLit, _OutlineLit, outlineAlpha > 0.1);
			#endif

			#if FADE_ON
			half originalAlpha = c.a;
			half2 tiledUvFade1 = IN.uv_MainTex;
			half2 tiledUvFade2 = IN.uv_MainTex;
			half fadeTemp = tex2D(_FadeTex, tiledUvFade1).r;
			half fade = smoothstep(_FadeAmount + 0.01, _FadeAmount + _FadeBurnTransition, fadeTemp);
			half fadeBurn = smoothstep(_FadeAmount - _FadeBurnWidth, _FadeAmount - _FadeBurnWidth + 0.1, fadeTemp);
			o.Alpha *= fade;
			_FadeBurnColor.rgb *= _FadeBurnGlow;
			o.Albedo += fadeBurn * tex2D(_FadeBurnTex, tiledUvFade2) * _FadeBurnColor * originalAlpha * (1 - o.Alpha);
			#endif

			o.Alpha *= _Cutoff;

			#if GLOW_ON
			half4 emission;
			emission = tex2D(_GlowTex, IN.uv_MainTex);
			emission.rgb *= emission.a * _GlowAmount * _GlowColor * o.Albedo.rgb;
			o.Albedo.rgb += emission.rgb;
			o.IsLit = lerp(o.IsLit, _GlowLit, emission.a);
			#endif

			#if !NORMALMAP_ON
			o.Normal = UnpackNormal(half4(0.5, 0.5, 1, 1));
			#else
			o.Normal = UnpackNormal(tex2D(_NormalsTex, IN.uv_MainTex));
			o.Normal.xy *= _NormalStrenght;
			o.Normal = normalize(o.Normal);
			#if OUTLINE_ON
			o.Normal = lerp(o.Normal, UnpackNormal(half4(0.5, 0.5, 1, 1)), outlineAlpha > 0.1);
			#endif
			#endif

			o.Emission = half3(0,0,0);
			o.uv = IN.uv_MainTex;
		}
		ENDCG
	}
	CustomEditor "SpriteLightingGUI"
	Fallback "Transparent/VertexLit"
}