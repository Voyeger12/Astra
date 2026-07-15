[CmdletBinding()]
param(
    [switch]$UpdateLockFile
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
    $arguments += if ($UpdateLockFile) { '--use-lock-file' } else { '--locked-mode' }
    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        $mode = if ($UpdateLockFile) { 'Lockfile-Aktualisierung' } else { 'Locked Restore' }
        throw "$mode ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
