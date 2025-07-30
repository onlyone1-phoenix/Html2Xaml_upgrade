using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Win32;

namespace HTMLConverter
{
    /// <summary>
    /// Professional test interface for HTML to XAML conversion
    /// Original improved by use of Microsoft GitHub Copilot™
    /// </summary>
    public partial class TestWindow : Window
    {
        private TextBox inputBox;
        private TextBox outputBox;
        private Button convertButton;
        private Button clearButton;
        private Button loadSampleButton;
        private Button loadComplexSampleButton;
        private Button exportXamlButton;
        private Button exportFlowDocButton;
        private RadioButton useLegacyConverter;
        private RadioButton useModernConverter;
        private RadioButton useAdvancedConverter;
        private CheckBox preserveLayoutCheckbox;
        private TextBlock statusText;

        // Directory management
        private string HtmlFilesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "html-files");
        private string ConvertedFilesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "converted-files");
        private string SampleFilesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample-files");

        public TestWindow()
        {
            InitializeComponent();
            InitializeDirectories();
            LoadSampleHtml();
        }

        private void InitializeDirectories()
        {
            try
            {
                // Create directories if they don't exist
                Directory.CreateDirectory(HtmlFilesDirectory);
                Directory.CreateDirectory(ConvertedFilesDirectory);
                Directory.CreateDirectory(SampleFilesDirectory);

                // Copy sample files to html-files directory for easy access
                CopySampleFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing directories: {ex.Message}", "Directory Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CopySampleFiles()
        {
            try
            {
                var sampleFiles = new[]
                {
                    "simple-example.html",
                    "modern-example.html",
                    "directory-demo.html"
                };

                foreach (var fileName in sampleFiles)
                {
                    var sourcePath = Path.Combine(SampleFilesDirectory, fileName);
                    var destPath = Path.Combine(HtmlFilesDirectory, fileName);
                    
                    if (File.Exists(sourcePath) && !File.Exists(destPath))
                    {
                        File.Copy(sourcePath, destPath);
                    }
                }
            }
            catch (Exception)
            {
                // Sample file copying failed, continue without copying
            }
        }

        private void InitializeComponent()
        {
            this.Title = "Advanced HTML to XAML Converter Test";
            this.Width = 1200;
            this.Height = 800;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var mainGrid = new Grid();
            
            // Define rows
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Controls
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Input
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Convert button
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Output
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Status

            // Controls panel
            var controlsPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal, 
                Margin = new Thickness(10)
            };
            
            // Converter selection
            var converterGroup = new GroupBox { Header = "Converter", Margin = new Thickness(0, 0, 10, 0) };
            var converterStack = new StackPanel();
            
            useLegacyConverter = new RadioButton { Content = "Legacy (Basic HTML)", Margin = new Thickness(5) };
            useModernConverter = new RadioButton { Content = "Modern (HTML5 + CSS)", Margin = new Thickness(5) };
            useAdvancedConverter = new RadioButton { Content = "Advanced (HTML5 + Tailwind + CSS3)", Margin = new Thickness(5), IsChecked = true };
            
            converterStack.Children.Add(useLegacyConverter);
            converterStack.Children.Add(useModernConverter);
            converterStack.Children.Add(useAdvancedConverter);
            converterGroup.Content = converterStack;
            controlsPanel.Children.Add(converterGroup);
            
            // Options
            var optionsGroup = new GroupBox { Header = "Options", Margin = new Thickness(0, 0, 10, 0) };
            var optionsStack = new StackPanel();
            
            preserveLayoutCheckbox = new CheckBox 
            { 
                Content = "Preserve Layout (Grid)", 
                Margin = new Thickness(5),
                IsChecked = true,
                ToolTip = "Use Grid layout instead of FlowDocument"
            };
            optionsStack.Children.Add(preserveLayoutCheckbox);
            optionsGroup.Content = optionsStack;
            controlsPanel.Children.Add(optionsGroup);
            
            // Sample buttons
            var samplesGroup = new GroupBox { Header = "Input Sources" };
            var samplesStack = new StackPanel { Orientation = Orientation.Horizontal };
            
            // File input button
            var loadFileButton = new Button 
            { 
                Content = "📁 Load HTML File", 
                Margin = new Thickness(5),
                Padding = new Thickness(8, 4, 8, 4),
                Background = System.Windows.Media.Brushes.LightGreen,
                FontWeight = FontWeights.Bold
            };
            loadFileButton.Click += LoadFileButton_Click;
            
            loadSampleButton = new Button 
            { 
                Content = "Load Simple", 
                Margin = new Thickness(5),
                Padding = new Thickness(8, 4, 8, 4),
                ToolTip = "Test code provided by uxpilot.ai"
            };
            loadSampleButton.Click += LoadSampleButton_Click;
            
            loadComplexSampleButton = new Button 
            { 
                Content = "Load Complex (Cosmic)", 
                Margin = new Thickness(5),
                Padding = new Thickness(8, 4, 8, 4),
                ToolTip = "Test code provided by uxpilot.ai"
            };
            loadComplexSampleButton.Click += LoadComplexSampleButton_Click;
            
            samplesStack.Children.Add(loadFileButton);
            samplesStack.Children.Add(loadSampleButton);
            samplesStack.Children.Add(loadComplexSampleButton);
            
            // Directory management buttons
            var openHtmlDirButton = new Button 
            { 
                Content = "📂 HTML Folder", 
                Margin = new Thickness(5),
                Padding = new Thickness(6, 3, 6, 3),
                Background = System.Windows.Media.Brushes.LightBlue,
                FontSize = 11
            };
            openHtmlDirButton.Click += (s, e) => OpenDirectory(HtmlFilesDirectory, "HTML Files");
            
            var openConvertedDirButton = new Button 
            { 
                Content = "📂 Converted", 
                Margin = new Thickness(5),
                Padding = new Thickness(6, 3, 6, 3),
                Background = System.Windows.Media.Brushes.LightBlue,
                FontSize = 11
            };
            openConvertedDirButton.Click += (s, e) => OpenDirectory(ConvertedFilesDirectory, "Converted Files");
            
            samplesStack.Children.Add(openHtmlDirButton);
            samplesStack.Children.Add(openConvertedDirButton);
            samplesGroup.Content = samplesStack;
            controlsPanel.Children.Add(samplesGroup);
            
            Grid.SetRow(controlsPanel, 0);
            mainGrid.Children.Add(controlsPanel);

            // Input textbox with label
            var inputLabel = new Label { Content = "HTML Input:", Margin = new Thickness(10, 5, 10, 0), FontWeight = FontWeights.Bold };
            var inputContainer = new DockPanel { Margin = new Thickness(10, 0, 10, 10) };
            DockPanel.SetDock(inputLabel, Dock.Top);
            
            inputBox = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                FontSize = 12
            };
            
            inputContainer.Children.Add(inputLabel);
            inputContainer.Children.Add(inputBox);
            Grid.SetRow(inputContainer, 1);
            mainGrid.Children.Add(inputContainer);

            // Convert and Clear buttons
            var buttonPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal, 
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10)
            };
            
            convertButton = new Button
            {
                Content = "🚀 Convert to XAML",
                Margin = new Thickness(5),
                Padding = new Thickness(15, 8, 15, 8),
                FontWeight = FontWeights.Bold,
                Background = System.Windows.Media.Brushes.LightBlue
            };
            convertButton.Click += ConvertButton_Click;
            
            clearButton = new Button
            {
                Content = "Clear All",
                Margin = new Thickness(5),
                Padding = new Thickness(15, 8, 15, 8)
            };
            clearButton.Click += ClearButton_Click;
            
            exportXamlButton = new Button
            {
                Content = "💾 Export XAML",
                Margin = new Thickness(5),
                Padding = new Thickness(15, 8, 15, 8),
                Background = System.Windows.Media.Brushes.LightGreen,
                ToolTip = "Export converted XAML to .xaml file"
            };
            exportXamlButton.Click += ExportXamlButton_Click;
            
            exportFlowDocButton = new Button
            {
                Content = "📄 Export FlowDocument",
                Margin = new Thickness(5),
                Padding = new Thickness(15, 8, 15, 8),
                Background = System.Windows.Media.Brushes.LightCoral,
                ToolTip = "Export as FlowDocument XAML (.xaml)"
            };
            exportFlowDocButton.Click += ExportFlowDocButton_Click;
            
            buttonPanel.Children.Add(convertButton);
            buttonPanel.Children.Add(clearButton);
            buttonPanel.Children.Add(exportXamlButton);
            buttonPanel.Children.Add(exportFlowDocButton);
            Grid.SetRow(buttonPanel, 2);
            mainGrid.Children.Add(buttonPanel);

            // Output textbox with label
            var outputLabel = new Label { Content = "XAML Output:", Margin = new Thickness(10, 5, 10, 0), FontWeight = FontWeights.Bold };
            var outputContainer = new DockPanel { Margin = new Thickness(10, 0, 10, 10) };
            DockPanel.SetDock(outputLabel, Dock.Top);
            
            outputBox = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                IsReadOnly = true,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                FontSize = 12,
                Background = System.Windows.Media.Brushes.LightGray
            };
            
            outputContainer.Children.Add(outputLabel);
            outputContainer.Children.Add(outputBox);
            Grid.SetRow(outputContainer, 3);
            mainGrid.Children.Add(outputContainer);

            // Status bar
            statusText = new TextBlock
            {
                Text = "Ready for conversion",
                Margin = new Thickness(10, 5, 10, 5),
                Background = System.Windows.Media.Brushes.LightYellow,
                Padding = new Thickness(10, 5, 10, 5)
            };
            Grid.SetRow(statusText, 4);
            mainGrid.Children.Add(statusText);

            this.Content = mainGrid;
        }

        private void LoadSampleHtml()
        {
            var sampleHtml = @"<div class=""flex flex-col items-center p-8 bg-gray-900 text-white"">
    <h1 class=""text-4xl font-bold mb-6 text-center"">Modern HTML Test</h1>
    <p class=""text-lg text-gray-300 mb-4"">This is a sample with modern CSS and Tailwind classes.</p>
    <button class=""px-6 py-3 bg-blue-500 text-white rounded-lg hover:bg-blue-600"">Click Me</button>
</div>";
            
            inputBox.Text = sampleHtml;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                var htmlInput = inputBox.Text;
                
                if (string.IsNullOrWhiteSpace(htmlInput))
                {
                    outputBox.Text = "Please enter some HTML to convert.";
                    statusText.Text = "❌ No input provided";
                    return;
                }

                string result;
                string conversionInfo;

                if (useAdvancedConverter.IsChecked == true)
                {
                    result = AdvancedHtmlToXamlConverter.ConvertAdvancedHtmlToXaml(htmlInput, preserveLayoutCheckbox.IsChecked == true);
                    conversionInfo = "🚀 Advanced Converter (HTML5 + Tailwind + CSS3)";
                }
                else if (useModernConverter.IsChecked == true)
                {
                    result = ModernHtmlToXamlConverter.ConvertHtmlToXamlAdvanced(htmlInput);
                    conversionInfo = "⚡ Modern Converter (Enhanced HTML5)";
                }
                else
                {
                    result = HtmlToXamlConverter.ConvertHtmlToXaml(htmlInput, false);
                    conversionInfo = "📄 Legacy Converter (Basic HTML)";
                }

                stopwatch.Stop();
                
                outputBox.Text = result;
                
                // Update status
                statusText.Text = $"{conversionInfo} | " +
                                 $"Conversion time: {stopwatch.ElapsedMilliseconds}ms | " +
                                 $"Input: {htmlInput.Length} chars | " +
                                 $"Output: {result.Length} chars";
                
                System.Diagnostics.Debug.WriteLine($"✅ Conversion completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                outputBox.Text = $"❌ Conversion Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}";
                statusText.Text = $"❌ Error occurred during conversion";
                System.Diagnostics.Debug.WriteLine($"❌ Conversion error: {ex.Message}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inputBox.Clear();
            outputBox.Clear();
            statusText.Text = "Ready for conversion";
        }

        private void LoadSampleButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSampleHtml();
            statusText.Text = "Simple sample HTML loaded - test code provided by uxpilot.ai";
        }

        private void LoadComplexSampleButton_Click(object sender, RoutedEventArgs e)
        {
            var complexHtml = @"<main class=""flex-grow overflow-y-auto"">
    <div class=""flex items-center justify-center mx-4 my-8"">
        <div class=""cosmic-bg text-white"">
            <header class=""relative z-50 p-8"">
                <nav class=""flex justify-between items-center max-w-7xl mx-auto"">
                    <div class=""flex items-center space-x-3"">
                        <div class=""w-12 h-12 bg-gradient-to-r from-cyan-400 to-purple-500 rounded-full flex items-center justify-center"">
                            <i class=""fa-solid fa-atom text-white text-xl""></i>
                        </div>
                        <span class=""text-3xl font-bold glow-text tracking-wider"">COSMICA</span>
                    </div>
                    <button class=""px-8 py-3 bg-gradient-to-r from-cyan-500 to-purple-600 rounded-full hover:from-cyan-400 hover:to-purple-500 transition-all font-medium tracking-wide"">
                        ENTER SYSTEM
                    </button>
                </nav>
            </header>
            
            <main class=""relative z-10 flex flex-col items-center justify-center h-[900px] px-6"">
                <div class=""text-center mb-20"">
                    <h1 class=""text-7xl md:text-9xl font-black mb-8 glow-text tracking-wider"">
                        COSMIC
                        <span class=""block bg-gradient-to-r from-cyan-400 via-purple-500 to-indigo-600 bg-clip-text text-transparent cosmic-text"">
                            INTELLIGENCE
                        </span>
                    </h1>
                    <p class=""text-xl md:text-2xl text-gray-300 max-w-4xl mx-auto font-light tracking-wide"">
                        Navigate the infinite possibilities of artificial consciousness through our quantum interface
                    </p>
                </div>
            </main>
            
            <section class=""relative z-10 py-32 px-6"">
                <div class=""max-w-7xl mx-auto"">
                    <h2 class=""text-6xl font-black text-center mb-20 glow-text tracking-wider"">QUANTUM DIMENSIONS</h2>
                    
                    <div class=""grid md:grid-cols-3 gap-12"">
                        <div class=""cosmic-card p-10 rounded-3xl"">
                            <div class=""w-20 h-20 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-2xl flex items-center justify-center mb-8"">
                                <i class=""fa-solid fa-microchip text-3xl text-white""></i>
                            </div>
                            <h3 class=""text-3xl font-bold mb-6 cosmic-text"">NEURAL MATRIX</h3>
                            <p class=""text-gray-300 text-lg leading-relaxed"">Quantum neural networks that transcend traditional computing boundaries</p>
                        </div>
                        
                        <div class=""cosmic-card p-10 rounded-3xl"">
                            <div class=""w-20 h-20 bg-gradient-to-r from-purple-400 to-pink-500 rounded-2xl flex items-center justify-center mb-8"">
                                <i class=""fa-solid fa-eye text-3xl text-white""></i>
                            </div>
                            <h3 class=""text-3xl font-bold mb-6 cosmic-text"">COSMIC VISION</h3>
                            <p class=""text-gray-300 text-lg leading-relaxed"">See beyond dimensions with multiversal pattern recognition</p>
                        </div>
                        
                        <div class=""cosmic-card p-10 rounded-3xl"">
                            <div class=""w-20 h-20 bg-gradient-to-r from-indigo-400 to-cyan-500 rounded-2xl flex items-center justify-center mb-8"">
                                <i class=""fa-solid fa-infinity text-3xl text-white""></i>
                            </div>
                            <h3 class=""text-3xl font-bold mb-6 cosmic-text"">INFINITE SCALE</h3>
                            <p class=""text-gray-300 text-lg leading-relaxed"">Expand across galaxies with unlimited computational power</p>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</main>";
            
            inputBox.Text = complexHtml;
            statusText.Text = "Complex HTML sample loaded (Cosmic theme with Tailwind CSS) - test code provided by uxpilot.ai";
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select HTML File",
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "html",
                Multiselect = false,
                InitialDirectory = HtmlFilesDirectory // Automatically open in html-files directory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var fileContent = File.ReadAllText(openFileDialog.FileName);
                    inputBox.Text = fileContent;
                    
                    var fileName = Path.GetFileName(openFileDialog.FileName);
                    statusText.Text = $"HTML file loaded: {fileName} ({fileContent.Length} characters)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "File Load Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    statusText.Text = "Error loading file";
                }
            }
        }

        private void ExportXamlButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(outputBox.Text))
            {
                MessageBox.Show("No XAML to export. Please convert some HTML first.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Export XAML File",
                    Filter = "XAML Files (*.xaml)|*.xaml|All Files (*.*)|*.*",
                    DefaultExt = "xaml",
                    FileName = GenerateFileName("converted"),
                    InitialDirectory = ConvertedFilesDirectory // Automatically save in converted-files directory
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, outputBox.Text);
                    statusText.Text = $"✅ XAML exported successfully to: {Path.GetFileName(saveFileDialog.FileName)}";
                    
                    // Optionally open the file location
                    var result = MessageBox.Show($"XAML file saved successfully!\n\nFile: {saveFileDialog.FileName}\n\nWould you like to open the folder?", 
                        "Export Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting XAML file:\n{ex.Message}", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "❌ Export failed";
            }
        }

        private void ExportFlowDocButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(outputBox.Text))
            {
                MessageBox.Show("No XAML to export. Please convert some HTML first.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Convert current XAML to FlowDocument format if needed
                var xamlContent = outputBox.Text;
                var flowDocXaml = ConvertToFlowDocumentFormat(xamlContent);

                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Export FlowDocument XAML",
                    Filter = "XAML FlowDocument (*.xaml)|*.xaml|All Files (*.*)|*.*",
                    DefaultExt = "xaml",
                    FileName = GenerateFileName("flowdoc"),
                    InitialDirectory = ConvertedFilesDirectory // Automatically save in converted-files directory
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, flowDocXaml);
                    statusText.Text = $"✅ FlowDocument exported successfully to: {Path.GetFileName(saveFileDialog.FileName)}";
                    
                    var result = MessageBox.Show($"FlowDocument XAML saved successfully!\n\nFile: {saveFileDialog.FileName}\n\nWould you like to open the folder?", 
                        "Export Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting FlowDocument:\n{ex.Message}", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "❌ FlowDocument export failed";
            }
        }

        private string GenerateFileName(string prefix)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var converterType = useAdvancedConverter.IsChecked == true ? "advanced" : 
                               useModernConverter.IsChecked == true ? "modern" : "legacy";
            return $"{prefix}_{converterType}_{timestamp}";
        }

        private string ConvertToFlowDocumentFormat(string xamlContent)
        {
            // If already a FlowDocument, return as-is
            if (xamlContent.Contains("<FlowDocument"))
            {
                return xamlContent;
            }

            // Extract the inner content and wrap in FlowDocument
            var innerContent = xamlContent;
            
            // Remove Grid wrapper if present and extract content
            if (xamlContent.Contains("<Grid") && xamlContent.Contains("</Grid>"))
            {
                var startIndex = xamlContent.IndexOf('>') + 1;
                var endIndex = xamlContent.LastIndexOf("</Grid>");
                if (startIndex < endIndex)
                {
                    innerContent = xamlContent.Substring(startIndex, endIndex - startIndex).Trim();
                }
            }

            // Create FlowDocument wrapper
            var flowDocXaml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<FlowDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
              xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
              xml:space=""preserve"">
{innerContent}
</FlowDocument>";

            return flowDocXaml;
        }

        private void OpenDirectory(string directoryPath, string directoryName)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Process.Start("explorer.exe", directoryPath);
                }
                else
                {
                    MessageBox.Show($"{directoryName} directory not found: {directoryPath}", "Directory Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening {directoryName} directory: {ex.Message}", "Directory Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
