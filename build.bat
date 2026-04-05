@echo off
setlocal EnableExtensions EnableDelayedExpansion
REM Generate Markdown docs from nd100-definitions (git submodule).
REM Run from anywhere; repo root = directory containing this script.
REM Output: output\docs\

set "REPO=%~dp0"
if "%REPO:~-1%"=="\" set "REPO=%REPO:~0,-1%"
cd /d "%REPO%"

echo === nd100-markdown build (repo: %REPO%) ===

if not exist "%REPO%\nd100-definitions\specs\cpu.yaml" (
  echo Initializing nd100-definitions submodule...
  git -C "%REPO%" submodule update --init --recursive
  if errorlevel 1 exit /b 1
)

where dotnet >nul 2>nul
if errorlevel 1 (
  echo Error: dotnet not found in PATH.
  exit /b 1
)

echo === dotnet build ===
dotnet build "%REPO%\ND100.Markdown.sln" -c Release
if errorlevel 1 exit /b 1

echo === generate Markdown -^> docs ===
dotnet run --project "%REPO%\src\ND100.Markdown.Tool\ND100.Markdown.Tool.csproj" -c Release --no-build
if errorlevel 1 exit /b 1

echo === Done. Output: %REPO%\docs ===
exit /b 0
