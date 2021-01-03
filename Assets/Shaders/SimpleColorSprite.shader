Shader "Unlit/SimpleColorSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ChannelColor ("Color", Color) = (1,1,1,1)
        _ChannelFocus("Channel Focus", Vector) = (0,0,0,0)
        _Channeling ("Channeling", Float) = 0.0
        _Bands ("Bands", Float) = 0.0
        _TimeScale("Time Scale", Float) = 1.0
        _Bounds ("Smooth bounds", Vector) = (0.7, 1.0, 0, 0)
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
            // make fog work
            #pragma multi_compile_fog

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color, _ChannelColor;

            float2 _ChannelFocus, _Bounds;
            float _Channeling, _TimeScale, _Bands;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;

                float dist =  distance(i.worldPos, _ChannelFocus);

                float val = ((_Time.x * _TimeScale) + dist) % _Bands;


                float smo =  smoothstep(_Bounds.x, _Bounds.y, val);
                return lerp(_Color, lerp(_Color, _ChannelColor, smo), _Channeling);
               // return _Color;
            }
            ENDCG
        }
    }
}
