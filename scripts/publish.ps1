[CmdletBinding()]
param(
    [string]$RuntimeIdentifier = 'win-x64',
    [switch]$SelfContained = $true
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$project = Join-Path $root 'src/Astra.Desktop/Astra.Desktop.csproj'

if (-not (Test-Path $project)) {
    throw 'Astra.Desktop.csproj wurde noch nicht angelegt. Publish ist erst nach dem Solution-Scaffolding möglich.'
}

Push-Location $root
try {
    dotnet publish $project --configuration Release --runtime $RuntimeIdentifier --self-contained:$SelfContained --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
