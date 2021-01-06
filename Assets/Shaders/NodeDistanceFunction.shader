Shader "Unlit/NodeDistanceFunction"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaxDist("Maximum Distance", Float) = 1.0
        _DistFalloff ("Distance Falloff", Float) = 1.0
        _Domain ("Domain", Vector) = (0,0,1,1)
        _Offset ("Offset", Vector) = (0,0,1,1)
        _ArrayLength ("Array Length", Int) = 120
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

            float _NodePosition[240];
            float _NodeData[120];
            uint _ArrayLength;

            float _MaxDist, _DistFalloff;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Domain;
            float2 _Offset;


            float2 uv2world(float2 uv) 
            {
                float2 pos = uv;

                pos.x = (pos.x * (_Domain.y - _Domain.x)) +  _Domain.x + _Offset.x;
                pos.y = (pos.y *(_Domain.w - _Domain.z)) + _Domain.z + _Offset.y;
                return pos;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p_uv = uv2world(i.uv);

                float val = 0.0f;
                float min_dist = _MaxDist;
                float2 pos = 0.0f;

                for (uint j = 0; j < _ArrayLength; j++)
                {

                    float2 p = float2(_NodePosition[j*2], _NodePosition[j * 2+1]);
                    //float2 p = _NodePosition[j];
                    float scale = _NodeData[j];

                    float n_dist = distance(p, p_uv);
                    float swap = lerp(0, 1, min_dist > n_dist);

                    val = lerp(val, scale, swap);
                    min_dist = lerp(min_dist, n_dist, swap);
                    pos = lerp(pos, p, swap);
                }
                //return  (min_dist / _MaxDist);
                
                min_dist = min(min_dist, _MaxDist);

                min_dist = 1-(min_dist / _MaxDist);
                return min_dist;
                //return float4((pos + 4.5) / 9, 1, 1);
                //return float4((p_uv + 4.5) / 9, 1, 1);
                //return  max(min_dist, _MaxDist);
            }
            ENDCG
        }
    }
}
