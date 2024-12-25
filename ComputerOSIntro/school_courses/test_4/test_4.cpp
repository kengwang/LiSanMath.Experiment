#include <iostream>
#include <unordered_map>
#include <vector>

// 页表条目结构体
struct PageTableEntry
{
    int pageNumber;  // 页号
    int frameNumber; // 帧号
    bool valid;      // 有效位，标记页是否在内存中
};

class PagingSystem
{
private:
    std::unordered_map<int, PageTableEntry> pageTable; // 页表
    int pageSize;                                      // 页大小（字节）
    int memorySize;                                    // 物理内存大小（字节）
    int frameCount;                                    // 物理帧总数

public:
    // 构造函数
    PagingSystem(int pageSize, int memorySize) : pageSize(pageSize), memorySize(memorySize)
    {
        frameCount = memorySize / pageSize;
    }

    // 添加页表条目
    void addPageTableEntry(int pageNumber, int frameNumber, bool valid)
    {
        if (frameNumber >= frameCount)
        {
            std::cerr << "Error: Frame number exceeds physical memory limit." << std::endl;
            return;
        }
        pageTable[pageNumber] = {pageNumber, frameNumber, valid};
    }

    // 地址转换：逻辑地址 -> 物理地址
    int translateAddress(int logicalAddress)
    {
        int pageNumber = logicalAddress / pageSize; // 计算页号
        int offset = logicalAddress % pageSize;     // 计算页内偏移量

        // 查找页表
        if (pageTable.find(pageNumber) == pageTable.end() || !pageTable[pageNumber].valid)
        {
            std::cout << "* Page " << pageNumber << " - Page fault occurred!" << std::endl;
            return -1; // 模拟页错误
        }

        int frameNumber = pageTable[pageNumber].frameNumber;
        int physicalAddress = frameNumber * pageSize + offset; // 计算物理地址
        return physicalAddress;
    }

    // 模拟指令执行
    void executeInstructions(const std::vector<int> &logicalAddresses)
    {
        for (int logicalAddress : logicalAddresses)
        {
            std::cout << "Logical address: " << logicalAddress << std::endl;
            int physicalAddress = translateAddress(logicalAddress);
            if (physicalAddress != -1)
            {
                std::cout << "Physical address: " << physicalAddress << std::endl;
            }
        }
    }
};

int main()
{
    // 示例：设置页大小为 1024 字节，物理内存大小为 16 KB
    PagingSystem pagingSystem(1024, 16 * 1024);

    // 手动添加页表条目（页号 -> 帧号，是否有效）
    pagingSystem.addPageTableEntry(0, 2, true);  // 页 0 -> 帧 2
    pagingSystem.addPageTableEntry(1, 5, true);  // 页 1 -> 帧 5
    pagingSystem.addPageTableEntry(2, 7, false); // 页 2 -> 帧 7 (无效，模拟缺页)

    // 示例指令逻辑地址列表
    std::vector<int> instructions = {2048, 4096, 3072, 1024};

    // 执行指令
    pagingSystem.executeInstructions(instructions);

    return 0;
}
