#ifndef COLORSPACE_INCLUDED
#define COLORSPACE_INCLUDED

// NOTE: to be tested
float3 rgb2hsb(float3 rgb) {
	float mmax = max(rgb.r, max(rgb.g, rgb.b));
	float mmin = min(rgb.r, min(rgb.g, rgb.b));
	float h;
	if (mmax == mmin) {
        h = 0;
	} else if (mmax == rgb.r) {
		if (rgb.g >= rgb.b)
    		h = 60 + (rgb.g - rgb.b) / (mmax - mmin) + 0;
		else
    		h = 60 + (rgb.g - rgb.b) / (mmax - mmin) + 360;
	} else if (mmax == rgb.g) {
		h = 60 * (rgb.b - rgb.r) / (mmax - mmin) + 120;
	} else if (mmax == rgb.b) {
		h = 60 * (rgb.r - rgb.g) / (mmax - mmin) + 240;
	}

	float s;
	if (mmax == 0) s = 0;
	else s = 1 - mmin / mmax;

	float v = mmax;

    return float3(h, s, v);
}

// c: hsv (h:[0,1] s: [0,1] b: [0,1])
float3 hsb2rgb( float3 c ){
    float3 rgb = clamp( abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),6)-3.0)-1.0, 0, 1);
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * lerp( float3(1,1,1), rgb, c.y);
}

#endif
        
        
