var target = Argument ("target", "Default");

var slnPath = "./src/Set5.sln";
var problem1 = "./src/Problem1/Problem1.csproj";
var problem2 = "./src/Problem2/Problem2.csproj";
var testPath = "./src/Set5.Tests/bin/Debug/netcoreapp3.0/Set5.Tests.dll";

Task ("Default")
    .IsDependentOn("Clean")
    .IsDependentOn ("RestoreNuGet")
    .IsDependentOn ("Build")
    .IsDependentOn ("Publish")
    .IsDependentOn ("Test");


Task("RestoreNuGet")
    .Does(() => {
        DotNetCoreRestore(slnPath);    
    });

Task("Build")
    .Does(() => {
        DotNetCoreBuild(slnPath);
    });

Task("Clean")
    .Does(() => {
        DotNetCoreClean(problem1);
        DotNetCoreClean(problem2);
    });

Task ("Publish")
    .Does (() => {
        DotNetCorePublish (problem1);
        DotNetCorePublish (problem2);
    });


Task ("Test")
    .Does (() => {
        DotNetCoreVSTest(testPath);
    });

RunTarget (target);