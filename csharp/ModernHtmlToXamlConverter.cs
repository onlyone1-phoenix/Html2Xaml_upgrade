using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Modern HTML to XAML converter supporting HTML5, CSS, and complex structures
    /// Original improved by use of Microsoft GitHub Copilot™
    /// </summary>
    public static class ModernHtmlToXamlConverter
    {
        private static readonly Dictionary<string, string> ElementMappings = new()
        {
            // HTML5 Semantic Elements
            {"main", "Section"},
            {"article", "Section"},
            {"section", "Section"},
            {"header", "Section"},
            {"footer", "Section"},
            {"nav", "Section"},
            {"aside", "Section"},
            
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
            {"hr", "LineBreak"}
        };

        private static readonly Dictionary<string, System.Windows.FontWeight> FontWeightMappings = new()
        {
            {"bold", FontWeights.Bold},
            {"bolder", FontWeights.Black},
            {"lighter", FontWeights.Light},
            {"normal", FontWeights.Normal},
            {"100", FontWeights.Thin},
            {"200", FontWeights.ExtraLight},
            {"300", FontWeights.Light},
            {"400", FontWeights.Normal},
            {"500", FontWeights.Medium},
            {"600", FontWeights.SemiBold},
            {"700", FontWeights.Bold},
            {"800", FontWeights.ExtraBold},
            {"900", FontWeights.Black}
        };

        public static string ConvertHtmlToXamlAdvanced(string htmlString, bool asFlowDocument = true)
        {
            try
            {
                // Clean and preprocess HTML
                var cleanedHtml = PreprocessHtml(htmlString);
                
                // Parse HTML with HtmlAgilityPack for better HTML5 support
                var doc = new HtmlDocument();
                doc.LoadHtml(cleanedHtml);
                
                // Extract and parse CSS
                var cssRules = ExtractCssRules(doc);
                
                // Create XAML document
                var xamlDoc = new XmlDocument();
                var rootElement = asFlowDocument ? "FlowDocument" : "Section";
                var xamlRoot = xamlDoc.CreateElement(rootElement, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                xamlRoot.SetAttribute("xml:space", "preserve");
                xamlDoc.AppendChild(xamlRoot);

                // Convert HTML body or document
                var bodyNode = doc.DocumentNode.SelectSingleNode("//body") ?? doc.DocumentNode;
                
                ConvertHtmlNodeToXaml(bodyNode, xamlRoot, cssRules);
                
                var result = xamlDoc.OuterXml;
                
                return result;
            }
            catch (Exception ex)
            {
                return $"<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph>Conversion failed: {ex.Message}</Paragraph></FlowDocument>";
            }
        }

        private static string PreprocessHtml(string html)
        {
            // Remove script and style content but keep structure for analysis
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Remove script content but keep tags for structure
            foreach (var script in doc.DocumentNode.SelectNodes("//script")?.ToList() ?? new List<HtmlNode>())
            {
                script.InnerHtml = "";
            }

            // Extract text content from style tags for potential CSS parsing
            var styleContents = new List<string>();
            foreach (var style in doc.DocumentNode.SelectNodes("//style")?.ToList() ?? new List<HtmlNode>())
            {
                if (!string.IsNullOrWhiteSpace(style.InnerText))
                {
                    styleContents.Add(style.InnerText);
                }
                style.Remove();
            }

            // Store CSS for later processing
            if (styleContents.Any())
            {
                doc.DocumentNode.SetAttributeValue("data-extracted-css", string.Join("\n", styleContents));
            }

            return doc.DocumentNode.OuterHtml;
        }

        private static Dictionary<string, Dictionary<string, string>> ExtractCssRules(HtmlDocument doc)
        {
            var cssRules = new Dictionary<string, Dictionary<string, string>>();
            
            try
            {
                var extractedCss = doc.DocumentNode.GetAttributeValue("data-extracted-css", "");
                if (!string.IsNullOrWhiteSpace(extractedCss))
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
                        
                        cssRules[selector] = properties;
                    }
                }
            }
            catch
            {
                // CSS parsing failed, continue without CSS rules
            }

            return cssRules;
        }

        private static void ConvertHtmlNodeToXaml(HtmlNode htmlNode, XmlNode xamlParent, Dictionary<string, Dictionary<string, string>> cssRules)
        {
            if (htmlNode == null) return;

            switch (htmlNode.NodeType)
            {
                case HtmlNodeType.Text:
                    var textContent = htmlNode.InnerText;
                    
                    if (!string.IsNullOrWhiteSpace(textContent))
                    {
                        // Preserve whitespace but clean up excessive whitespace
                        textContent = System.Text.RegularExpressions.Regex.Replace(textContent, @"\s+", " ");
                        
                        var textNode = xamlParent.OwnerDocument.CreateTextNode(textContent);
                        xamlParent.AppendChild(textNode);
                    }
                    break;

                case HtmlNodeType.Element:
                    ConvertHtmlElement(htmlNode, xamlParent, cssRules);
                    break;

                case HtmlNodeType.Document:
                    // Process children of document node
                    foreach (var child in htmlNode.ChildNodes)
                    {
                        ConvertHtmlNodeToXaml(child, xamlParent, cssRules);
                    }
                    break;

                default:
                    // Skip other node types
                    break;
            }
        }

        private static void ConvertHtmlElement(HtmlNode htmlElement, XmlNode xamlParent, Dictionary<string, Dictionary<string, string>> cssRules)
        {
            var tagName = htmlElement.Name.ToLower();
            
            // Skip unsupported or problematic elements
            if (ShouldSkipElement(tagName))
            {
                // Process children of skipped elements
                foreach (var child in htmlElement.ChildNodes)
                {
                    ConvertHtmlNodeToXaml(child, xamlParent, cssRules);
                }
                return;
            }

            // Map HTML element to XAML element
            if (!ElementMappings.TryGetValue(tagName, out var xamlElementName))
            {
                // Default to Span for unknown inline elements, Section for unknown block elements
                xamlElementName = IsBlockElement(tagName) ? "Section" : "Span";
            }

            // Create XAML element
            var xamlElement = xamlParent.OwnerDocument.CreateElement(xamlElementName, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            // Apply styling
            ApplyElementStyling(htmlElement, xamlElement, cssRules);

            // Special handling for specific elements
            if (tagName == "a")
            {
                var href = htmlElement.GetAttributeValue("href", "");
                if (!string.IsNullOrWhiteSpace(href))
                {
                    xamlElement.SetAttribute("NavigateUri", href);
                }
            }

            // Add to parent first
            xamlParent.AppendChild(xamlElement);

            // Special handling for list items
            if (tagName == "li" && xamlElementName == "ListItem")
            {
                // Create a paragraph wrapper for list item content
                var listItemParagraph = xamlParent.OwnerDocument.CreateElement("Paragraph", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                xamlElement.AppendChild(listItemParagraph);
                
                // Process children into the paragraph
                foreach (var child in htmlElement.ChildNodes)
                {
                    ConvertHtmlNodeToXaml(child, listItemParagraph, cssRules);
                }
                return; // Skip normal child processing
            }

            // Process children normally
            foreach (var child in htmlElement.ChildNodes)
            {
                ConvertHtmlNodeToXaml(child, xamlElement, cssRules);
            }
        }

        private static bool ShouldSkipElement(string tagName)
        {
            var skipElements = new HashSet<string>
            {
                "script", "style", "meta", "link", "title", "head",
                "iframe", // Skip complex embedded content
                "object", "embed", "video", "audio" // Skip multimedia
            };
            return skipElements.Contains(tagName);
        }

        private static bool IsBlockElement(string tagName)
        {
            var blockElements = new HashSet<string>
            {
                "div", "p", "h1", "h2", "h3", "h4", "h5", "h6",
                "main", "article", "section", "header", "footer", "nav", "aside",
                "ul", "ol", "li", "table", "tr", "td", "th", "thead", "tbody", "tfoot",
                "blockquote", "pre", "hr"
            };
            return blockElements.Contains(tagName);
        }

        private static void ApplyElementStyling(HtmlNode htmlElement, XmlElement xamlElement, Dictionary<string, Dictionary<string, string>> cssRules)
        {
            // Apply direct style attributes
            var styleAttr = htmlElement.GetAttributeValue("style", "");
            if (!string.IsNullOrWhiteSpace(styleAttr))
            {
                ApplyInlineStyles(xamlElement, styleAttr);
            }

            // Apply CSS class styles
            var classAttr = htmlElement.GetAttributeValue("class", "");
            if (!string.IsNullOrWhiteSpace(classAttr))
            {
                ApplyCssClassStyles(xamlElement, classAttr, cssRules);
            }

            // Apply element-specific styles
            ApplyElementSpecificStyles(htmlElement, xamlElement);
        }

        private static void ApplyInlineStyles(XmlElement xamlElement, string styleString)
        {
            try
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
            catch
            {
                // Style application failed, continue without this style
            }
        }

        private static void ApplyCssClassStyles(XmlElement xamlElement, string classNames, Dictionary<string, Dictionary<string, string>> cssRules)
        {
            var classes = classNames.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var className in classes)
            {
                if (cssRules.TryGetValue($".{className}", out var classStyles))
                {
                    foreach (var style in classStyles)
                    {
                        ApplyCssProperty(xamlElement, style.Key, style.Value);
                    }
                }
            }
        }

        private static void ApplyCssProperty(XmlElement xamlElement, string property, string value)
        {
            switch (property.ToLower())
            {
                case "font-weight":
                    if (FontWeightMappings.TryGetValue(value.ToLower(), out var fontWeight))
                    {
                        xamlElement.SetAttribute("FontWeight", fontWeight.ToString());
                    }
                    break;

                case "font-size":
                    if (TryParseFontSize(value, out var fontSize))
                    {
                        xamlElement.SetAttribute("FontSize", fontSize.ToString());
                    }
                    break;

                case "color":
                    if (TryParseColor(value, out var color))
                    {
                        xamlElement.SetAttribute("Foreground", color);
                    }
                    break;

                case "background-color":
                    if (TryParseColor(value, out var bgColor))
                    {
                        xamlElement.SetAttribute("Background", bgColor);
                    }
                    break;

                case "text-align":
                    var alignment = value.ToLower() switch
                    {
                        "left" => "Left",
                        "right" => "Right",
                        "center" => "Center",
                        "justify" => "Justify",
                        _ => null
                    };
                    if (alignment != null)
                    {
                        xamlElement.SetAttribute("TextAlignment", alignment);
                    }
                    break;

                case "margin":
                    if (TryParseMargin(value, out var margin))
                    {
                        xamlElement.SetAttribute("Margin", margin);
                    }
                    break;
            }
        }

        private static void ApplyElementSpecificStyles(HtmlNode htmlElement, XmlElement xamlElement)
        {
            var tagName = htmlElement.Name.ToLower();
            
            switch (tagName)
            {
                case "strong":
                case "b":
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "em":
                case "i":
                    xamlElement.SetAttribute("FontStyle", "Italic");
                    break;

                case "u":
                    xamlElement.SetAttribute("TextDecorations", "Underline");
                    break;

                case "h1":
                    xamlElement.SetAttribute("FontSize", "32");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "h2":
                    xamlElement.SetAttribute("FontSize", "24");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "h3":
                    xamlElement.SetAttribute("FontSize", "20");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "h4":
                    xamlElement.SetAttribute("FontSize", "18");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "h5":
                    xamlElement.SetAttribute("FontSize", "16");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "h6":
                    xamlElement.SetAttribute("FontSize", "14");
                    xamlElement.SetAttribute("FontWeight", "Bold");
                    break;

                case "ul":
                    xamlElement.SetAttribute("MarkerStyle", "Disc");
                    break;

                case "ol":
                    xamlElement.SetAttribute("MarkerStyle", "Decimal");
                    break;
            }
        }



        #region Helper Methods

        private static bool TryParseFontSize(string value, out double fontSize)
        {
            fontSize = 12; // default

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
                    fontSize = em * 16; // Assume 16px base
                    return true;
                }
            }
            else if (double.TryParse(value, out fontSize))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseColor(string value, out string color)
        {
            color = "Black"; // default

            // Handle common color names and hex values
            if (value.StartsWith("#"))
            {
                color = value;
                return true;
            }

            var colorMappings = new Dictionary<string, string>
            {
                {"red", "#FF0000"},
                {"green", "#008000"},
                {"blue", "#0000FF"},
                {"black", "#000000"},
                {"white", "#FFFFFF"},
                {"gray", "#808080"},
                {"yellow", "#FFFF00"},
                {"cyan", "#00FFFF"},
                {"magenta", "#FF00FF"}
            };

            if (colorMappings.TryGetValue(value.ToLower(), out color))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseMargin(string value, out string margin)
        {
            margin = "0";
            
            // Simple margin parsing - could be enhanced
            if (value.Contains("px"))
            {
                margin = value.Replace("px", "");
                return true;
            }

            return false;
        }

        #endregion
    }
}
