[CmdletBinding()]
param(
    [switch]$Locked
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

Push-Location $root
try {
    $arguments = @('restore', $solution)
    $arguments += if ($Locked) { '--locked-mode' } else { '--use-lock-file' }
    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet restore ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
