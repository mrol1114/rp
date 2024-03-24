@echo off

taskkill /f /im dotnet.exe
taskkill /f /im nginx.exe

start wsl redis-cli -p 6379 shutdown