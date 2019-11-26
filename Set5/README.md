# Tame of Thrones
My solution to the GeekTrust backend problem set, Tame of Thromes. This is a C# solution containing a single console application for the two seperate problems in the problem set. The user can specify which problem they wish to run by specifying it as a parameter to the console (examples below).

The console application was built using .NET Core 3 and runs on both Windows and Linux.

## Solution status
[![Actions Status](https://github.com/prajnanBhuyan/GeekTrust/workflows/Set%205%20Build%20and%20Test/badge.svg)](https://github.com/prajnanBhuyan/GeekTrust/actions?query=workflow%3A%22Set+5+Build+and+Test%22)

## Prerequisites

You need to have [.NET Core v3.0.0](https://dotnet.microsoft.com/download/dotnet-core/3.0) installed on your system in order to build and run the application.

| OS        |Installers	                    |
|-----------|-------------------------------|
| Linux	    |[Package manager instructions] |
| Windows	|[x64] / [x86]	                |
| All       |[dotnet-install scripts]       



## Building the Application
Use one of the two build scripts  without any paramters to Clean, Build, Test and then Publish the application into a 'Published' folder:<br>
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

After running one of the build scripts you can head to the _Published_ directory to run the application. The console application expects the user to enter wihch problem they wish to run as a parameter. The valid values are **Problem1** and **Problem2** (case insensetive)

### Windows

```
D:\GeekTrust\Set5\src\Build\Published>TameOfThrones.exe Problem1
Who is the ruler of Southeros?
None
Air, "oaaawaala"
...
```

```
D:\GeekTrust\Set5\src\Build\Published>TameOfThrones.exe Problem2
Who is the ruler of Southeros?
None
Allies of Ruler?
...
```

### Linux

```
$ ./TameOfThrones Problem1
Who is the ruler of Southeros?
None
Water, "Go risk it all"
...
```

```
$ ./TameOfThrones Problem2
Who is the ruler of Southeros?
None
Enter the kingdoms competing to be the ruler:
...
```

## User Interaction

### Who is the ruler of Southeros?<br>
The application return the current ruler if it reads *"who is the ruler"* or *"ruler of southeros"* as the input or part of the input.

### Allies of Ruler?<br>
The application return the allies of the current ruler if it reads *"allies of"* as the input or part of the input.

### Sending Messages to Other Kingdoms<br>
In Problem 1, A Golden Crown, the user can send a message to another kingdom at any given point of time but it should be in the form:<br>

```
<recipient_kingdom>"<message_text>"
```

i.e. at the very least, it should contain a valid kingdom name, followed by a message of length greater than zero enclosed by double quotes on both ends.<br>
The *recipient_kingdom* and the *message_text* can be seperated by one or more whitespace character ` ` or comma ` `

These are all valid inputs to send a message:
- Fire, "Drag on Martin!"
- Fire "Drag on Martin!"
- Fire,"Drag on Martin!"
- Fire"Drag on Martin!"
- Fire,   "Drag on Martin!"
- Fire,,, "Drag   " on "   Martin!"

### Entering competing kingdoms<br>
In Problem 2, Breaker of Chains, the application keeps listening for the phrases *"enter"* and *"kingdoms competing"* both (in any order) as the input or part of the input.<br>
On reading an input contain both of the phrases, for example:
```
Enter the kingdoms competing to be the ruler:
```
The application wait for the user to enter the kingdoms that will be competing for the throne **seperated by *space*** (as we shown in the sample input output for the problem)



## Sample Input and Output

### Problem 1

``` Batchfile
Who is the ruler of Southeros?
None
Allies of Ruler?
None

Air, "oaaawaala"
Land, "a1d22n333a4444p"
Ice, "zmzmzmzaztzozh"

Who is the ruler of Southeros?
King Shan
Allies of Ruler?
Air, Land, Ice

Fire, "Ahoy! Fight for me with men and money"

Allies of Ruler?
Air, Land, Ice, Fire

Water, "Go risk it all"

Allies of Ruler?
Air, Land, Ice, Fire

Water, "The quick brown fox jumps over a lazy dog multiple times."

Allies of Ruler?
Air, Land, Ice, Fire, Water
Who is the ruler of Southeros?
King Shan
```

### Problem 2

``` Batchfile
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

6. The ballot process does not start without atleast 2 kingdoms participating.

7. The kingdoms go to war and the ballot is not conducted if all 6 kingdoms try to take part in the process.



[build_ubuntu]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Set5/build.sh
[build_windows]: https://github.com/prajnanBhuyan/GeekTrust/blob/master/Set5/build.ps1
[x64]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.0.101-windows-x64-installer
[x86]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.0.101-windows-x86-installer
[Package manager instructions]: https://dotnet.microsoft.com/download/linux-package-manager/sdk-3.0.101
[dotnet-install scripts]: https://dotnet.microsoft.com/download/dotnet-core/scripts