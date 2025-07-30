# Html2XamlConverter Enhanced - Easy Installer
# Original improved by use of Microsoft GitHub Copilot™

Write-Host "🚀 Html2XamlConverter Enhanced Installer" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green

# Check if .NET 8 is installed
Write-Host "📋 Checking prerequisites..." -ForegroundColor Yellow

try {
    $dotnetVersion = dotnet --version 2>$null
    if ($dotnetVersion -and $dotnetVersion.StartsWith("8.")) {
        Write-Host "✅ .NET 8 Runtime found: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "❌ .NET 8 Runtime not found!" -ForegroundColor Red
        Write-Host "📥 Please install .NET 8 Runtime from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "❌ .NET Runtime not found!" -ForegroundColor Red
    Write-Host "📥 Please install .NET 8 Runtime from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    exit 1
}

# Installation options
Write-Host ""
Write-Host "🔧 Installation Options:" -ForegroundColor Cyan
Write-Host "1. Install as NuGet Package (for developers)"
Write-Host "2. Build Standalone Executable (single .exe file)"
Write-Host "3. Run from Source (development mode)"

$choice = Read-Host "Select option (1-3)"

switch ($choice) {
    "1" {
        Write-Host "📦 Installing as NuGet Package..." -ForegroundColor Yellow
        Write-Host "Use this command in your project:" -ForegroundColor Cyan
        Write-Host "Install-Package Html2XamlConverter.Enhanced" -ForegroundColor White
        Write-Host ""
        Write-Host "Or add to your .csproj:" -ForegroundColor Cyan
        Write-Host '<PackageReference Include="Html2XamlConverter.Enhanced" Version="2.0.0" />' -ForegroundColor White
    }
    
    "2" {
        Write-Host "🔨 Building standalone executable..." -ForegroundColor Yellow
        
        if (Test-Path "Html2XamlConverter.Standalone.csproj") {
            dotnet publish Html2XamlConverter.Standalone.csproj -c Release -o "dist\standalone" --self-contained true
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✅ Build successful!" -ForegroundColor Green
                Write-Host "📁 Executable location: dist\standalone\Html2XamlConverter.exe" -ForegroundColor Cyan
                Write-Host "💡 You can copy this .exe file anywhere and run it!" -ForegroundColor Yellow
            } else {
                Write-Host "❌ Build failed!" -ForegroundColor Red
            }
        } else {
            Write-Host "❌ Standalone project file not found!" -ForegroundColor Red
        }
    }
    
    "3" {
        Write-Host "🛠️ Running from source..." -ForegroundColor Yellow
        
        if (Test-Path "TestWindow.cs") {
            Write-Host "📋 Restoring packages..." -ForegroundColor Yellow
            dotnet restore
            
            Write-Host "🔨 Building application..." -ForegroundColor Yellow
            dotnet build -c Release
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "▶️ Running application..." -ForegroundColor Green
                dotnet run
            } else {
                Write-Host "❌ Build failed!" -ForegroundColor Red
            }
        } else {
            Write-Host "❌ Source files not found!" -ForegroundColor Red
        }
    }
    
    default {
        Write-Host "❌ Invalid option selected!" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "🎯 Features included:" -ForegroundColor Green
Write-Host "  • Modern .NET 8 support"
Write-Host "  • HTML5 semantic elements"
Write-Host "  • Advanced CSS parsing"
Write-Host "  • Intelligent directory management"
Write-Host "  • Professional file input/output"
Write-Host "  • Sample files and documentation"
Write-Host ""
Write-Host "📚 Documentation: https://github.com/onlyone1-phoenix/Html2Xaml_upgrade" -ForegroundColor Cyan
Write-Host "🏷️ Original improved by use of Microsoft GitHub Copilot™" -ForegroundColor Gray
Write-Host "🏷️ Test code provided by uxpilot.ai" -ForegroundColor Gray
