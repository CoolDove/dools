
// to be confirmed
float4 quat_mul(float4 _qa, float4 _qb) {
	float3 va = _qa.xyz;
	float3 vb = _qb.xyz;
	return float4(float3(_qa.w * vb + _qb.w * va + cross(va, vb)), _qa.w * _qb.w - dot(va, vb));
}

float4 quat_axis_angle(float3 _axis, float _radian) {
    float t = sin(_radian * 0.5);
    return float4(_axis.x * t, _axis.y * t, _axis.z * t, cos(_radian * 0.5));
}

float3 quat_apply(float3 v, float4 q) {
    float3 qv = float3(q.x, q.y, q.z);
    float3 t = 2 * cross(qv, v);
    return v + q.w * t + cross(qv, t);
}
