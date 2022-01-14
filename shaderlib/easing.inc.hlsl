#ifndef EASING_INCLUDED
#define EASING_INCLUDED

#ifndef PI
#define PI 3.141592653579
#endif

float es_InSine(float x) {return 1 - cos(x * PI / 2);}
float es_OutSine(float x) {return sin(x * PI / 2);}
float es_InOutSine(float x) {return -(cos(PI * x) - 1) / 2;}

float es_InQuad(float x) {return x * x;}
float es_OutQuad(float x) {return 1 - (1 - x) * (1 - x);}
float es_InOutQuad(float x) {return x < 0.5 ? 2 * x * x : 1 - pow(-2 * x + 2, 2) / 2;}

float es_InCubic(float x) {return x * x * x;}
float es_OutCubic(float x) {return 1 - pow(1 - x, 3);}
float es_InOutCubic(float x) {return x < 0.5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2;}

float es_InQuart(float x) {return x * x * x * x;}
float es_OutQuart(float x) {return 1 - pow(1 - x, 4);}
float es_InOutQuart(float x) {return  x < 0.5 ? 8 * x * x * x * x : 1 - pow(-2 * x + 2, 4) / 2;}

float es_InQuint(float x) {return x * x * x * x * x;}
float es_OutQuint(float x) {return 1 - pow(1 - x, 5);}
float es_InOutQuint(float x) {return  x < 0.5 ? 8 * x * x * x * x * x : 1 - pow(-2 * x + 2, 5) / 2;}

float es_InExpo(float x) {return x == 0? 0 : pow(2, 10 * x - 10);}
float es_OutExpo(float x) {return x == 1? 1 : 1 - pow(2, -10 * x);}
float es_InOutExpo(float x) {
	return x == 0? 0 :
    (x == 1?
        1 :
		(x < 0.5?
			pow(2, 20 * x - 10) / 2 :
			(2 - pow(2, -20 * x + 10)) / 2));
}

#endif

