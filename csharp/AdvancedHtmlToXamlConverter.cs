using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using HtmlAgilityPack;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Css.Parser;
using ExCSS;

namespace HTMLConverter
{
    /// <summary>
    /// Advanced HTML to XAML converter supporting modern web technologies including:
    /// - HTML5 semantic elements
    /// - CSS3 advanced properties
    /// - Tailwind CSS classes
    /// - Complex layouts and animations
    /// - Responsive design patterns
    /// Original improved by use of Microsoft GitHub Copilot™
    /// </summary>
    public static class AdvancedHtmlToXamlConverter
    {
        private static readonly Dictionary<string, string> Html5ElementMappings = new()
        {
            // HTML5 Semantic Elements
            {"main", "Section"},
            {"article", "Section"},
            {"section", "Section"},
            {"header", "Section"},
            {"footer", "Section"},
            {"nav", "Section"},
            {"aside", "Section"},
            {"figure", "Section"},
            {"figcaption", "Paragraph"},
            {"details", "Section"},
            {"summary", "Paragraph"},
            
            // Traditional Elements
            {"div", "Section"},
            {"p", "Paragraph"},
            {"span", "Span"},
            {"strong", "Run"},
            {"b", "Run"},
            {"em", "Run"},
            {"i", "Run"},
            {"u", "Run"},
            {"h1", "Paragraph"},
            {"h2", "Paragraph"},
            {"h3", "Paragraph"},
            {"h4", "Paragraph"},
            {"h5", "Paragraph"},
            {"h6", "Paragraph"},
            {"ul", "List"},
            {"ol", "List"},
            {"li", "ListItem"},
            {"a", "Hyperlink"},
            {"br", "LineBreak"},
            {"hr", "Separator"},
            {"button", "Button"},
            
            // Form Elements
            {"input", "TextBox"},
            {"textarea", "TextBox"},
            {"select", "ComboBox"},
            {"option", "ComboBoxItem"},
            
            // Media Elements (converted to placeholders)
            {"img", "Image"},
            {"video", "MediaElement"},
            {"audio", "MediaElement"},
            {"iframe", "Frame"},
            
            // Table Elements
            {"table", "Table"},
            {"thead", "TableRowGroup"},
            {"tbody", "TableRowGroup"},
            {"tfoot", "TableRowGroup"},
            {"tr", "TableRow"},
            {"td", "TableCell"},
            {"th", "TableCell"}
        };

        private static readonly Dictionary<string, string> TailwindClassMappings = new()
        {
            // Layout
            {"flex", "Orientation=\"Horizontal\""},
            {"flex-col", "Orientation=\"Vertical\""},
            {"grid", "IsItemsHost=\"True\""},
            {"block", "Display=\"Block\""},
            {"inline", "Display=\"Inline\""},
            {"hidden", "Visibility=\"Hidden\""},
            
            // Text
            {"text-center", "TextAlignment=\"Center\""},
            {"text-left", "TextAlignment=\"Left\""},
            {"text-right", "TextAlignment=\"Right\""},
            {"text-justify", "TextAlignment=\"Justify\""},
            {"font-bold", "FontWeight=\"Bold\""},
            {"font-black", "FontWeight=\"Black\""},
            {"font-light", "FontWeight=\"Light\""},
            {"font-medium", "FontWeight=\"Medium\""},
            {"font-semibold", "FontWeight=\"SemiBold\""},
            {"font-thin", "FontWeight=\"Thin\""},
            {"italic", "FontStyle=\"Italic\""},
            {"underline", "TextDecorations=\"Underline\""},
            
            // Colors
            {"text-white", "Foreground=\"White\""},
            {"text-black", "Foreground=\"Black\""},
            {"text-gray-300", "Foreground=\"#D1D5DB\""},
            {"text-gray-400", "Foreground=\"#9CA3AF\""},
            {"text-cyan-400", "Foreground=\"#22D3EE\""},
            {"text-purple-500", "Foreground=\"#A855F7\""},
            {"bg-white", "Background=\"White\""},
            {"bg-black", "Background=\"Black\""},
            {"bg-gray-800", "Background=\"#1F2937\""},
            {"bg-cyan-500", "Background=\"#06B6D4\""},
            {"bg-purple-600", "Background=\"#9333EA\""},
            
            // Spacing
            {"p-4", "Padding=\"4\""},
            {"p-6", "Padding=\"6\""},
            {"p-8", "Padding=\"8\""},
            {"p-10", "Padding=\"10\""},
            {"p-16", "Padding=\"16\""},
            {"p-32", "Padding=\"32\""},
            {"m-4", "Margin=\"4\""},
            {"m-6", "Margin=\"6\""},
            {"m-8", "Margin=\"8\""},
            {"m-16", "Margin=\"16\""},
            {"m-32", "Margin=\"32\""},
            {"mb-8", "Margin=\"0,0,0,8\""},
            {"mb-12", "Margin=\"0,0,0,12\""},
            {"mb-16", "Margin=\"0,0,0,16\""},
            {"mb-20", "Margin=\"0,0,0,20\""},
            {"mt-2", "Margin=\"0,2,0,0\""},
            {"mx-auto", "HorizontalAlignment=\"Center\""},
            
            // Borders
            {"rounded", "CornerRadius=\"4\""},
            {"rounded-lg", "CornerRadius=\"8\""},
            {"rounded-xl", "CornerRadius=\"12\""},
            {"rounded-2xl", "CornerRadius=\"16\""},
            {"rounded-3xl", "CornerRadius=\"24\""},
            {"rounded-full", "CornerRadius=\"9999\""},
            {"border", "BorderThickness=\"1\""},
            {"border-2", "BorderThickness=\"2\""},
            
            // Sizing
            {"w-full", "Width=\"*\""},
            {"h-full", "Height=\"*\""},
            {"w-12", "Width=\"48\""},
            {"h-12", "Height=\"48\""},
            {"w-20", "Width=\"80\""},
            {"h-20", "Height=\"80\""},
            {"max-w-4xl", "MaxWidth=\"896\""},
            {"max-w-7xl", "MaxWidth=\"1280\""}
        };

        public static string ConvertAdvancedHtmlToXaml(string htmlString, bool preserveLayout = true)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"🚀 Starting advanced conversion of HTML: {htmlString.Substring(0, Math.Min(300, htmlString.Length))}...");
                
                // Preprocess and clean HTML
                var processedHtml = PreprocessAdvancedHtml(htmlString);
                System.Diagnostics.Debug.WriteLine($"📋 Preprocessed HTML length: {processedHtml.Length}");
                
                // Parse with HtmlAgilityPack
                var doc = new HtmlDocument();
                doc.LoadHtml(processedHtml);
                
                // Extract CSS and Tailwind classes
                var styleContext = ExtractAdvancedStyles(doc);
                System.Diagnostics.Debug.WriteLine($"🎨 Extracted {styleContext.CssRules.Count} CSS rules and {styleContext.TailwindClasses.Count} Tailwind classes");
                
                // Create XAML document
                var xamlDoc = new XmlDocument();
                var xamlRoot = CreateXamlRoot(xamlDoc, preserveLayout);
                
                // Convert HTML content
                var bodyNode = doc.DocumentNode.SelectSingleNode("//body") ?? 
                              doc.DocumentNode.SelectSingleNode("//main") ?? 
                              doc.DocumentNode;
                              
                ConvertAdvancedNode(bodyNode, xamlRoot, styleContext);
                
                var result = FormatXamlOutput(xamlDoc);
                System.Diagnostics.Debug.WriteLine($"✅ Conversion complete. Output length: {result.Length}");
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Advanced conversion error: {ex.Message}");
                return CreateErrorDocument(ex.Message);
            }
        }

        private static string PreprocessAdvancedHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Remove or simplify problematic elements
            var elementsToRemove = new[] { "script", "style", "noscript", "meta", "link" };
            foreach (var tagName in elementsToRemove)
            {
                var elements = doc.DocumentNode.SelectNodes($"//{tagName}")?.ToList() ?? new List<HtmlNode>();
                foreach (var element in elements)
                {
                    if (tagName == "style")
                    {
                        // Preserve CSS content for processing
                        var cssContent = element.InnerText;
                        if (!string.IsNullOrWhiteSpace(cssContent))
                        {
                            doc.DocumentNode.SetAttributeValue("data-extracted-css", cssContent);
                        }
                    }
                    element.Remove();
                }
            }

            // Convert iframe content if it contains HTML
            var iframes = doc.DocumentNode.SelectNodes("//iframe")?.ToList() ?? new List<HtmlNode>();
            foreach (var iframe in iframes)
            {
                var srcDoc = iframe.GetAttributeValue("srcdoc", "");
                if (!string.IsNullOrWhiteSpace(srcDoc))
                {
                    // Create a placeholder div with the iframe content
                    var placeholder = doc.CreateElement("div");
                    placeholder.SetAttributeValue("class", "iframe-content");
                    placeholder.InnerHtml = System.Net.WebUtility.HtmlDecode(srcDoc);
                    iframe.ParentNode.ReplaceChild(placeholder, iframe);
                }
                else
                {
                    // Create a simple placeholder
                    var placeholder = doc.CreateElement("div");
                    placeholder.SetAttributeValue("class", "iframe-placeholder");
                    placeholder.InnerHtml = $"[Embedded Content: {iframe.GetAttributeValue("title", "iframe")}]";
                    iframe.ParentNode.ReplaceChild(placeholder, iframe);
                }
            }

            // Simplify complex animations and transitions
            var animatedElements = doc.DocumentNode.SelectNodes("//*[@style]")?.ToList() ?? new List<HtmlNode>();
            foreach (var element in animatedElements)
            {
                var style = element.GetAttributeValue("style", "");
                if (style.Contains("animation") || style.Contains("transition"))
                {
                    // Remove animation properties but keep layout and color properties
                    style = Regex.Replace(style, @"animation[^;]*;?", "", RegexOptions.IgnoreCase);
                    style = Regex.Replace(style, @"transition[^;]*;?", "", RegexOptions.IgnoreCase);
                    style = Regex.Replace(style, @"transform[^;]*;?", "", RegexOptions.IgnoreCase);
                    element.SetAttributeValue("style", style);
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private static StyleContext ExtractAdvancedStyles(HtmlDocument doc)
        {
            var context = new StyleContext();
            
            // Extract CSS rules
            var extractedCss = doc.DocumentNode.GetAttributeValue("data-extracted-css", "");
            if (!string.IsNullOrWhiteSpace(extractedCss))
            {
                try
                {
                    var parser = new StylesheetParser();
                    var stylesheet = parser.Parse(extractedCss);
                    
                    foreach (var rule in stylesheet.StyleRules.OfType<ExCSS.StyleRule>())
                    {
                        var selector = rule.SelectorText;
                        var properties = new Dictionary<string, string>();
                        
                        foreach (var declaration in rule.Style)
                        {
                            properties[declaration.Name] = declaration.Value;
                        }
                        
                        context.CssRules[selector] = properties;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"CSS parsing failed: {ex.Message}");
                }
            }

            // Extract Tailwind classes from all elements
            var allElements = doc.DocumentNode.SelectNodes("//*[@class]");
            if (allElements != null)
            {
                foreach (var element in allElements)
                {
                    var classes = element.GetAttributeValue("class", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var className in classes)
                    {
                        if (IsTailwindClass(className))
                        {
                            context.TailwindClasses.Add(className);
                        }
                    }
                }
            }

            return context;
        }

        private static bool IsTailwindClass(string className)
        {
            var tailwindPrefixes = new[] 
            { 
                "flex", "grid", "block", "inline", "hidden",
                "text-", "font-", "bg-", "border-", "rounded",
                "p-", "m-", "w-", "h-", "max-", "min-",
                "space-", "gap-", "items-", "justify-",
                "hover:", "focus:", "active:", "lg:", "md:", "sm:"
            };
            
            return tailwindPrefixes.Any(prefix => className.StartsWith(prefix));
        }

        private static XmlElement CreateXamlRoot(XmlDocument xamlDoc, bool preserveLayout)
        {
            var rootElement = preserveLayout ? "Grid" : "FlowDocument";
            var xamlRoot = xamlDoc.CreateElement(rootElement, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            
            if (preserveLayout)
            {
                xamlRoot.SetAttribute("Background", "#0F172A"); // Dark cosmic background
            }
            
            xamlRoot.SetAttribute("xml:space", "preserve");
            xamlDoc.AppendChild(xamlRoot);
            
            return xamlRoot;
        }

        private static void ConvertAdvancedNode(HtmlNode htmlNode, XmlNode xamlParent, StyleContext styleContext)
        {
            if (htmlNode == null) return;

            System.Diagnostics.Debug.WriteLine($"🔄 Converting: {htmlNode.NodeType} - {htmlNode.Name}");

            switch (htmlNode.NodeType)
            {
                case HtmlNodeType.Text:
                    ConvertTextNode(htmlNode, xamlParent);
                    break;

                case HtmlNodeType.Element:
                    ConvertAdvancedElement(htmlNode, xamlParent, styleContext);
                    break;

                case HtmlNodeType.Document:
                    foreach (var child in htmlNode.ChildNodes)
                    {
                        ConvertAdvancedNode(child, xamlParent, styleContext);
                    }
                    break;
            }
        }

        private static void ConvertTextNode(HtmlNode textNode, XmlNode xamlParent)
        {
            var textContent = textNode.InnerText;
            if (string.IsNullOrWhiteSpace(textContent)) return;

            // Clean up whitespace
            textContent = Regex.Replace(textContent, @"\s+", " ").Trim();
            
            if (textContent.Length > 0)
            {
                var xamlText = xamlParent.OwnerDocument.CreateTextNode(textContent);
                xamlParent.AppendChild(xamlText);
            }
        }

        private static void ConvertAdvancedElement(HtmlNode htmlElement, XmlNode xamlParent, StyleContext styleContext)
        {
            var tagName = htmlElement.Name.ToLower();
            
            // Skip certain elements
            if (ShouldSkipElement(tagName))
            {
                foreach (var child in htmlElement.ChildNodes)
                {
                    ConvertAdvancedNode(child, xamlParent, styleContext);
                }
                return;
            }

            // Map to XAML element
            var xamlElementName = Html5ElementMappings.GetValueOrDefault(tagName, "Section");
            var xamlElement = xamlParent.OwnerDocument.CreateElement(xamlElementName, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            // Apply advanced styling
            ApplyAdvancedStyling(htmlElement, xamlElement, styleContext);

            // Special element handling
            ApplySpecialElementHandling(htmlElement, xamlElement, tagName);

            // Add to parent
            xamlParent.AppendChild(xamlElement);

            // Process children
            foreach (var child in htmlElement.ChildNodes)
            {
                ConvertAdvancedNode(child, xamlElement, styleContext);
            }
        }

        private static void ApplyAdvancedStyling(HtmlNode htmlElement, XmlElement xamlElement, StyleContext styleContext)
        {
            // Apply Tailwind classes
            var classAttr = htmlElement.GetAttributeValue("class", "");
            if (!string.IsNullOrWhiteSpace(classAttr))
            {
                ApplyTailwindClasses(xamlElement, classAttr);
            }

            // Apply inline styles
            var styleAttr = htmlElement.GetAttributeValue("style", "");
            if (!string.IsNullOrWhiteSpace(styleAttr))
            {
                ApplyInlineStyles(xamlElement, styleAttr);
            }

            // Apply CSS rules
            ApplyCssRules(htmlElement, xamlElement, styleContext.CssRules);

            // Apply element-specific defaults
            ApplyElementDefaults(htmlElement, xamlElement);
        }

        private static void ApplyTailwindClasses(XmlElement xamlElement, string classNames)
        {
            var classes = classNames.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var className in classes)
            {
                if (TailwindClassMappings.TryGetValue(className, out var xamlProperty))
                {
                    ApplyXamlProperty(xamlElement, xamlProperty);
                }
                else if (IsTailwindSizeClass(className, out var property, out var value))
                {
                    xamlElement.SetAttribute(property, value);
                }
                else if (IsTailwindColorClass(className, out property, out value))
                {
                    xamlElement.SetAttribute(property, value);
                }
                else if (IsTailwindSpacingClass(className, out property, out value))
                {
                    xamlElement.SetAttribute(property, value);
                }
            }
        }

        private static void ApplyXamlProperty(XmlElement xamlElement, string propertyString)
        {
            if (propertyString.Contains("=\""))
            {
                var parts = propertyString.Split('=');
                if (parts.Length == 2)
                {
                    var property = parts[0].Trim();
                    var value = parts[1].Trim('"');
                    xamlElement.SetAttribute(property, value);
                }
            }
        }

        private static bool IsTailwindSizeClass(string className, out string property, out string value)
        {
            property = "";
            value = "";

            var sizePattern = @"(w|h|text)-(\d+|xs|sm|base|lg|xl|\dxl)";
            var match = Regex.Match(className, sizePattern);
            
            if (match.Success)
            {
                var prefix = match.Groups[1].Value;
                var size = match.Groups[2].Value;
                
                property = prefix switch
                {
                    "w" => "Width",
                    "h" => "Height",
                    "text" => "FontSize",
                    _ => ""
                };
                
                value = ConvertTailwindSize(size, prefix == "text");
                return !string.IsNullOrEmpty(property);
            }
            
            return false;
        }

        private static bool IsTailwindColorClass(string className, out string property, out string value)
        {
            property = "";
            value = "";

            var colorPattern = @"(text|bg)-(red|blue|green|yellow|purple|pink|indigo|cyan|gray|white|black)-(\d{2,3})";
            var match = Regex.Match(className, colorPattern);
            
            if (match.Success)
            {
                var prefix = match.Groups[1].Value;
                var color = match.Groups[2].Value;
                var shade = match.Groups[3].Value;
                
                property = prefix == "text" ? "Foreground" : "Background";
                value = ConvertTailwindColor(color, shade);
                return true;
            }
            
            return false;
        }

        private static bool IsTailwindSpacingClass(string className, out string property, out string value)
        {
            property = "";
            value = "";

            var spacingPattern = @"(p|m|px|py|mx|my|mt|mr|mb|ml)-(\d+)";
            var match = Regex.Match(className, spacingPattern);
            
            if (match.Success)
            {
                var prefix = match.Groups[1].Value;
                var size = match.Groups[2].Value;
                
                property = prefix.StartsWith("p") ? "Padding" : "Margin";
                value = ConvertTailwindSpacing(prefix, size);
                return true;
            }
            
            return false;
        }

        private static string ConvertTailwindSize(string size, bool isText)
        {
            if (isText)
            {
                return size switch
                {
                    "xs" => "12",
                    "sm" => "14",
                    "base" => "16",
                    "lg" => "18",
                    "xl" => "20",
                    "2xl" => "24",
                    "3xl" => "30",
                    "4xl" => "36",
                    "5xl" => "48",
                    "6xl" => "60",
                    "7xl" => "72",
                    "8xl" => "96",
                    "9xl" => "128",
                    _ => int.TryParse(size, out var num) ? (num * 4).ToString() : "16"
                };
            }
            else
            {
                return int.TryParse(size, out var num) ? (num * 4).ToString() : size;
            }
        }

        private static string ConvertTailwindColor(string color, string shade)
        {
            var colorMap = new Dictionary<string, Dictionary<string, string>>
            {
                ["red"] = new() { ["500"] = "#EF4444", ["600"] = "#DC2626", ["400"] = "#F87171" },
                ["blue"] = new() { ["500"] = "#3B82F6", ["600"] = "#2563EB", ["400"] = "#60A5FA" },
                ["green"] = new() { ["500"] = "#10B981", ["600"] = "#059669", ["400"] = "#34D399" },
                ["purple"] = new() { ["500"] = "#8B5CF6", ["600"] = "#7C3AED", ["400"] = "#A78BFA" },
                ["cyan"] = new() { ["500"] = "#06B6D4", ["400"] = "#22D3EE", ["600"] = "#0891B2" },
                ["gray"] = new() { ["300"] = "#D1D5DB", ["400"] = "#9CA3AF", ["800"] = "#1F2937" },
                ["indigo"] = new() { ["500"] = "#6366F1", ["600"] = "#4F46E5", ["400"] = "#818CF8" }
            };

            return colorMap.GetValueOrDefault(color, new Dictionary<string, string>())
                          .GetValueOrDefault(shade, "#000000");
        }

        private static string ConvertTailwindSpacing(string prefix, string size)
        {
            var value = int.TryParse(size, out var num) ? (num * 4).ToString() : "0";
            
            return prefix switch
            {
                "p" or "m" => value,
                "px" => $"{value},{value},0,0",
                "py" => $"0,{value},0,{value}",
                "mx" => $"{value},0,{value},0",
                "my" => $"0,{value},0,{value}",
                "mt" => $"0,{value},0,0",
                "mr" => $"0,0,{value},0",
                "mb" => $"0,0,0,{value}",
                "ml" => $"{value},0,0,0",
                _ => value
            };
        }

        private static void ApplyInlineStyles(XmlElement xamlElement, string styleString)
        {
            var styles = styleString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var style in styles)
            {
                var parts = style.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var property = parts[0].Trim();
                    var value = parts[1].Trim();
                    ApplyCssProperty(xamlElement, property, value);
                }
            }
        }

        private static void ApplyCssProperty(XmlElement xamlElement, string property, string value)
        {
            switch (property.ToLower())
            {
                case "background":
                case "background-color":
                    if (TryParseColor(value, out var bgColor))
                        xamlElement.SetAttribute("Background", bgColor);
                    break;
                    
                case "color":
                    if (TryParseColor(value, out var textColor))
                        xamlElement.SetAttribute("Foreground", textColor);
                    break;
                    
                case "font-size":
                    if (TryParseFontSize(value, out var fontSize))
                        xamlElement.SetAttribute("FontSize", fontSize.ToString());
                    break;
                    
                case "font-weight":
                    var fontWeight = value.ToLower() switch
                    {
                        "bold" or "700" => "Bold",
                        "normal" or "400" => "Normal",
                        "light" or "300" => "Light",
                        "black" or "900" => "Black",
                        _ => "Normal"
                    };
                    xamlElement.SetAttribute("FontWeight", fontWeight);
                    break;
                    
                case "text-align":
                    var alignment = value.ToLower() switch
                    {
                        "center" => "Center",
                        "left" => "Left",
                        "right" => "Right",
                        "justify" => "Justify",
                        _ => null
                    };
                    if (alignment != null)
                        xamlElement.SetAttribute("TextAlignment", alignment);
                    break;
                    
                case "width":
                    if (TryParseSize(value, out var width))
                        xamlElement.SetAttribute("Width", width);
                    break;
                    
                case "height":
                    if (TryParseSize(value, out var height))
                        xamlElement.SetAttribute("Height", height);
                    break;
                    
                case "margin":
                    if (TryParseSpacing(value, out var margin))
                        xamlElement.SetAttribute("Margin", margin);
                    break;
                    
                case "padding":
                    if (TryParseSpacing(value, out var padding))
                        xamlElement.SetAttribute("Padding", padding);
                    break;
            }
        }

        private static void ApplyCssRules(HtmlNode htmlElement, XmlElement xamlElement, Dictionary<string, Dictionary<string, string>> cssRules)
        {
            // Apply rules based on element type, class, and id
            var tagName = htmlElement.Name.ToLower();
            var className = htmlElement.GetAttributeValue("class", "");
            var id = htmlElement.GetAttributeValue("id", "");

            // Element-based rules
            if (cssRules.TryGetValue(tagName, out var elementRules))
            {
                ApplyRuleSet(xamlElement, elementRules);
            }

            // Class-based rules
            if (!string.IsNullOrEmpty(className))
            {
                var classes = className.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var cls in classes)
                {
                    if (cssRules.TryGetValue($".{cls}", out var classRules))
                    {
                        ApplyRuleSet(xamlElement, classRules);
                    }
                }
            }

            // ID-based rules
            if (!string.IsNullOrEmpty(id) && cssRules.TryGetValue($"#{id}", out var idRules))
            {
                ApplyRuleSet(xamlElement, idRules);
            }
        }

        private static void ApplyRuleSet(XmlElement xamlElement, Dictionary<string, string> rules)
        {
            foreach (var rule in rules)
            {
                ApplyCssProperty(xamlElement, rule.Key, rule.Value);
            }
        }

        private static void ApplyElementDefaults(HtmlNode htmlElement, XmlElement xamlElement)
        {
            var tagName = htmlElement.Name.ToLower();
            
            switch (tagName)
            {
                case "h1":
                    xamlElement.SetAttribute("FontSize", "32");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;
                case "h2":
                    xamlElement.SetAttribute("FontSize", "24");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;
                case "strong":
                case "b":
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;
                case "em":
                case "i":
                    xamlElement.SetAttribute("FontStyle", "Italic");
                    break;
                case "button":
                    xamlElement.SetAttribute("Background", "#06B6D4");
                    xamlElement.SetAttribute("Foreground", "White");
                    xamlElement.SetAttribute("Padding", "8,4");
                    break;
            }
        }

        private static void ApplySpecialElementHandling(HtmlNode htmlElement, XmlElement xamlElement, string tagName)
        {
            switch (tagName)
            {
                case "a":
                    var href = htmlElement.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href))
                    {
                        xamlElement.SetAttribute("NavigateUri", href);
                        xamlElement.SetAttribute("Foreground", "#22D3EE");
                        xamlElement.SetAttribute("TextDecorations", "Underline");
                    }
                    break;
                    
                case "img":
                    var src = htmlElement.GetAttributeValue("src", "");
                    var alt = htmlElement.GetAttributeValue("alt", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        xamlElement.SetAttribute("Source", src);
                    }
                    if (!string.IsNullOrEmpty(alt))
                    {
                        xamlElement.SetAttribute("ToolTip", alt);
                    }
                    break;
                    
                case "button":
                    var buttonText = htmlElement.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(buttonText))
                    {
                        xamlElement.SetAttribute("Content", buttonText);
                    }
                    break;
            }
        }

        private static bool ShouldSkipElement(string tagName)
        {
            var skipElements = new HashSet<string>
            {
                "script", "style", "meta", "link", "title", "head", "noscript"
            };
            return skipElements.Contains(tagName);
        }

        private static string FormatXamlOutput(XmlDocument xamlDoc)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            xamlDoc.Save(xmlWriter);
            return stringWriter.ToString();
        }

        private static string CreateErrorDocument(string errorMessage)
        {
            return $@"<FlowDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                <Paragraph Foreground=""Red"" FontWeight=""Bold"">
                    Conversion Error: {System.Security.SecurityElement.Escape(errorMessage)}
                </Paragraph>
            </FlowDocument>";
        }

        #region Helper Methods

        private static bool TryParseColor(string value, out string color)
        {
            color = "Black";

            if (value.StartsWith("#"))
            {
                color = value;
                return true;
            }

            if (value.StartsWith("rgb"))
            {
                var match = Regex.Match(value, @"rgb\((\d+),\s*(\d+),\s*(\d+)\)");
                if (match.Success)
                {
                    var r = int.Parse(match.Groups[1].Value);
                    var g = int.Parse(match.Groups[2].Value);
                    var b = int.Parse(match.Groups[3].Value);
                    color = $"#{r:X2}{g:X2}{b:X2}";
                    return true;
                }
            }

            var namedColors = new Dictionary<string, string>
            {
                {"white", "#FFFFFF"}, {"black", "#000000"}, {"red", "#FF0000"},
                {"green", "#008000"}, {"blue", "#0000FF"}, {"yellow", "#FFFF00"},
                {"cyan", "#00FFFF"}, {"magenta", "#FF00FF"}, {"gray", "#808080"}
            };

            if (namedColors.TryGetValue(value.ToLower(), out color))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseFontSize(string value, out double fontSize)
        {
            fontSize = 12;

            if (value.EndsWith("px"))
            {
                return double.TryParse(value[..^2], out fontSize);
            }
            else if (value.EndsWith("pt"))
            {
                return double.TryParse(value[..^2], out fontSize);
            }
            else if (value.EndsWith("em"))
            {
                if (double.TryParse(value[..^2], out var em))
                {
                    fontSize = em * 16;
                    return true;
                }
            }
            else if (double.TryParse(value, out fontSize))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseSize(string value, out string size)
        {
            size = "Auto";

            if (value.EndsWith("px"))
            {
                size = value[..^2];
                return true;
            }
            else if (value == "100%")
            {
                size = "*";
                return true;
            }
            else if (value == "auto")
            {
                size = "Auto";
                return true;
            }

            return double.TryParse(value, out _);
        }

        private static bool TryParseSpacing(string value, out string spacing)
        {
            spacing = "0";

            var values = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var numbers = new List<string>();

            foreach (var val in values)
            {
                var num = val.Replace("px", "").Replace("em", "").Replace("%", "");
                if (double.TryParse(num, out var result))
                {
                    numbers.Add(result.ToString());
                }
                else
                {
                    numbers.Add("0");
                }
            }

            spacing = numbers.Count switch
            {
                1 => numbers[0],
                2 => $"{numbers[1]},{numbers[0]},{numbers[1]},{numbers[0]}",
                3 => $"{numbers[1]},{numbers[0]},{numbers[1]},{numbers[2]}",
                4 => $"{numbers[3]},{numbers[0]},{numbers[1]},{numbers[2]}",
                _ => "0"
            };

            return true;
        }

        #endregion
    }

    public class StyleContext
    {
        public Dictionary<string, Dictionary<string, string>> CssRules { get; } = new();
        public HashSet<string> TailwindClasses { get; } = new();
    }
}
