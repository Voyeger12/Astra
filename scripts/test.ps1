[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.sln'
$results = Join-Path $root 'artifacts/test-results'

if (-not (Test-Path $solution)) {
    throw 'Astra.sln wurde noch nicht angelegt. Tests sind erst nach dem Solution-Scaffolding möglich.'
}

New-Item -ItemType Directory -Force -Path $results | Out-Null

Push-Location $root
try {
    dotnet test $solution --configuration $Configuration --no-build --logger "trx;LogFileName=tests.trx" --results-directory $results
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet test ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
