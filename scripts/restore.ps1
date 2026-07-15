[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.sln'

if (-not (Test-Path $solution)) {
    throw 'Astra.sln wurde noch nicht angelegt. Restore ist erst nach dem Solution-Scaffolding möglich.'
}

Push-Location $root
try {
    dotnet restore $solution --locked-mode
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet restore ist mit Exitcode $LASTEXITCODE fehlgeschlagen."
    }
}
finally {
    Pop-Location
}
