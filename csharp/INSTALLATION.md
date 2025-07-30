# 🚀 Html2XamlConverter Enhanced - Installation Guide

**Modern HTML to XAML converter with .NET 8 support**  
*Original improved by use of Microsoft GitHub Copilot™*  
*Test code provided by uxpilot.ai*

## ⚡ Quick Install Options

### Option 1: One-Click Windows Installer
```bash
# Download and run the installer
curl -O https://raw.githubusercontent.com/onlyone1-phoenix/Html2Xaml_upgrade/enhanced-converter-v2/install.bat
install.bat
```

### Option 2: PowerShell Installer (Recommended)
```powershell
# Run the PowerShell installer
Invoke-WebRequest -Uri "https://raw.githubusercontent.com/onlyone1-phoenix/Html2Xaml_upgrade/enhanced-converter-v2/install.ps1" -OutFile "install.ps1"
.\install.ps1
```

### Option 3: NuGet Package (For Developers)
```xml
<!-- Add to your .csproj file -->
<PackageReference Include="Html2XamlConverter.Enhanced" Version="2.0.0" />
```

```csharp
// Use in your code
using HTMLConverter;

string xaml = ModernHtmlToXamlConverter.ConvertHtmlToXamlAdvanced(htmlContent);
```

## 📋 Prerequisites

- **Windows 10/11** (for WPF applications)
- **.NET 8 Runtime** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)

## 🎯 Installation Methods

### 🖥️ **Standalone Executable**
Perfect for end users who just want to use the converter:

```bash
# Clone and build
git clone https://github.com/onlyone1-phoenix/Html2Xaml_upgrade.git
cd Html2Xaml_upgrade/csharp
dotnet publish Html2XamlConverter.Standalone.csproj -c Release -o dist --self-contained true
```

**Result**: Single `.exe` file that runs anywhere on Windows!

### 📦 **NuGet Library**
Perfect for developers integrating into their projects:

```bash
# Build the NuGet package
dotnet pack Html2XamlConverter.Modern.csproj -c Release
```

### 🛠️ **Development Mode**
Perfect for contributing or customizing:

```bash
# Run from source
git clone https://github.com/onlyone1-phoenix/Html2Xaml_upgrade.git
cd Html2Xaml_upgrade/csharp
dotnet restore
dotnet build
dotnet run
```

## ✨ Enhanced Features

- **🎨 Modern UI**: Professional file management interface
- **📁 Smart Directories**: Auto-creates `html-files/`, `converted-files/`, `sample-files/`
- **🔧 Triple Engine**: Three conversion engines for different use cases
- **📝 HTML5 Support**: Semantic elements, modern CSS parsing
- **💾 Easy Export**: One-click save with smart file naming
- **📋 Sample Content**: Built-in examples to get started quickly

## 🚀 Usage Examples

### Basic Conversion
```csharp
using HTMLConverter;

// Simple conversion
string htmlContent = "<h1>Hello World</h1><p>This is a test.</p>";
string xaml = ModernHtmlToXamlConverter.ConvertHtmlToXamlAdvanced(htmlContent);
```

### Advanced Features
```csharp
// With CSS support
string htmlWithCss = @"
<style>
.highlight { color: red; font-weight: bold; }
</style>
<div class='highlight'>Styled content</div>";

string xaml = ModernHtmlToXamlConverter.ConvertHtmlToXamlAdvanced(htmlWithCss);
```

### GUI Application
Just run the executable and use the intuitive interface:
1. **Load HTML files** with the file browser
2. **Preview conversion** in real-time
3. **Export XAML** with one click
4. **Manage files** in organized directories

## 🏗️ Build from Source

```bash
# Prerequisites check
dotnet --version  # Should show 8.x.x

# Clone repository
git clone https://github.com/onlyone1-phoenix/Html2Xaml_upgrade.git
cd Html2Xaml_upgrade/csharp

# Restore dependencies
dotnet restore

# Build library version
dotnet build Html2XamlConverter.Modern.csproj -c Release

# Build standalone executable
dotnet publish Html2XamlConverter.Standalone.csproj -c Release -o dist --self-contained true

# Run tests
dotnet test
```

## 📚 Documentation

- **📖 User Guide**: See the built-in help in the application
- **🔧 Developer API**: Check the XML documentation in the code
- **🎯 Examples**: Sample files included in `sample-files/` directory
- **🐛 Issues**: Report at [GitHub Issues](https://github.com/onlyone1-phoenix/Html2Xaml_upgrade/issues)

## 🏷️ Credits

- **Original Project**: Based on the Html2Xaml project by runtu9527
- **Enhanced by**: Microsoft GitHub Copilot™ assistance
- **Test Code**: Provided by uxpilot.ai
- **Modernization**: Upgraded to .NET 8 with enhanced features

## 📄 License

MIT License - Feel free to use, modify, and distribute!

---

**Need help?** Open an issue on GitHub or check the documentation!
