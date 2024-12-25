#include <iostream>
#include <unistd.h>
#include <csignal>
#include <sys/wait.h>

pid_t child1, child2;

// 子进程信号处理函数
void child_signal_handler(int signum)
{
    if (getpid() == child1)
    {
        printf("child process1 is killed by parent!\n");
    }
    else if (getpid() == child2)
    {
        printf("child process2 is killed by parent!\n");
    }
    _exit(0); // 子进程退出
}

// 父进程信号处理函数
void parent_signal_handler(int signum)
{
    printf("Parent caught SIGINT, terminating children...\n");
    kill(child1, SIGTERM); // 向子进程1发送终止信号
    kill(child2, SIGTERM); // 向子进程2发送终止信号
}

int main()
{
    // 创建第一个子进程
    child1 = fork();
    if (child1 == 0)
    {
        // 子进程1
        signal(SIGTERM, child_signal_handler); // 捕捉父进程的终止信号
        while (true)
            pause(); // 等待信号
    }
    else if (child1 > 0)
    {
        // 创建第二个子进程
        child2 = fork();
        if (child2 == 0)
        {
            // 子进程2
            signal(SIGTERM, child_signal_handler); // 捕捉父进程的终止信号
            while (true)
                pause(); // 等待信号
        }
        else if (child2 > 0)
        {
            // 父进程
            signal(SIGINT, parent_signal_handler); // 捕捉键盘中断信号 (Ctrl+C)

            // 等待子进程退出
            waitpid(child1, nullptr, 0);
            waitpid(child2, nullptr, 0);

            // 输出父进程终止信息
            printf("parent process is killed!\n");
            return 0;
        }
        else
        {
            fprintf(stderr, "Failed to create child process 2.\n");
            return 1;
        }
    }
    else
    {
        fprintf(stderr, "Failed to create child process 1.\n");
        return 1;
    }
}
