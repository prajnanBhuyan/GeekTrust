function Write-Header([string] $title)
{
    $len = 30

    Write-Host('=' * $len)
    #Write-Host(' ' * [math]::floor(($len - $title.Length)/2) + $title + ' ' * ($len - $title.Length)/2)
    Write-Host(" $title")
    Write-Host('=' * $len)
}

Write-Header("CLEAN")
dotnet clean -c 'Release' ./src/Traffic.sln

Write-Host "`n"
Write-Header("RESTORE")
dotnet restore -s https://api.nuget.org/v3/index.json ./src/Traffic.sln

Write-Host "`n"
Write-Header("BUILD")
dotnet build -c 'Release' ./src/Traffic.sln

Write-Host "`n"
Write-Header("TEST")
dotnet test -c 'Release' ./src/Traffic.sln

Write-Host "`n"
Write-Header("PUBLISH")
dotnet publish -o ./src/Build/Published -c 'Release' ./src/Traffic/Traffic.csproj --no-build

$exePath = Resolve-Path -Path ".\src\Build\Published\geektrust.dll"

Write-Host "`nExecutable can be found at: $exePath" -ForegroundColor Green