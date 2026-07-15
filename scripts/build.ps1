[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'
$logs = Join-Path $root 'artifacts/logs'
$buildLog = Join-Path $logs 'build.log'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

New-Item -ItemType Directory -Force -Path $logs | Out-Null

Push-Location $root
try {
    dotnet build $solution --configuration $Configuration --no-restore 2>&1 | Tee-Object -FilePath $buildLog
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build ist mit Exitcode $LASTEXITCODE fehlgeschlagen. Siehe $buildLog."
    }
}
finally {
    Pop-Location
}
