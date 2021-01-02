Shader "Unlit/DiffusionReaction2DDraw"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ROWS ("Rows", Int) = 128
        _COLS ("Cols", Int) = 128
        _DrawColor ("DrawColor", Color) = (1,1,0,0)
        _Radius ("Radius", Float) = 0.1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Include/misc_defines.hlsl"

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

            sampler2D _MainTex, _RampTex;
            float4 _MainTex_ST;

            int _ROWS, _COLS;
			float2 _DiffusionRatio, _MousePosition, _DrawColor;
            float _Radius;
		
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float2 frag(v2f i) : SV_Target
            {
                int2 dimensions = int2(_ROWS, _COLS);
                float2 uv = i.uv * aspect_ratio_factor;

				float2 q = tex2D(_MainTex, uv);

                float2 mouse = _MousePosition * aspect_ratio_factor;

                float d = distance(uv, mouse);

				return lerp(q, _DrawColor, saturate((1 / (d*d)) * (d < _Radius)) );

            }


            ENDCG
        }
    }
}
