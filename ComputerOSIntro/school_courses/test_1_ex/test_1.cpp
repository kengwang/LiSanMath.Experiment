#include "test_1.h"
#include <unistd.h>
#include <iostream>

#define WITH_LOCK

int main()
{
    pid_t pid = fork();
    pid_t pid2;

    if (pid < 0)
    {
        std::cerr << "Fork failed" << std::endl;
        return 1;
    }
    else if (pid == 0)
    {
#ifdef WITH_LOCK
        lockf(1, 1, 0);
#endif
        std::cout << "This is the child process" << std::endl;
        std::cout << "Child process ID: " << getpid() << std::endl;
        for (size_t i = 0; i < 10; i++)
        {
            std::cout << "child1" << std::endl;
        }
#ifdef WITH_LOCK
        lockf(1, 0, 0);
#endif
    }
    else
    {
        pid2 = fork();
        if (pid2 < 0)
        {
            std::cerr << "Fork failed" << std::endl;
            return 1;
        }
        else if (pid2 == 0)
        {
#ifdef WITH_LOCK
            lockf(1, 1, 0);
#endif
            std::cout << "This is the second child process" << std::endl;
            std::cout << "Second child process ID: " << getpid() << std::endl;
            for (size_t i = 0; i < 10; i++)
            {
                std::cout << "child2" << std::endl;
            }
#ifdef WITH_LOCK
            lockf(1, 0, 0);
#endif
        }
        else
        {
#ifdef WITH_LOCK
            lockf(1, 1, 0);
#endif
            std::cout << "This is the parent process" << std::endl;
            std::cout << "Parent process ID: " << getpid() << std::endl;
            for (size_t i = 0; i < 10; i++)
            {
                std::cout << "parent" << std::endl;
            }
            std::cout << "parent end" << std::endl;
#ifdef WITH_LOCK
            lockf(1, 0, 0);
#endif
        }
    }

    return 0;
}