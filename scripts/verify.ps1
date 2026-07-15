[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

& (Join-Path $PSScriptRoot 'restore.ps1')

Push-Location $root
try {
    dotnet format $solution --verify-no-changes --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet format ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}

& (Join-Path $PSScriptRoot 'build.ps1') -Configuration Release
& (Join-Path $PSScriptRoot 'test.ps1') -Configuration Release
