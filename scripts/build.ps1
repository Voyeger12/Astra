[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.sln'

if (-not (Test-Path $solution)) {
    throw 'Astra.sln wurde noch nicht angelegt. Build ist erst nach dem Solution-Scaffolding möglich.'
}

Push-Location $root
try {
    dotnet build $solution --configuration $Configuration --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
