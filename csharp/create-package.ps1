# 📦 Create Distribution Package

Write-Host "🚀 Creating Html2XamlConverter Enhanced Distribution Package" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green

# Create distribution directory
$distDir = "dist"
if (Test-Path $distDir) {
    Remove-Item $distDir -Recurse -Force
}
New-Item -ItemType Directory -Path $distDir -Force | Out-Null

# Copy source files for users who want to build themselves
Write-Host "📁 Copying source files..." -ForegroundColor Yellow
$sourceDir = "$distDir\source"
New-Item -ItemType Directory -Path $sourceDir -Force | Out-Null

# Copy essential files
Copy-Item "*.cs" $sourceDir -Force
Copy-Item "*.xaml" $sourceDir -Force -ErrorAction SilentlyContinue
Copy-Item "*.csproj" $sourceDir -Force
Copy-Item "README*.md" $sourceDir -Force -ErrorAction SilentlyContinue
Copy-Item "INSTALLATION.md" $sourceDir -Force -ErrorAction SilentlyContinue
Copy-Item "install.*" $sourceDir -Force -ErrorAction SilentlyContinue

# Copy sample files
if (Test-Path "sample-files") {
    Copy-Item "sample-files" $sourceDir -Recurse -Force
}

# Create a ready-to-run package info
$packageInfo = @"
# Html2XamlConverter Enhanced v2.0
## Distribution Package

### What's Included:
- 📄 Complete source code
- 🎯 Sample HTML files  
- 📝 Installation guides
- 🔧 Project files for .NET 8

### Quick Start:
1. Ensure .NET 8 is installed
2. Run: dotnet restore
3. Run: dotnet build
4. Run: dotnet run

### Features:
✅ Modern .NET 8 support
✅ HTML5 semantic elements
✅ Advanced CSS parsing  
✅ Intelligent directory management
✅ Professional file interface
✅ Sample files included

### Credits:
- Original improved by use of Microsoft GitHub Copilot™
- Test code provided by uxpilot.ai

Repository: https://github.com/onlyone1-phoenix/Html2Xaml_upgrade
"@

$packageInfo | Out-File "$distDir\README.txt" -Encoding UTF8

# Create a simple run script
$runScript = @"
@echo off
echo Starting Html2XamlConverter Enhanced...
echo.

cd source
echo Checking .NET installation...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET 8 Runtime required!
    echo Download from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo Restoring packages...
dotnet restore

echo Building application...
dotnet build -c Release

if %errorlevel% equ 0 (
    echo.
    echo Starting application...
    dotnet run
) else (
    echo Build failed!
    pause
)
"@

$runScript | Out-File "$distDir\RUN.bat" -Encoding ASCII

Write-Host "✅ Distribution package created in 'dist' folder!" -ForegroundColor Green
Write-Host "📦 Contents:" -ForegroundColor Cyan
Write-Host "  • Complete source code"
Write-Host "  • Installation guides" 
Write-Host "  • Sample files"
Write-Host "  • RUN.bat for easy execution"
Write-Host ""
Write-Host "🎯 Users can now:" -ForegroundColor Yellow
Write-Host "  • Download the 'dist' folder"
Write-Host "  • Double-click RUN.bat to start"
Write-Host "  • Or follow the installation guide"
Write-Host ""
Write-Host "📁 Package location: $((Get-Location).Path)\$distDir" -ForegroundColor Cyan
