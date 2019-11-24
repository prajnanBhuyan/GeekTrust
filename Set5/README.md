## Tame of Thrones
My solution to the GeekTrust backend problem set Tame of Thromes. This is a C# solution containing a single console application for the two seperate problems in the problem set. The user can specify which problem they wish to run by specifying it as a parameter to the console (examples below).

The console application was built using .NET Core 3 and runs on both Windows and Linux.

## Solution status
[![Actions Status](https://github.com/prajnanBhuyan/GeekTrust/workflows/Set%205%20Build%20and%20Test/badge.svg)](https://github.com/prajnanBhuyan/GeekTrust/actions?query=workflow%3A%22Set+5+Build+and+Test%22)

## Framework used
- [.NET Core 3](https://dotnet.microsoft.com/download/dotnet-core/3.0) to create the application 
- [Cake Build](https://cakebuild.net/) to create the build scripts

## Installation
TODO: Add a definite list of prerequisites withh download links

Prerequisites:
[.NET Core 3](https://dotnet.microsoft.com/download/dotnet-core/3.0)

## Tests
TODO: Describe how to run tests using build script/vstest.console.exe/mstest.exe
.\build.ps1 -Target Test

## How to run?
TODO: Add detail about how to run the application
Using the build scripts:
.\build.ps1 with no parameters to Clean, Restore NuGet, Build and run Tests

## Sample Input and Output
TODO: Add sample input output from the terminal

## Assumptions:
TODO: Re-arrange, categorise and expand on assumtions if needed
1. The other kingdoms simply ignore any future messages sent to them once they recieve their first message. This behavious can be easily changed so as to mimic kingdoms getting offended on not having their emblem in the message and breaking off alliances that were already in place.

2. Set a limit for the ballot rounds so that in an unfortunate event we don't go on going forever.

3. A competing kingdom WILL compose a message for another competing kingdom and can posibbly be one of the 6 that the high priest decides to send out as understood by point 2 of **Rules to decide allegiance by a kingdom**:
    *If the receiving kingdom is competing to be the ruler, they will not give their allegiance even if the message they received is correct.*

4. Once a message is sent out, the high priest burns it in the holy flame so as to not send it out a second time by mistake.

5. If three or more competing kingdoms are entered and one of them is invalid, the ballot process is conducted between between the two or more more valid kingdoms.