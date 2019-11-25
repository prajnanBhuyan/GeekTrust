#!/usr/bin/env bash

dotnet clean -c 'Release' ./src/Set5.sln
dotnet build -c 'Release' ./src/Set5.sln
dotnet test -c 'Release' ./src/Set5.sln
dotnet publish -o ./src/Build/Published -c 'Release' ./src/TameOfThrones/TameOfThrones.csproj --no-build
