var target = Argument ("target", "Default");

var slnPath = "./src/Set5.sln";
var compiledPath = "./src/Build/Compiled/";
var enginePath = "./src/Engine/Engine.csproj";
var consolePath = "./src/TameOfThrones/TameOfThrones.csproj";
var testPath = "./src/TameOfThrones.Tests/TameOfThrones.Tests.csproj";
var compiledTestsPath = $"{compiledPath}/TameOfThrones.Tests.dll";

Task ("Default")
    .IsDependentOn("Clean")
    .IsDependentOn ("RestoreNuGet")
    .IsDependentOn ("Build")
    .IsDependentOn ("Test");

Task("Clean")
    .Does(() => {
        var settings = new DotNetCoreCleanSettings
        {
            Configuration = "Release"
        };
        DotNetCoreClean(enginePath, settings);
        DotNetCoreClean(consolePath, settings);
        DotNetCoreClean(testPath, settings);
    });

Task("RestoreNuGet")
    .Does(() => {
        DotNetCoreRestore(slnPath);    
    });

Task("Build")
    .Does(() => {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = "Release"
        };
        DotNetCoreBuild(slnPath, settings);
    });

Task ("Test")
    .Does (() => {
        DotNetCoreTest(testPath);
    });

RunTarget (target);