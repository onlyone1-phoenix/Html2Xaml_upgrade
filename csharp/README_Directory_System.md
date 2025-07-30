# HTML to XAML Converter - Enhanced Directory Management

## 🎯 Overview

The HTML to XAML Converter now includes intelligent directory management for a seamless workflow experience. Files are automatically organized and dialogs open in the appropriate directories.

## 📁 Directory Structure

The application automatically creates and manages three directories:

### `/html-files/`
- **Purpose**: Store your input HTML files here
- **Usage**: File dialogs automatically open here when loading HTML files
- **Contains**: Your HTML files ready for conversion

### `/converted-files/`
- **Purpose**: All generated XAML files are saved here
- **Usage**: Export dialogs automatically save here
- **Contains**: 
  - `.xaml` files from XAML export
  - `.xaml` FlowDocument files from FlowDocument export

### `/sample-files/`
- **Purpose**: Pre-made example HTML files for testing
- **Usage**: Contains demonstration files
- **Contains**:
  - `simple-example.html` - Basic HTML example
  - `modern-example.html` - Advanced HTML5 + CSS3 example
  - `directory-demo.html` - Directory system demonstration

## 🚀 Enhanced Features

### Automatic Directory Creation
- Directories are created automatically when the application starts
- Sample files are copied to `html-files/` for easy access
- No manual setup required

### Smart File Dialogs
- **Load HTML File**: Opens directly in `html-files/` directory
- **Export XAML**: Saves directly to `converted-files/` directory
- **Export FlowDocument**: Saves directly to `converted-files/` directory

### Quick Access Buttons
- **📂 HTML Folder**: Opens the `html-files/` directory in Windows Explorer
- **📂 Converted**: Opens the `converted-files/` directory in Windows Explorer

## 🛠️ How to Use

1. **Loading Files**:
   - Click "📁 Load HTML File"
   - Dialog opens automatically in `html-files/` directory
   - Select your HTML file

2. **Converting**:
   - Choose your preferred converter (Legacy, Modern, or Advanced)
   - Click "🚀 Convert to XAML"

3. **Exporting Results**:
   - Click "💾 Export XAML" or "📄 Export FlowDocument"
   - Dialog saves automatically to `converted-files/` directory
   - Files are automatically named with timestamp and converter type

4. **Managing Files**:
   - Use "📂 HTML Folder" to quickly access your input files
   - Use "📂 Converted" to view all your generated XAML files

## 📝 File Naming Convention

Exported files use an intelligent naming system:
- Format: `{type}_{converter}_{timestamp}.xaml`
- Examples:
  - `converted_advanced_20250730_235959.xaml`
  - `flowdoc_modern_20250730_235959.xaml`

## 💡 Tips

- Place your HTML files in `html-files/` for quick access
- Use the directory buttons for instant folder navigation
- Sample files are automatically copied for immediate testing
- All exports include optional folder opening for easy file location

This enhanced directory system provides a professional, organized workflow for HTML to XAML conversion projects!
