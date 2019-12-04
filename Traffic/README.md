# Traffic :rocket:
My solution to the GeekTrust backend problem set, Traffic. This is a C# solution containing a single console application that reads input from text file and writes output to the console. The path to the text file needs to be provided as an argument to the console application when called.

The console application was built using .NET Core 2.2 and runs on both Windows and Linux.

## Solution status
[![Actions Status](https://github.com/prajnanBhuyan/GeekTrust/workflows/Traffic%20Build%20and%20Test/badge.svg)](https://github.com/prajnanBhuyan/GeekTrust/actions?query=workflow%3A%22Traffic+Build+and+Test%22)

## Prerequisites

You need to have [.NET Core v2.2.0](https://dotnet.microsoft.com/download/dotnet-core/2.2) installed on your system in order to build and run the application.

| OS        |Installers	                    |
|-----------|-------------------------------|
| Linux	    |[Package manager instructions] |
| Windows	|[x64] / [x86]	                |
| All       |[dotnet-install scripts]       |



## Building the Application
Use one of the two build scripts  without any parameters to Clean, Build, Test and then Publish the application into a 'Published' folder:<br>
 - [`build.ps1`][build_windows] on Windows<br>
 - [`build.sh`][build_ubuntu] on Ubuntu

## Running the Tests
Use the `dotnet` cli's `test` command to run the tests present in the solution.<br>
```
Usage: dotnet test [options] <PROJECT | SOLUTION> [[--] <RunSettings arguments>...]]
```
If a project or solution file is not specified to operate on, the command will search the current directory for one.<br>
Run ```dotnet test --help``` for more information.

## Running the Application

After running one of the build scripts you can head to the _Published_ directory to run the application. The console application expects the user to enter which problem they wish to run as a parameter. The valid values are **Problem1** and **Problem2** (case-insensitive)

### Windows

```
D:\Geek Trust\Traffic\src\Build\Published>dotnet geektrust.dll "D:\Geek Trust\Traffic\sample-io\input1.txt"
CAR ORBIT2
```

### Linux

```
$ dotnet geektrust.dll /home/pb/GeekTrust/Traffic/sample-io/input1.txt
CAR ORBIT2
```


## Assumptions
1. The number of craters will always be an integer. So, a 10% increase in 15 craters would be 16.5 craters, but since that is not possible in the real world, we select the greatest integer less than or equal to it, i.e. 16.


[build_ubuntu]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Traffic/build.sh
[build_windows]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Traffic/build.ps1
[x64]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-2.2.100-windows-x64-installer
[x86]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-2.2.100-windows-x86-installer
[Package manager instructions]: https://docs.microsoft.com/dotnet/core/install/linux-package-managers
[dotnet-install scripts]: https://dotnet.microsoft.com/download/dotnet-core/scripts