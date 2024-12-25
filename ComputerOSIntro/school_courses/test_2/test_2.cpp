#include "test_2.h"
#include "unistd.h"
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>

void onSIGINT(int sig)
{
    printf("Caught signal %d\n", sig);
    printf("Exiting...\n");
    exit(0);
}

int main()
{
    signal(SIGINT, onSIGINT);
    scanf("%d");
}

