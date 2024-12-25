#include "test_1.h"
#include <unistd.h>
#include <iostream>

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
        std::cout << "This is the child process" << std::endl;
        std::cout << "Child process ID: " << getpid() << std::endl;
        std::cout << "bbbbbbbbbbbbbbb" << std::endl;
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
            std::cout << "This is the second child process" << std::endl;
            std::cout << "Second child process ID: " << getpid() << std::endl;
            std::cout << "ccccccccccccc" << std::endl;
        }
        else
        {

            std::cout << "This is the parent process" << std::endl;
            std::cout << "Parent process ID: " << getpid() << std::endl;
            std::cout << "aaaaaaaaaaaaaaaa" << std::endl;
        }
    }

    return 0;
}