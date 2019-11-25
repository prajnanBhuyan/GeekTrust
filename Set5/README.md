## Tame of Thrones
My solution to the GeekTrust backend problem set, Tame of Thromes. This is a C# solution containing a single console application for the two seperate problems in the problem set. The user can specify which problem they wish to run by specifying it as a parameter to the console (examples below).

The console application was built using .NET Core 3 and runs on both Windows and Linux.

## Solution status
[![Actions Status](https://github.com/prajnanBhuyan/GeekTrust/workflows/Set%205%20Build%20and%20Test/badge.svg)](https://github.com/prajnanBhuyan/GeekTrust/actions?query=workflow%3A%22Set+5+Build+and+Test%22)

## Prerequisites

You need to have [.NET Core v3.0.0](https://dotnet.microsoft.com/download/dotnet-core/3.0) installed on your system in order to build and run the application.

## Build Application
Use one of the two build scripts  without any paramters to Clean, Build, Test and then Publish the application into a 'Published' folder:<br>
 - [`build.ps1`][build_windows] on Windows<br>
 - [`build.sh`][build_ubuntu] on Ubuntu

## Tests
Use the `dotnet` cli's `test` command to run the tests present in the solution.<br>
Run ```dotnet test --help``` for more information.


## How to run?
TODO: Add detail about how to run the application
Clean, build and run tests using the build scripts:
.\build.ps1 with no parameters to Clean, Restore NuGet, Build and run Tests

```D:\GeekTrust\Set5\src\Build\Published>TameOfThrones.exe Problem1```

## Sample Input and Output
TODO: Add sample input output from the terminal

```
Who is the ruler of Southeros?
None
Allies of Ruler?
None
Enter the kingdoms competing to be the ruler:
ice air fire water land
Results after round One ballot count
Allies for Land : 0
Allies for Water : 0
Allies for Ice : 0
Allies for Air : 0
Allies for Fire : 0
Results after round Two ballot count
Allies for Land : 0
Allies for Water : 0
Allies for Ice : 0
Allies for Air : 0
Allies for Fire : 0
...
Results after round Sixteen ballot count
Allies for Land : 1
Allies for Water : 0
Allies for Ice : 0
Allies for Air : 0
Allies for Fire : 0
Who is the ruler of Southeros?
Land
Allies of Ruler?
Space
```

## Assumptions:
TODO: Re-arrange, categorise and expand on assumtions if needed
1. The other kingdoms simply ignore any future messages sent to them once they recieve their first message. This behavious can be easily changed so as to mimic kingdoms getting offended on not having their emblem in the message and breaking off alliances that were already in place.

2. Set a limit for the ballot rounds so that in an unfortunate event we don't go on going forever.

3. A competing kingdom WILL compose a message for another competing kingdom and can posibbly be one of the 6 that the high priest decides to send out as understood by point 2 of **Rules to decide allegiance by a kingdom**:
    *If the receiving kingdom is competing to be the ruler, they will not give their allegiance even if the message they received is correct.*

4. Once a message is sent out, the high priest burns it in the holy flame so as to not send it out a second time by mistake.

5. If three or more competing kingdoms are entered and one of them is invalid, the ballot process is conducted between between the two or more more valid kingdoms.



[build_ubuntu]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Set5/build.sh
[build_windows]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Set5/build.ps1