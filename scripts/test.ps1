[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'
$results = Join-Path $root 'artifacts/test-results'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

New-Item -ItemType Directory -Force -Path $results | Out-Null

Push-Location $root
try {
    dotnet test --solution $solution --configuration $Configuration --no-build -- --report-trx --results-directory $results
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet test ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
