[CmdletBinding()]
param(
    [string]$RuntimeIdentifier = 'win-x64',
    [switch]$SelfContained = $true
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'
$project = Join-Path $root 'src/Astra.Desktop/Astra.Desktop.csproj'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

if (-not (Test-Path $project)) {
    throw 'Astra.Desktop.csproj wurde nicht gefunden.'
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
