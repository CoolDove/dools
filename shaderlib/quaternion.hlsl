#ifndef QUATERNION_INCLUDED
#define QUATERNION_INCLUDED

// NOTE: rotate with qb first, then qa
float4 quat_mul(float4 _qa, float4 _qb) {
	float3 va = _qa.xyz;
	float3 vb = _qb.xyz;
	float scalar = _qa.w * _qb.w - dot(_qa, _qb);
	float3 vec = _qa.w * vb + _qb.w * va + cross(va, vb);
	return float4(vec, scalar);
}
float4 quat_mul(float4 _qa, float3 _qb) {
	return quat_mul(_qa, float4(_qb, 0));
}
float4 quat_mul(float3 _qa, float4 _qb) {
	return quat_mul(float4(_qa, 0), _qb);
}

float quat_mag(float4 _q) {
	return sqrt(_q.x * _q.x + _q.y * _q.y + _q.z * _q.z + _q.w * _q.w);
}

float4 quat_conjugate(float4 q) {
	return float4(-q.x, -q.y, -q.z, q.w);
}

float4 quat_neg(float4 q) {
	float nmag = 1.0 / quat_mag(q);
	return quat_conjugate(q) * nmag;
}

// -----------------

float3 quat_apply(float3 v, float4 q) {
	return quat_mul(quat_mul(q, v), quat_neg(q));
}

float4 quat_axisangle(float3 _axis, float _radian) {
    float t = sin(_radian * 0.5);
    return float4(_axis.x * t, _axis.y * t, _axis.z * t, cos(_radian * 0.5));
}

float4 quat_vectovec(float3 va, float3 vb) {
	float3 na = normalize(va);
	float3 nb = normalize(vb);
	float3 axis = cross(na, nb);
	float radian = acos(dot(na, nb));
	return normalize(quat_axisangle(axis, radian));
}

// TODO: confirm this
float4 quat_slerp(float4 qa, float4 qb, float interp) {
	float theta = acos(dot(qa, qb));
	return (sin(theta - interp) / sin(theta)) * qa + (sin(theta * t) / sin(theta)) * qb;
}

// FIXME:
float4 quat_lookat(float3 _fwd, float3 _up) {
	float3 targetup = cross(-cross(_up, _fwd), _fwd);
	float4 qa = quat_vectovec(float3(0, 0, 1), _fwd);
	float3 rotatedup = quat_apply(_up, qa);
	float4 qb = quat_vectovec(rotatedup, targetup);

    return normalize(quat_mul(qb, qa));
}

#endif
