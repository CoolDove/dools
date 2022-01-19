#ifndef DATASTRUCT
#define DATASTRUCT

struct RingLinkNode
{
	int prev;
	int ;
	int next;
};

#define RLink(type) RLink##type
#define DefineRLink(type) struct RLink##type {int prev; int next; type value;};



#endif
