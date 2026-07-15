[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'
$configurationDirectory = $Configuration.ToLowerInvariant()
$testHost = Join-Path $root "artifacts/bin/Astra.Architecture.Tests/$configurationDirectory/Astra.Architecture.Tests.exe"
$logs = Join-Path $root 'artifacts/logs'
$logFile = Join-Path $logs 'test.log'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

if (-not (Test-Path $testHost)) {
    throw "Der gebaute MTP-Testhost wurde nicht gefunden: $testHost"
}

New-Item -ItemType Directory -Force -Path $logs | Out-Null

Push-Location $root
try {
    & $testHost 2>&1 |
        Tee-Object -FilePath $logFile
    if ($LASTEXITCODE -ne 0) {
        throw "Der Architekturtest-Host ist mit Exitcode $LASTEXITCODE fehlgeschlagen. Details: $logFile"
    }
}
finally {
    Pop-Location
}
