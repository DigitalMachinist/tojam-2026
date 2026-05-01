#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]$UnityPath = "D:\Program Files\Unity Hub\6000.3.8f1\Editor\Unity.exe",
    [string]$ProjectPath = $PSScriptRoot,
    [ValidateSet('EditMode','PlayMode','All')]
    [string]$Platform = 'All'
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path $UnityPath)) {
    throw "Unity editor not found at: $UnityPath"
}

$running = Get-Process -Name Unity -ErrorAction SilentlyContinue
if ($running) {
    throw "Unity is already running (PID(s): $($running.Id -join ', ')). Close the Editor before running tests -- Unity holds an exclusive lock on the project's Library/ folder, so a second batchmode instance will fail with exit code 1."
}
Write-Host "Reminder: keep the Unity Editor closed while this script runs." -ForegroundColor Yellow

$modes = if ($Platform -eq 'All') { @('EditMode','PlayMode') } else { @($Platform) }
$failed = @()

foreach ($mode in $modes) {
    $results = Join-Path $ProjectPath "test-results-$mode.xml"
    $log = Join-Path $ProjectPath "test-log-$mode.txt"
    Write-Host "Running $mode tests..." -ForegroundColor Cyan

    $args = @(
        '-batchmode', '-nographics',
        '-projectPath', $ProjectPath,
        '-runTests', '-testPlatform', $mode,
        '-testResults', $results,
        '-logFile', $log
    )

    $proc = Start-Process -FilePath $UnityPath -ArgumentList $args -Wait -PassThru -NoNewWindow
    $exit = $proc.ExitCode

    if (Test-Path $log) { Get-Content $log -Tail 50 }

    if ($exit -ne 0) {
        Write-Warning "$mode tests failed (exit $exit). Full log: $log"
        $failed += $mode
    } else {
        Write-Host "$mode tests passed." -ForegroundColor Green
    }
}

if ($failed.Count -gt 0) {
    throw "Test failures in: $($failed -join ', ')"
}
