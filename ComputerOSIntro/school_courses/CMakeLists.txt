cmake_minimum_required(VERSION 3.10)

# Set the project name
project(TestProject)

# Add the executable
add_executable(Test1 test_1/test_1.cpp test_1/test_1.h)
add_executable(Test1_Ex test_1_ex/test_1.cpp test_1/test_1.h)
add_executable(Test2 test_2/test_2.cpp test_2/test_2.h)
add_executable(Test3 test_3/test_3.cpp test_3/test_3.h)
add_executable(Test4 test_4/test_4.cpp test_4/test_4.h)

# Include directories
include_directories(${CMAKE_SOURCE_DIR}/test_1)
include_directories(${CMAKE_SOURCE_DIR}/test_1_ex)
include_directories(${CMAKE_SOURCE_DIR}/test_2)
include_directories(${CMAKE_SOURCE_DIR}/test_3)
include_directories(${CMAKE_SOURCE_DIR}/test_4)