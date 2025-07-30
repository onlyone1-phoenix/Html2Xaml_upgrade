# 🚀 HTML to XAML Converter - Production Ready

A professional-grade HTML to XAML converter with modern web technology support, intelligent directory management, and three conversion engines.

*Original improved by use of Microsoft GitHub Copilot™*

## ✨ Features

### 🎯 Triple Converter Architecture
- **Legacy Converter**: Original Microsoft converter for basic HTML
- **Modern Converter**: Enhanced HTML5 + CSS support
- **Advanced Converter**: Complete modern web stack with Tailwind CSS

### 📁 Intelligent Directory Management
- **html-files/**: Input HTML files with auto-open dialogs
- **converted-files/**: Exported XAML files with smart naming
- **sample-files/**: Pre-made examples for testing

### 🛠️ Professional Features
- **File Import**: Load HTML files with smart directory navigation
- **Export Options**: XAML and FlowDocument export formats
- **Auto-naming**: Timestamp-based file naming with converter type
- **Quick Access**: Directory buttons for instant folder opening

## 🚀 Quick Start

1. **Run the Application**:
   ```bash
   dotnet run --project converthtml-modern.csproj
   ```

2. **Load HTML**:
   - Click "📁 Load HTML File" (opens in html-files/)
   - Or use sample files with "Load Simple" / "Load Complex"

3. **Convert**:
   - Choose converter type (Advanced recommended)
   - Click "🚀 Convert to XAML"

4. **Export**:
   - Click "💾 Export XAML" or "📄 Export FlowDocument"
   - Files save automatically to converted-files/

## 📋 System Requirements

- **.NET 8.0** or higher
- **Windows** with WPF support
- **Visual Studio 2022** or VS Code (recommended)

## 📦 Dependencies

```xml
<PackageReference Include="HtmlAgilityPack" Version="1.11.61" />
<PackageReference Include="AngleSharp" Version="0.17.1" />
<PackageReference Include="ExCSS" Version="4.2.3" />
```

## 🏗️ Project Structure

```
csharp/
├── converthtml-modern.csproj    # Modern .NET 8 project
├── App.cs                       # Application entry point
├── TestWindow.cs                # Main UI with directory management
├── HtmlToXamlConverter.cs       # Legacy Microsoft converter
├── ModernHtmlToXamlConverter.cs # Modern HTML5 + CSS converter
├── AdvancedHtmlToXamlConverter.cs # Advanced Tailwind + CSS3 converter
├── sample-files/                # Example HTML files
├── html-files/                  # User HTML input files (auto-created)
└── converted-files/             # Generated XAML output (auto-created)
```

## 🎨 Supported Technologies

### HTML5 Elements
- Semantic elements: `<main>`, `<article>`, `<section>`, `<header>`, `<footer>`
- Traditional elements: `<div>`, `<p>`, `<span>`, `<h1-h6>`, `<ul>`, `<ol>`, `<li>`
- Text formatting: `<strong>`, `<em>`, `<b>`, `<i>`, `<u>`

### CSS3 Properties
- Typography: `font-size`, `font-weight`, `color`
- Layout: `margin`, `text-align`, `background-color`
- Modern features: Gradients, box-shadow (partial support)

### Tailwind CSS Classes
- 50+ utility classes supported
- Responsive design prefixes
- Color palette and spacing system
- Typography and layout utilities

## 💡 Usage Examples

### Basic HTML Conversion
```html
<div class="text-center">
    <h1>Welcome</h1>
    <p style="color: blue;">Modern HTML to XAML</p>
</div>
```

### Advanced with Tailwind
```html
<main class="flex-grow overflow-y-auto">
    <div class="bg-gradient-to-r from-cyan-400 to-purple-500">
        <h1 class="text-4xl font-bold text-white">Cosmic Design</h1>
    </div>
</main>
```

## 🔧 Development

### Building
```bash
dotnet build converthtml-modern.csproj
```

### Running
```bash
dotnet run --project converthtml-modern.csproj
```

### Testing
Load sample files and test all three converters with various HTML structures.

## 📄 File Naming Convention

Exported files use intelligent naming:
- `converted_advanced_20250730_235959.xaml`
- `flowdoc_modern_20250730_235959.xaml`

Format: `{type}_{converter}_{timestamp}.xaml`

## 🎯 Production Ready

✅ **Clean Architecture**: Organized code structure  
✅ **Error Handling**: Graceful failure handling  
✅ **User Experience**: Intuitive interface design  
✅ **File Management**: Professional directory organization  
✅ **Performance**: Optimized conversion algorithms  

## 🤝 Contributing

This is a production-ready HTML to XAML converter. The codebase is clean, well-structured, and ready for professional use or further development.

## 🙏 Credits

- **Original Project**: Based on the Html2Xaml converter
- **Enhancement Development**: Original improved by use of Microsoft GitHub Copilot™
- **Sample Code**: Test code provided by uxpilot.ai

---

**Ready to convert HTML to professional XAML!** 🎉
