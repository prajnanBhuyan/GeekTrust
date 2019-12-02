# Traffic :rocket:
My solution to the GeekTrust backend problem set, Traffic. This is a C# solution containing a single console application that reads input from text file and writes output to the console. The path to the text file needs to be provided as an argument to the console application when called.

The console application was built using .NET Core 2.2 and runs on both Windows and Linux.

## Solution status
[![Actions Status](https://github.com/prajnanBhuyan/GeekTrust/workflows/Traffic%20Build%20and%20Test/badge.svg)](https://github.com/prajnanBhuyan/GeekTrust/actions?query=workflow%3A%22Traffic+Build+and+Test%22)

## Assumptions
1. The number of craters will always be an integer. So, a 10% increase in 15 craters would be 16.5 craters, but since that is not possible in the real world, we select the greatest integer less than or equal to it, i.e. 16.