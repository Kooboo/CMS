#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
/*
 * 
 * http://regexadvice.com/blogs/mash/archive/2008/04/18/Follow-up-to-Additional-CSS-minifying-regex-patterns.aspx
 * 
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.WebResourceLoader
{
    public class CSSMinify
    {
        public static Hashtable shortColorNames = new Hashtable();
        public static Hashtable shortHexColors = new Hashtable();

        static CSSMinify()
        {
            createHashTable();
        }

        public static string Minify(System.Web.Mvc.UrlHelper urlHelper, string cssPath, string requestPath, string cssContent)
        {
            // this construct is to enable us to refer to the relativePath above ...
            MatchEvaluator urlDelegate = new MatchEvaluator(delegate(Match m)
            {
                // Change relative (to the original CSS) URL references to make them relative to the requested URL (controller / action)
                string url = m.Value;

                Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);

                if (uri.IsAbsoluteUri || VirtualPathUtility.IsAbsolute(url))
                {
                    // if the URL is absolute ("http://server/path/file.ext") or app absolute ("/path/file.ext") then leave it as it is
                }
                else
                {
                    // get app relative url
                    url = VirtualPathUtility.Combine(cssPath, url);
                    url = VirtualPathUtility.MakeRelative(requestPath, url);


                    url = urlHelper.Content(url);
                }

                return url;
            });
            return Minify(urlHelper, urlDelegate, cssContent, 0);
        }
        public static string Minify(System.Web.Mvc.UrlHelper urlHelper, MatchEvaluator urlReplacer, string cssContent, int columnWidth)
        {
            // BSD License http://developer.yahoo.net/yui/license.txt
            // New css tests and regexes by Michael Ash
            MatchEvaluator rgbDelegate = new MatchEvaluator(RGBMatchHandler);
            MatchEvaluator shortColorNameDelegate = new MatchEvaluator(ShortColorNameMatchHandler);
            MatchEvaluator shortColorHexDelegate = new MatchEvaluator(ShortColorHexMatchHandler);

            cssContent = RemoveCommentBlocks(cssContent);
            cssContent = Regex.Replace(cssContent, @"\s+", " "); //Normalize whitespace
            cssContent = Regex.Replace(cssContent, @"\x22\x5C\x22}\x5C\\x22\x22", "___PSEUDOCLASSBMH___"); //hide Box model hack 
            /* Remove the spaces before the things that should not have spaces before them.
               But, be careful not to turn "p :link {...}" into "p:link{...}"
            */
            cssContent = Regex.Replace(cssContent, @"(?#no preceding space needed)\s+((?:[!{};>+()\],])|(?<={[^{}]*):(?=[^}]*}))", "$1");
            cssContent = Regex.Replace(cssContent, @"([!{}:;>+([,])\s+", "$1");  // Remove the spaces after the things that should not have spaces after them.
            cssContent = Regex.Replace(cssContent, @"([^;}])}", "$1;}");    // Add the semicolon where it's missing.
            cssContent = Regex.Replace(cssContent, @"(\d+)\.0+(p(?:[xct])|(?:[cem])m|%|in|ex)\b", "$1$2"); // Remove .0 from size units x.0em becomes xem
            cssContent = Regex.Replace(cssContent, @"([\s:])(0)(px|em|%|in|cm|mm|pc|pt|ex)\b", "$1$2"); // Remove unit from zero
            //New test
            //Font weights
            cssContent = Regex.Replace(cssContent, @"(?<=font-weight:)normal\b", "400");
            cssContent = Regex.Replace(cssContent, @"(?<=font-weight:)bold\b", "700");
            //Thought this was a good idea but properties of a set not defined get element defaults. This is reseting them. css = ShortHandProperty(css);
            cssContent = ShortHandAllProperties(cssContent);
            //css = Regex.Replace(css, @":(\s*0){2,4}\s*;", ":0;"); // if all parameters zero just use 1 parameter
            // if all 4 parameters the same unit make 1 parameter
            //css = Regex.Replace(css, @":\s*(inherit|auto|0|(?:(?:\d*\.?\d+(?:p(?:[xct])|(?:[cem])m|%|in|ex))))(\s+\1){1,3};", ":$1;", RegexOptions.IgnoreCase); //if background-position:500px 500px; replaced to background-position:500px;, that will parsed to background-position:500px 50% at the client.
            // if has 4 parameters and top unit = bottom unit and right unit = left unit make 2 parameters
            cssContent = Regex.Replace(cssContent, @":\s*((inherit|auto|0|(?:(?:\d*\.?\d+(?:p(?:[xct])|(?:[cem])m|%|in|ex))))\s+(inherit|auto|0|(?:(?:\d?\.?\d(?:p(?:[xct])|(?:[cem])m|%|in|ex)))))\s+\2\s+\3;", ":$1;", RegexOptions.IgnoreCase);
            // if has 4 parameters and top unit != bottom unit and right unit = left unit make 3 parameters
            cssContent = Regex.Replace(cssContent, @":\s*((?:(?:inherit|auto|0|(?:(?:\d*\.?\d+(?:p(?:[xct])|(?:[cem])m|%|in|ex))))\s+)?(inherit|auto|0|(?:(?:\d?\.?\d(?:p(?:[xct])|(?:[cem])m|%|in|ex))))\s+(?:0|(?:(?:\d?\.?\d(?:p(?:[xct])|(?:[cem])m|%|in|ex)))))\s+\2;", ":$1;", RegexOptions.IgnoreCase);
            //// if has 3 parameters and top unit = bottom unit make 2 parameters
            //css = Regex.Replace(css, @":\s*((0|(?:(?:\d?\.?\d(?:p(?:[xct])|(?:[cem])m|%|in|ex))))\s+(?:0|(?:(?:\d?\.?\d(?:p(?:[xct])|(?:[cem])m|%|in|ex)))))\s+\2;", ":$1;", RegexOptions.IgnoreCase);
            cssContent = Regex.Replace(cssContent, "background-position:0;", "background-position:0 0;");
            cssContent = Regex.Replace(cssContent, @"(:|\s)0+\.(\d+)", "$1.$2");
            //  Outline-styles and Border-sytles parameter reduction
            cssContent = Regex.Replace(cssContent, @"(outline|border)-style\s*:\s*(none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset)(?:\s+\2){1,3};", "$1-style:$2;", RegexOptions.IgnoreCase);

            cssContent = Regex.Replace(cssContent, @"(outline|border)-style\s*:\s*((none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset)\s+(none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset ))(?:\s+\3)(?:\s+\4);", "$1-style:$2;", RegexOptions.IgnoreCase);

            cssContent = Regex.Replace(cssContent, @"(outline|border)-style\s*:\s*((?:(?:none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset)\s+)?(none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset )\s+(?:none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset ))(?:\s+\3);", "$1-style:$2;", RegexOptions.IgnoreCase);

            cssContent = Regex.Replace(cssContent, @"(outline|border)-style\s*:\s*((none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset)\s+(?:none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset ))(?:\s+\3);", "$1-style:$2;", RegexOptions.IgnoreCase);

            //  Outline-color and Border-color parameter reduction
            cssContent = Regex.Replace(cssContent, @"(outline|border)-color\s*:\s*((?:\#(?:[0-9A-F]{3}){1,2})|\S+)(?:\s+\2){1,3};", "$1-color:$2;", RegexOptions.IgnoreCase);

            cssContent = Regex.Replace(cssContent, @"(outline|border)-color\s*:\s*(((?:\#(?:[0-9A-F]{3}){1,2})|\S+)\s+((?:\#(?:[0-9A-F]{3}){1,2})|\S+))(?:\s+\3)(?:\s+\4);", "$1-color:$2;", RegexOptions.IgnoreCase);

            cssContent = Regex.Replace(cssContent, @"(outline|border)-color\s*:\s*((?:(?:(?:\#(?:[0-9A-F]{3}){1,2})|\S+)\s+)?((?:\#(?:[0-9A-F]{3}){1,2})|\S+)\s+(?:(?:\#(?:[0-9A-F]{3}){1,2})|\S+))(?:\s+\3);", "$1-color:$2;", RegexOptions.IgnoreCase);

            // Shorten colors from rgb(51,102,153) to #336699
            // This makes it more likely that it'll get further compressed in the next step.
            cssContent = Regex.Replace(cssContent, @"rgb\s*\x28((?:25[0-5])|(?:2[0-4]\d)|(?:[01]?\d?\d))\s*,\s*((?:25[0-5])|(?:2[0-4]\d)|(?:[01]?\d?\d))\s*,\s*((?:25[0-5])|(?:2[0-4]\d)|(?:[01]?\d?\d))\s*\x29", rgbDelegate);
            cssContent = Regex.Replace(cssContent, @"(?<![\x22\x27=]\s*)\#(?:([0-9A-F])\1)(?:([0-9A-F])\2)(?:([0-9A-F])\3)", "#$1$2$3", RegexOptions.IgnoreCase);
            // Replace hex color code with named value is shorter
            cssContent = Regex.Replace(cssContent, @"(?<=color\s*:\s*.*)\#(?<hex>f00)\b", "red", RegexOptions.IgnoreCase);
            cssContent = Regex.Replace(cssContent, @"(?<=color\s*:\s*.*)\#(?<hex>[0-9a-f]{6})", shortColorNameDelegate, RegexOptions.IgnoreCase);
            cssContent = Regex.Replace(cssContent, @"(?<=color\s*:\s*)\b(Black|Fuchsia|LightSlateGr[ae]y|Magenta|White|Yellow)\b", shortColorHexDelegate, RegexOptions.IgnoreCase);

            // replace URLs
            if (urlReplacer != null)
            {
                cssContent = Regex.Replace(cssContent, @"(?<=url\(\s*([""']?))(?<url>[^'""]+?)(?=\1\s*\))", urlReplacer, RegexOptions.IgnoreCase);
                cssContent = Regex.Replace(cssContent, @"(?<=@import\s*([""']))(?<url>[^'""]+?)(?=\1\s*;)", urlReplacer, RegexOptions.IgnoreCase);
            }

            // Remove empty rules.
            cssContent = Regex.Replace(cssContent, @"[^}]+{;}", "");
            //Remove semicolon of last property
            cssContent = Regex.Replace(cssContent, ";(})", "$1");
            if (columnWidth > 0)
            {
                cssContent = BreakLines(cssContent, columnWidth);
            }
            return cssContent;
        }

        private static string RemoveCommentBlocks(string input)
        {
            int startIndex = 0;
            int endIndex = 0;
            bool iemac = false;
            startIndex = input.IndexOf(@"/*", startIndex);
            while (startIndex >= 0)
            {
                endIndex = input.IndexOf(@"*/", startIndex + 2);
                if (endIndex >= startIndex + 2)
                {
                    if (input[endIndex - 1] == '\\')
                    {
                        startIndex = endIndex + 2;
                        iemac = true;
                    }
                    else if (iemac)
                    {
                        startIndex = endIndex + 2;
                        iemac = false;
                    }
                    else
                    {
                        input = input.Remove(startIndex, endIndex + 2 - startIndex);
                    }
                }
                startIndex = input.IndexOf(@"/*", startIndex);
            }
            return input;
        }

        private static String RGBMatchHandler(Match m)
        {
            int val = 0;
            StringBuilder hexcolor = new StringBuilder("#");
            for (int index = 1; index <= 3; index += 1)
            {
                val = Int32.Parse(m.Groups[index].Value);
                hexcolor.Append(val.ToString("x2"));
            }
            return hexcolor.ToString();
        }

        private static string BreakLines(string css, int columnWidth)
        {
            int i = 0;
            int start = 0;
            StringBuilder sb = new StringBuilder(css);
            while (i < sb.Length)
            {
                char c = sb[i++];
                if (c == '}' && i - start > columnWidth)
                {
                    sb.Insert(i, '\n');
                    start = i;
                }
            }
            return sb.ToString();
        }

        private static string ReplaceNonEmpty(string inputText, string replacementText)
        {
            if (replacementText.Trim() != string.Empty)
            {
                inputText = string.Format(" {0}", replacementText);
            }
            return inputText;
        }

        private static string ShortColorNameMatchHandler(Match m)
        {
            // This function replace hex color values named colors if the name is shorter than the hex code
            string returnValue = m.Value;
            if (shortColorNames.ContainsKey(m.Groups["hex"].Value))
            {
                returnValue = shortColorNames[m.Groups["hex"].Value].ToString();
            }
            return returnValue;
        }

        private static string ShortColorHexMatchHandler(Match m)
        {
            //This function replaces named values with there shorter hex equivalent
            return shortHexColors[m.Value.ToString().ToLower()].ToString();
        }

        private static void createHashTable()
        {
            //Color names shorter than hex notation. Except for red.
            shortColorNames.Add("F0FFFF".ToLower(), "Azure".ToLower());
            shortColorNames.Add("F5F5DC".ToLower(), "Beige".ToLower());
            shortColorNames.Add("FFE4C4".ToLower(), "Bisque".ToLower());
            shortColorNames.Add("A52A2A".ToLower(), "Brown".ToLower());
            shortColorNames.Add("FF7F50".ToLower(), "Coral".ToLower());
            shortColorNames.Add("FFD700".ToLower(), "Gold".ToLower());
            shortColorNames.Add("808080".ToLower(), "Grey".ToLower());
            shortColorNames.Add("008000".ToLower(), "Green".ToLower());
            shortColorNames.Add("4B0082".ToLower(), "Indigo".ToLower());
            shortColorNames.Add("FFFFF0".ToLower(), "Ivory".ToLower());
            shortColorNames.Add("F0E68C".ToLower(), "Khaki".ToLower());
            shortColorNames.Add("FAF0E6".ToLower(), "Linen".ToLower());
            shortColorNames.Add("800000".ToLower(), "Maroon".ToLower());
            shortColorNames.Add("000080".ToLower(), "Navy".ToLower());
            shortColorNames.Add("808000".ToLower(), "Olive".ToLower());
            shortColorNames.Add("FFA500".ToLower(), "Orange".ToLower());
            shortColorNames.Add("DA70D6".ToLower(), "Orchid".ToLower());
            shortColorNames.Add("CD853F".ToLower(), "Peru".ToLower());
            shortColorNames.Add("FFC0CB".ToLower(), "Pink".ToLower());
            shortColorNames.Add("DDA0DD".ToLower(), "Plum".ToLower());
            shortColorNames.Add("800080".ToLower(), "Purple".ToLower());
            shortColorNames.Add("FA8072".ToLower(), "Salmon".ToLower());
            shortColorNames.Add("A0522D".ToLower(), "Sienna".ToLower());
            shortColorNames.Add("C0C0C0".ToLower(), "Silver".ToLower());
            shortColorNames.Add("FFFAFA".ToLower(), "Snow".ToLower());
            shortColorNames.Add("D2B48C".ToLower(), "Tan".ToLower());
            shortColorNames.Add("008080".ToLower(), "Teal".ToLower());
            shortColorNames.Add("FF6347".ToLower(), "Tomato".ToLower());
            shortColorNames.Add("EE82EE".ToLower(), "Violet".ToLower());
            shortColorNames.Add("F5DEB3".ToLower(), "Wheat".ToLower());

            // Hex notation shorter than named value
            shortHexColors.Add("black", "#000");
            shortHexColors.Add("fuchsia", "#f0f");
            shortHexColors.Add("lightSlategray", "#789");
            shortHexColors.Add("lightSlategrey", "#789");
            shortHexColors.Add("magenta", "#f0f");
            shortHexColors.Add("white", "#fff");
            shortHexColors.Add("yellow", "#ff0");
        }
        private static string ShortHandAllProperties(string css)
        {
            /*
             * This function searchs for properties specifying all the individual properties of a property type
             * and reduces it to a single property use shorthand notation
             */
            Regex reCSSBlock = new Regex("{[^{}]*}");
            Regex reTRBL1 = new Regex(@"(?<fullProperty>(?:(?<property>padding)-(?<position>top|right|bottom|left)))\s*:\s*(?<unit>[\w.]+);?", RegexOptions.IgnoreCase);
            Regex reTRBL2 = new Regex(@"(?<fullProperty>(?:(?<property>margin)-(?<position>top|right|bottom|left)))\s*:\s*(?<unit>[\w.]+);?", RegexOptions.IgnoreCase);
            Regex reTRBL3 = new Regex(@"(?<fullProperty>(?<property>border)-(?<position>top|right|bottom|left)(?<property2>-(?:color)))\s*:\s*(?<unit>[#\w.]+);?", RegexOptions.IgnoreCase);
            Regex reTRBL4 = new Regex(@"(?<fullProperty>(?<property>border)-(?<position>top|right|bottom|left)(?<property2>-(?:style)))\s*:\s*(?<unit>none|hidden|d(?:otted|ashed|ouble)|solid|groove|ridge|inset|outset);?", RegexOptions.IgnoreCase);
            Regex reTRBL5 = new Regex(@"(?<fullProperty>(?<property>border)-(?<position>top|right|bottom|left)(?<property2>-(?:width)))\s*:\s*(?<unit>[\w.]+);?", RegexOptions.IgnoreCase);
            Regex reListStyle = new Regex(@"list-style-(?<style>type|image|position)\s*:\s*(?<unit>[^};]+);?", RegexOptions.IgnoreCase);
            Regex reFont = new Regex(@"font-(?:(?:(?<fontProperty>family\b)\s*:\s*(?<fontPropertyValue>(?:\b[a-zA-Z]+(-[a-zA-Z]+)?\b|\x22[^\x22]+\x22)(?:\s*,\s*(?:\b[a-zA-Z]+(-[a-zA-Z]+)?\b|\x22[^\x22]+\x22))*)\b)|
(?:(?<fontProperty>style\b)\s*:\s*(?<fontPropertyValue>normal|italic|oblique|inherit))|
(?:(?<fontProperty>variant\b)\s*:\s*(?<fontPropertyValue>normal|small-caps|inherit))|
(?:(?<fontProperty>weight\b)\s*:\s*(?<fontPropertyValue>normal|bold|(?:bold|light)er|[1-9]00|inherit))|
(?:(?<fontProperty>size\b)\s*:\s*(?<fontPropertyValue>(?:(?:xx?-)?(?:small|large))|medium|(?:\d*\.?\d+(?:%|(p(?:[xct])|(?:[cem])m|in|ex))\b)|inherit|\b0\b)))\s*;?", (RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace));
            Regex reBackGround = new Regex(@"background-(?:
(?:(?<property>color)\s*:\s*(?<unit>transparent|inherit|(?:(?:\#(?:[0-9A-F]{3}){1,2})|\S+)))|
(?:(?<property>image)\s*:\s*(?<unit>none|inherit|(?:url\s*\([^()]+\))))|
(?:(?<property>repeat)\s*:\s*(?<unit>no-repeat|inherit|repeat(?:-[xy])))|
(?:(?<property>attachment)\s*:\s*(?<unit>scroll|inherit|fixed))|
(?:(?<property>position)\s*:\s*(?<unit>((?<horizontal>left | center | right|(?:0|(?:(?:\d*\.?\d+(?:p(?:[xct])|(?:[cem])m|%|in|ex)))))\s+(?<vertical>top | center | bottom |(?:0|(?:(?:\d*\.?\d+(?:p(?:[xct])|(?:[cem])m|%|in|ex))))))|
    ((?<vertical>top | center | bottom )\s+(?<horizontal>left | center | right ))|
    ((?<horizontal>left | center | right )|(?<vertical>top | center | bottom ))))
);?", (RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture));
            MatchCollection mcBlocks = reCSSBlock.Matches(css);
            foreach (Match mBlock in mcBlocks)
            {
                string strBlock = mBlock.Value;
                HasAllPositions(reTRBL1, ref strBlock);
                HasAllPositions(reTRBL2, ref strBlock);
                HasAllPositions(reTRBL3, ref strBlock);
                HasAllPositions(reTRBL4, ref strBlock);
                HasAllPositions(reTRBL5, ref strBlock);
                HasAllListStyle(reListStyle, ref strBlock);
                HasAllFontProperties(reFont, ref strBlock);
                HasAllBackGroundProperties(reBackGround, ref strBlock);
                css = css.Replace(mBlock.Value, strBlock);
            }
            return css;
        }

        private static void HasAllBackGroundProperties(Regex re, ref string CSSText)
        {
            {
                MatchCollection mcProperySet = re.Matches(CSSText);
                int z = 5;
                if (mcProperySet.Count == z)
                {

                    int y = 0;
                    for (int x = 0; x < z; x = x + 1)
                    {
                        switch (mcProperySet[x].Groups["property"].Value)
                        {
                            case "color":
                                y = y + 1;
                                break;
                            case "image":
                                y = y + 2;
                                break;
                            case "repeat":
                                y = y + 4;
                                break;
                            case "attachment":
                                y = y + 8;
                                break;
                            case "position":
                                y = y + 16;
                                break;
                        }
                    }
                    if (y == 31)
                    {
                        CSSText = ShortHandBackGroundReplaceV2(mcProperySet, re, CSSText);
                    }
                }
            }
        }

        private static void HasAllFontProperties(Regex re, ref string CSSText)
        {
            {
                MatchCollection mcProperySet = re.Matches(CSSText);
                int z = 5;
                if (mcProperySet.Count == z)
                {

                    int y = 0;
                    for (int x = 0; x < z; x = x + 1)
                    {
                        switch (mcProperySet[x].Groups["fontProperty"].Value)
                        {
                            case "style":
                                y = y + 1;
                                break;
                            case "variant":
                                y = y + 2;
                                break;
                            case "weight":
                                y = y + 4;
                                break;
                            case "size":
                                y = y + 8;
                                break;
                            case "family":
                                y = y + 16;
                                break;
                        }
                    }
                    if (y == 31)
                    {
                        CSSText = ShortHandFontReplaceV2(mcProperySet, re, CSSText);
                    }
                }
            }
        }

        private static void HasAllListStyle(Regex re, ref string CSSText)
        {
            {
                int z = 3;
                MatchCollection mcProperySet = re.Matches(CSSText);
                if (mcProperySet.Count == z)
                {

                    int y = 0;
                    for (int x = 0; x < z; x = x + 1)
                    {
                        switch (mcProperySet[x].Groups["style"].Value)
                        {
                            case "type":
                                y = y + 1;
                                break;
                            case "image":
                                y = y + 2;
                                break;
                            case "position":
                                y = y + 4;
                                break;

                        }
                    }
                    if (y == 7)
                    {
                        CSSText = ShortHandListReplaceV2(mcProperySet, re, CSSText);
                    }
                }
            }
        }

        private static void HasAllPositions(Regex re, ref string CSSText)
        {
            {
                MatchCollection mcProperySet = re.Matches(CSSText);
                if (mcProperySet.Count == 4)
                {

                    int y = 0;
                    for (int x = 0; x < 4; x = x + 1)
                    {
                        switch (mcProperySet[x].Groups["position"].Value)
                        {
                            case "top":
                                y = y + 1;
                                break;
                            case "right":
                                y = y + 2;
                                break;
                            case "bottom":
                                y = y + 4;
                                break;
                            case "left":
                                y = y + 8;
                                break;
                        }
                    }
                    if (y == 15)
                    {
                        CSSText = ShortHandReplaceV2(mcProperySet, re, CSSText);
                    }
                }
            }
        }

        private static string ShortHandFontReplaceV2(MatchCollection mcProperySet, Regex re, string InputText)
        {
            /*
             * This Function replaces the individual font properties with a single entry
             * */
            string strFamily, strStyle, strVariant, strWeight, strSize;
            Regex reLineHeight = new Regex(@"line-height\s*:\s*((?:\d*\.?\d+(?:%|(p(?:[xct])|(?:[cem])m|in|ex)\b)?)|normal|inherit);?", RegexOptions.IgnoreCase);
            strFamily = string.Empty;
            strStyle = string.Empty;
            strVariant = string.Empty;
            strWeight = string.Empty;
            strSize = string.Empty;
            string strStyle_Variant_Weight = string.Empty;
            foreach (Match mProperty in mcProperySet)
            {
                switch (mProperty.Groups[""].Value)
                {
                    case "family":
                        strFamily = string.Format(" {0}", mProperty.Groups["fontPropertyValue"].Value);
                        break;
                    case "size":
                        if (reLineHeight.IsMatch(InputText))
                        {
                            Match m = reLineHeight.Match(InputText);
                            if (m.Groups[1].Value != "normal")
                            {
                                strSize = String.Format("/{0}", m.Groups[1].Value);
                            }
                            InputText = reLineHeight.Replace(InputText, string.Empty);
                        }
                        strSize = string.Format(" {0}{1}", mProperty.Groups["fontPropertyValue"].Value, strSize);
                        if (strSize == "medium")
                        {
                            strSize = string.Empty;
                        }
                        break;
                    case "style":
                    case "variant":
                    case "weight":
                        if (mProperty.Groups["fontPropertyValue"].Value != "normal")
                        {
                            strStyle_Variant_Weight += string.Format(" {0}", mProperty.Groups["fontPropertyValue"].Value);
                        } break;

                }
            }

            string strShortcut;
            string strProperties = string.Format("{0}{1}{2};", strStyle_Variant_Weight, strVariant, strWeight, strSize, strFamily);
            strShortcut = string.Format("font:{0}", strProperties.Trim());
            string strNewBlock = re.Replace(InputText, "");
            strNewBlock = strNewBlock.Insert(1, strShortcut);
            return strNewBlock;
        }

        private static string ShortHandBackGroundReplaceV2(MatchCollection mcProperySet, Regex re, string InputText)
        {
            /*
             * This Function replaces the individual background properties with a single entry
             * */
            string strColor, strImage, strRepeat, strAttachment, strPosition;
            strColor = string.Empty;
            strImage = string.Empty;
            strRepeat = string.Empty;
            strAttachment = string.Empty;
            strPosition = string.Empty;
            foreach (Match mProperty in mcProperySet)
            {
                switch (mProperty.Groups["property"].Value)
                {
                    case "color":
                        if (mProperty.Groups["unit"].Value != "transparent")
                        {
                            strColor = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                    case "image":
                        if (mProperty.Groups["unit"].Value != "none")
                        {
                            strImage = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                    case "repeat":
                        if (mProperty.Groups["unit"].Value != "repeat")
                        {
                            strRepeat = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        } break;
                    case "attachment":
                        if (mProperty.Groups["unit"].Value != "scroll")
                        {
                            strAttachment = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                    case "position":
                        if (mProperty.Groups["unit"].Value != "0% 0%")
                        {
                            strPosition = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                }
            }

            string strShortcut;
            string strProperties = string.Format("{0}{1}{2}{3}{4};", strColor, strImage, strRepeat, strAttachment, strPosition);
            strShortcut = string.Format("background:{0}", strProperties.Trim());
            string strNewBlock = re.Replace(InputText, "");
            strNewBlock = strNewBlock.Insert(1, strShortcut);
            return strNewBlock;
        }

        private static string ShortHandReplaceV2(MatchCollection mcProperySet, Regex reTRBL1, string InputText)
        {
            // Replace method for regexes used in ShortHand property method for properties with top, right, bottom and left sub properties.
            string strTop, strRight, strBottom, strLeft;
            strTop = string.Empty;
            strRight = string.Empty;
            strBottom = string.Empty;
            strLeft = string.Empty;
            string strProperty;
            strProperty = string.Format("{0}{1}", mcProperySet[0].Groups["property"].Value, mcProperySet[0].Groups["property2"].Value);
            foreach (Match mProperty in mcProperySet)
            {
                switch (mProperty.Groups["position"].Value)
                {
                    case "top":
                        strTop = mProperty.Groups["unit"].Value;
                        break;
                    case "right":
                        strRight = mProperty.Groups["unit"].Value;
                        break;
                    case "bottom":
                        strBottom = mProperty.Groups["unit"].Value;
                        break;
                    case "left":
                        strLeft = mProperty.Groups["unit"].Value;
                        break;
                }

            }

            string strShortcut = string.Format("{0}:{1} {2} {3} {4};", strProperty, strTop, strRight, strBottom, strLeft);
            string strNewBlock = reTRBL1.Replace(InputText, "");
            strNewBlock = strNewBlock.Insert(1, strShortcut);
            return strNewBlock;
        }

        private static string ShortHandListReplaceV2(MatchCollection mcProperySet, Regex re, string InputText)
        {
            /*
             * This Function replaces the individual list properties with a single entry
             * */
            string strType, strPosition, strImage;
            strType = string.Empty;
            strPosition = string.Empty;
            strImage = string.Empty;
            foreach (Match mProperty in mcProperySet)
            {
                switch (mProperty.Groups["style"].Value)
                {
                    case "type":
                        if (mProperty.Groups["unit"].Value != "disc")
                        {
                            strType = mProperty.Groups["unit"].Value;
                        }
                        break;
                    case "position":
                        if (mProperty.Groups["unit"].Value != "outside")
                        {
                            strPosition = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                    case "style":
                        if (mProperty.Groups["unit"].Value != "none")
                        {
                            strImage = string.Format(" {0}", mProperty.Groups["unit"].Value);
                        }
                        break;
                }

            }

            string strShortcut = string.Format("list-style:{0}{1}{2};", strType, strPosition, strImage);
            string strNewBlock = re.Replace(InputText, "");
            strNewBlock = strNewBlock.Insert(1, strShortcut);
            return strNewBlock;
        }
    }
}
