#ifndef DMATH_INCLUDED
#define DMATH_INCLUDED

// < Vector >
float3 vec_proj(float3 v, float3 n) {
	float cos_theta = dot(normalize(v), normalize(n));
	return (v * cos_theta) * normalize(n);
}
float2 vec_proj(float2 v, float2 n) {
	float cos_theta = dot(normalize(v), normalize(n));
	return (v * cos_theta) * normalize(n);
}

// < Plane >


// < Quaternion >
#include "quaternion.inc.hlsl"


#endif
