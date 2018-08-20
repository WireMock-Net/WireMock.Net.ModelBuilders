@echo off

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

dotnet packages\build\fake-cli\tools\netcoreapp2.1\any\fake-cli.dll run build.fsx %*
