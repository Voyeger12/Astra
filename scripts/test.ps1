[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'Astra.slnx'
$configurationDirectory = $Configuration.ToLowerInvariant()
$binRoot = Join-Path $root 'artifacts/bin'
$logs = Join-Path $root 'artifacts/logs'
$logFile = Join-Path $logs 'test.log'

if (-not (Test-Path $solution)) {
    throw 'Astra.slnx wurde nicht gefunden.'
}

$testHosts = Get-ChildItem -Path $binRoot -Filter 'Astra.*.Tests.exe' -Recurse -File |
    Where-Object { $_.DirectoryName -like "*$configurationDirectory*" } |
    Sort-Object FullName

if (-not $testHosts) {
    throw "Es wurden keine gebauten MTP-Testhosts unter $binRoot gefunden."
}

New-Item -ItemType Directory -Force -Path $logs | Out-Null
Set-Content -Path $logFile -Value ''

Push-Location $root
try {
    foreach ($testHost in $testHosts) {
        "Running $($testHost.Name)" | Tee-Object -FilePath $logFile -Append
        & $testHost.FullName 2>&1 | Tee-Object -FilePath $logFile -Append
        if ($LASTEXITCODE -ne 0) {
            throw "$($testHost.Name) ist mit Exitcode $LASTEXITCODE fehlgeschlagen. Details: $logFile"
        }
    }
}
finally {
    Pop-Location
}
