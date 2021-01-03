Shader "Unlit/DiffusionReaction2DDisplayModified"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RampTex("RampTex", 2D) = "white" {}
        _Q("Data Texture", 2D) = "white" {}
        _Rows ("Rows", Int) = 128
        _Cols ("Cols", Int) = 128
        _ChannelColor("Color", Color) = (1,1,1,1)
        _ChannelFocus("Channel Focus", Vector) = (0,0,0,0)
        _Channeling("Channeling", Float) = 0.0
        _Bands("Bands", Float) = 0.0
        _TimeScale("Time Scale", Float) = 1.0
        _Bounds("Smooth bounds", Vector) = (0.7, 1.0, 0, 0)

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

            #define aspect_ratio_factor  ( _ScreenParams.xy / _ScreenParams.y )

            // Returns (row, col) on xy,  returns cell internal uv on zw
            float4 cell_coordinates(float2 uv, float2 cells)
            {
                float4 coords = 0.0f;
                float2 stretched = uv * cells;

                coords.xy = trunc(stretched);
                coords.zw = frac(stretched);

                return coords;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;

                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _RampTex;
            float4 _MainTex_ST;

            int _Rows, _Cols;

            float4 _Color, _ChannelColor;

            float2 _ChannelFocus, _Bounds;
            float _Channeling, _TimeScale, _Bands;

            sampler2D _Q;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float dist = distance(i.worldPos, _ChannelFocus);

                float val = ((_Time.x * _TimeScale) + dist) % _Bands;


                float smo = smoothstep(_Bounds.x, _Bounds.y, val);
                float4 col = lerp(1, lerp(_Color, _ChannelColor, smo), _Channeling);


                int2 dimensions = int2(_Rows, _Cols);
                float2 uv = i.uv * aspect_ratio_factor;
                float4 cell = cell_coordinates(uv, dimensions);
                fixed4 color = 0.0;

                float f = 1 - tex2D(_Q, i.uv).r ;
                //return 1-f;
                f += .5;
                return tex2D(_RampTex, float2( saturate(f), 0))* col *  (f * f) ;
                //return color / 9;
            }
            ENDCG
        }
    }
}
