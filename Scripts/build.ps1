#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]$UnityPath = "D:\Program Files\Unity Hub\6000.3.8f1\Editor\Unity.exe",
    [string]$ProjectPath = (Split-Path $PSScriptRoot -Parent),
    [string]$BuildRoot = (Join-Path (Split-Path $PSScriptRoot -Parent) 'Builds')
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path $UnityPath)) {
    throw "Unity editor not found at: $UnityPath"
}

$running = Get-Process -Name Unity -ErrorAction SilentlyContinue
if ($running) {
    throw "Unity is already running (PID(s): $($running.Id -join ', ')). Close the Editor before building -- Unity holds an exclusive lock on the project's Library/ folder, so a second batchmode instance will fail with exit code 1."
}
Write-Host "Reminder: keep the Unity Editor closed while this script runs." -ForegroundColor Yellow

$timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$buildName = "tojam-2026-WebGL-$timestamp"
$buildPath = Join-Path $BuildRoot $buildName
$log = Join-Path $ProjectPath "build-log-WebGL-$timestamp.txt"

if (-not (Test-Path $BuildRoot)) {
    New-Item -ItemType Directory -Path $BuildRoot | Out-Null
}

Write-Host "Building WebGL -> $buildPath" -ForegroundColor Cyan

$args = @(
    '-batchmode', '-nographics', '-quit',
    '-projectPath', $ProjectPath,
    '-buildTarget', 'WebGL',
    '-executeMethod', 'WebGLBuilder.Build',
    '-customBuildPath', $buildPath,
    '-logFile', $log
)

$proc = Start-Process -FilePath $UnityPath -ArgumentList $args -Wait -PassThru -NoNewWindow
$exit = $proc.ExitCode

if (Test-Path $log) { Get-Content $log -Tail 50 }

if ($exit -ne 0) {
    throw "WebGL build failed (exit $exit). Full log: $log"
}

Write-Host "WebGL build completed: $buildPath" -ForegroundColor Green
