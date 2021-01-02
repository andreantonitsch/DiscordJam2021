Shader "Unlit/DiffusionReaction2DClear"
{
    Properties
    {
        _ClearColor ("ClearColor", Color) = (1,1,0,0)
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

            float2 _ClearColor;
		
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = 0;

                return o;
            }

            float2 frag(v2f i) : SV_Target
            {
                

				return _ClearColor;

            }


            ENDCG
        }
    }
}
