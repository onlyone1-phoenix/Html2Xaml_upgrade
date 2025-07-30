@echo off
REM Html2XamlConverter Enhanced - Windows Installer
REM Original improved by use of Microsoft GitHub Copilot™

echo ================================
echo Html2XamlConverter Enhanced
echo ================================
echo.

echo Checking for .NET 8 Runtime...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET Runtime not found!
    echo Please install .NET 8 Runtime from:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo .NET Runtime found!
echo.

echo Building standalone executable...
if exist "Html2XamlConverter.Standalone.csproj" (
    dotnet publish Html2XamlConverter.Standalone.csproj -c Release -o "dist\standalone" --self-contained true
    
    if %errorlevel% equ 0 (
        echo.
        echo SUCCESS: Build completed!
        echo Executable location: dist\standalone\Html2XamlConverter.exe
        echo.
        echo You can now copy the .exe file anywhere and run it!
        echo.
        
        set /p choice="Open the output folder? (y/n): "
        if /i "%choice%"=="y" (
            explorer "dist\standalone"
        )
    ) else (
        echo ERROR: Build failed!
    )
) else (
    echo ERROR: Project file not found!
)

echo.
echo Features included:
echo - Modern .NET 8 support
echo - HTML5 semantic elements  
echo - Advanced CSS parsing
echo - Intelligent directory management
echo - Professional file input/output
echo.
echo Documentation: https://github.com/onlyone1-phoenix/Html2Xaml_upgrade
echo.
pause
