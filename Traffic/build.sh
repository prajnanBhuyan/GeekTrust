#!/usr/bin/env bash

write_header()
{
    printf '=%.0s' {1..30}
    printf '\n'
    printf " $1"
    printf '\n'
    printf '=%.0s' {1..30}
    printf '\n'
}

write_header "CLEAN"
dotnet clean -c 'Release' ./src/Traffic.sln

printf '\n'
printf '\n'
write_header "RESTORE"
dotnet restore -s https://api.nuget.org/v3/index.json ./src/Traffic.sln

printf '\n'
printf '\n'
write_header "BUILD"
dotnet build -c 'Release' ./src/Traffic.sln

printf '\n'
printf '\n'
write_header "TEST"
dotnet test -c 'Release' ./src/Traffic.sln

printf '\n'
printf '\n'
write_header "PUBLISH"
dotnet publish -o ./src/Build/Published -c 'Release' ./src/Traffic/Traffic.csproj --no-build

GREEN='\033[0;32m'
printf "\n\n${GREEN}Executable can be found at: ./src/Build/Published/geektrust.dll\n"