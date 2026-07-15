[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.sln'

if (-not (Test-Path $solution)) {
    throw 'Astra.sln wurde noch nicht angelegt. Die vollständige Verifikation ist erst nach dem Solution-Scaffolding möglich.'
}

& (Join-Path $PSScriptRoot 'restore.ps1')
& (Join-Path $PSScriptRoot 'build.ps1') -Configuration Release

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

& (Join-Path $PSScriptRoot 'test.ps1') -Configuration Release
