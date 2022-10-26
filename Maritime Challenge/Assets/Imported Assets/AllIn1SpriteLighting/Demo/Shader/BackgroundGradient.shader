Shader "AllIn1SpriteLighting/BackgroundGradient"
{
    Properties
    {
		_ColorTop("Color Top", Color) = (1,0,0,1)
		_ColorBot("Color Bot", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Geometry-250" }
        ZWrite Off
		ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _ColorTop, _ColorBot;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				return lerp(_ColorBot, _ColorTop, i.uv.y);
            }
            ENDCG
        }
    }
}
