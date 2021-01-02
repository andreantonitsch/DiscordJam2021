
#ifndef __MISC_DEFINES_HLSL__
#define __MISC_DEFINES_HLSL__


//Corrects the aspect ration based on height
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

#endif //__MISC_DEFINES_HLSL__
