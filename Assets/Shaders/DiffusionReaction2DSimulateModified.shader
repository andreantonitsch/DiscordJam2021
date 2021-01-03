Shader "Unlit/DiffusionReaction2DSimulationModified"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InfluenceTex ("Influence Texture", 2D) = "white" {}
        _ROWS ("Rows", Int) = 128
        _COLS ("Cols", Int) = 128
		_DiffusionRatio ("DiffusionRatio", Vector) = (1,1,1,1)
		_F ("F", Float) = 0.04
		_K ("K", Float) = 0.04
		_DELTATIME ("DeltaTime", Float) = 0.5

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
			#define aspect_ratio_factor  ( _ScreenParams.xy / _ScreenParams.y )

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

			// Returns (row, col) on xy,  returns cell internal uv on zw
			float4 cell_coordinates(float2 uv, float2 cells)
			{
				float4 coords = 0.0f;
				float2 stretched = uv * cells;

				coords.xy = trunc(stretched);
				coords.zw = frac(stretched);

				return coords;
			}

            sampler2D _MainTex, _InfluenceTex;
            float4 _MainTex_ST;

            int _ROWS, _COLS;
			float2 _DiffusionRatio;
			float _F, _K, _DELTATIME;

			float2 laplacian(uint2 id)
			{
				int2 cross_neighbors[4] = { int2(-1,-1), int2(1,1), int2(-1,1), int2(1,-1) };
				int2 orto_neighbors[4] = { int2(1,0), int2(-1,0), int2(0,1), int2(0,-1) };

				int2 id_ = int2(id);
				int2 aspect = int2(_ROWS, _COLS);
				float2 middle = 0.5f;
				float2 sum = tex2D(_MainTex, (id + middle) / (aspect));
				
				sum = -sum;

				for (int i = 0; i < 4; i++)
				{
					int2 n = cross_neighbors[i];
					n.x = min(max(0, id_.x + n.x), aspect.x);
					n.y = min(max(0, id_.y + n.y), aspect.y);

					float2 q = tex2D(_MainTex, (n + middle) / (aspect));

					sum += 0.05 * q;
				}

				for (i = 0; i < 4; i++)
				{
					int2 n = orto_neighbors[i];
					n.x = min(max(0, id_.x + n.x), aspect.x);
					n.y = min(max(0, id_.y + n.y), aspect.y);

					float2 q = tex2D(_MainTex, (n + middle) / (aspect));

					sum += 0.2 * q;
				}

				return sum;

			}
            
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
                float4 cell = cell_coordinates(uv, dimensions);
				float2 middle = 0.5f;

				float2 q = tex2D(_MainTex, (cell.xy + middle) / dimensions);
				float influence = tex2D(_InfluenceTex, (cell.xy + middle) / dimensions);

				float2 nabla_sq = laplacian(cell.xy);

				float dA = _DiffusionRatio.x;
				float dB = _DiffusionRatio.y;
				float a = q.x;
				float b = q.y;

				float b2 = b * b;

				float fa = _F * (1.0f - a) * lerp(0.85, 1.05, influence);
				float kb = (_K + _F) * b;

				float2 q_prime = 0;

				q_prime.x = q.x + (dA * nabla_sq.x - a * b2 + fa) * _DELTATIME;
				q_prime.y = q.y + (dB * nabla_sq.y + a * b2 - kb) * _DELTATIME;

				return q_prime;

            }


            ENDCG
        }
    }
}
