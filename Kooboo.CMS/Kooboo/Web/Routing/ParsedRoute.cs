#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace System.Web.Routing
{
    public sealed class ParsedRoute
    {
        // Methods
        public ParsedRoute(IList<PathSegment> pathSegments)
        {
            this.PathSegments = pathSegments;
        }

        public BoundUrl Bind(RouteValueDictionary currentValues, RouteValueDictionary values, RouteValueDictionary defaultValues, RouteValueDictionary constraints)
        {
            if (currentValues == null)
            {
                currentValues = new RouteValueDictionary();
            }
            if (values == null)
            {
                values = new RouteValueDictionary();
            }
            if (defaultValues == null)
            {
                defaultValues = new RouteValueDictionary();
            }
            RouteValueDictionary acceptedValues = new RouteValueDictionary();
            HashSet<string> unusedNewValues = new HashSet<string>(values.Keys, StringComparer.OrdinalIgnoreCase);
            ForEachParameter(this.PathSegments, delegate(ParameterSubsegment parameterSubsegment)
            {
                object obj2;
                object obj3;
                string key = parameterSubsegment.ParameterName;
                bool flag = values.TryGetValue(key, out obj2);
                if (flag)
                {
                    unusedNewValues.Remove(key);
                }
                bool flag2 = currentValues.TryGetValue(key, out obj3);
                if ((flag && flag2) && !RoutePartsEqual(obj3, obj2))
                {
                    return false;
                }
                if (flag)
                {
                    if (IsRoutePartNonEmpty(obj2))
                    {
                        acceptedValues.Add(key, obj2);
                    }
                }
                else if (flag2)
                {
                    acceptedValues.Add(key, obj3);
                }
                return true;
            });
            foreach (KeyValuePair<string, object> pair in values)
            {
                if (IsRoutePartNonEmpty(pair.Value) && !acceptedValues.ContainsKey(pair.Key))
                {
                    acceptedValues.Add(pair.Key, pair.Value);
                }
            }
            foreach (KeyValuePair<string, object> pair2 in currentValues)
            {
                string str = pair2.Key;
                if (!acceptedValues.ContainsKey(str) && (GetParameterSubsegment(this.PathSegments, str) == null))
                {
                    acceptedValues.Add(str, pair2.Value);
                }
            }
            ForEachParameter(this.PathSegments, delegate(ParameterSubsegment parameterSubsegment)
            {
                object obj2;
                if (!acceptedValues.ContainsKey(parameterSubsegment.ParameterName) && !IsParameterRequired(parameterSubsegment, defaultValues, out obj2))
                {
                    acceptedValues.Add(parameterSubsegment.ParameterName, obj2);
                }
                return true;
            });
            if (!ForEachParameter(this.PathSegments, delegate(ParameterSubsegment parameterSubsegment)
            {
                object obj2;
                if (IsParameterRequired(parameterSubsegment, defaultValues, out obj2) && !acceptedValues.ContainsKey(parameterSubsegment.ParameterName))
                {
                    return false;
                }
                return true;
            }))
            {
                return null;
            }
            RouteValueDictionary otherDefaultValues = new RouteValueDictionary(defaultValues);
            ForEachParameter(this.PathSegments, delegate(ParameterSubsegment parameterSubsegment)
            {
                otherDefaultValues.Remove(parameterSubsegment.ParameterName);
                return true;
            });
            foreach (KeyValuePair<string, object> pair3 in otherDefaultValues)
            {
                object obj2;
                if (values.TryGetValue(pair3.Key, out obj2))
                {
                    unusedNewValues.Remove(pair3.Key);
                    if (!RoutePartsEqual(obj2, pair3.Value))
                    {
                        return null;
                    }
                }
            }
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            bool flag2_1 = false;
            for (int i = 0; i < this.PathSegments.Count; i++)
            {
                PathSegment segment = this.PathSegments[i];
                if (segment is SeparatorPathSegment)
                {
                    if (flag2_1 && (builder2.Length > 0))
                    {
                        builder.Append(builder2.ToString());
                        builder2.Length = 0;
                    }
                    flag2_1 = false;
                    if ((builder2.Length > 0) && (builder2[builder2.Length - 1] == '/'))
                    {
                        return null;
                    }
                    builder2.Append("/");
                }
                else
                {
                    ContentPathSegment segment2 = segment as ContentPathSegment;
                    if (segment2 != null)
                    {
                        bool flag3 = false;
                        foreach (PathSubsegment subsegment2 in segment2.Subsegments)
                        {
                            LiteralSubsegment subsegment3 = subsegment2 as LiteralSubsegment;
                            if (subsegment3 != null)
                            {
                                flag2_1 = true;
                                builder2.Append(UrlEncode(subsegment3.Literal));
                            }
                            else
                            {
                                ParameterSubsegment subsegment4 = subsegment2 as ParameterSubsegment;
                                if (subsegment4 != null)
                                {
                                    object obj3;
                                    object obj4;
                                    if (flag2_1 && (builder2.Length > 0))
                                    {
                                        builder.Append(builder2.ToString());
                                        builder2.Length = 0;
                                        flag3 = true;
                                    }
                                    flag2_1 = false;
                                    if (acceptedValues.TryGetValue(subsegment4.ParameterName, out obj3))
                                    {
                                        unusedNewValues.Remove(subsegment4.ParameterName);
                                    }
                                    defaultValues.TryGetValue(subsegment4.ParameterName, out obj4);
                                    if (RoutePartsEqual(obj3, obj4))
                                    {
                                        builder2.Append(UrlEncode(Convert.ToString(obj3, CultureInfo.InvariantCulture)));
                                        continue;
                                    }
                                    if (builder2.Length > 0)
                                    {
                                        builder.Append(builder2.ToString());
                                        builder2.Length = 0;
                                    }
                                    builder.Append(UrlEncode(Convert.ToString(obj3, CultureInfo.InvariantCulture)));
                                    flag3 = true;
                                }
                            }
                        }
                        if (flag3 && (builder2.Length > 0))
                        {
                            builder.Append(builder2.ToString());
                            builder2.Length = 0;
                        }
                    }
                }
            }
            if (flag2_1 && (builder2.Length > 0))
            {
                builder.Append(builder2.ToString());
            }
            if (constraints != null)
            {
                foreach (KeyValuePair<string, object> pair4 in constraints)
                {
                    unusedNewValues.Remove(pair4.Key);
                }
            }
            if (unusedNewValues.Count > 0)
            {
                bool flag5 = true;
                foreach (string str2 in unusedNewValues)
                {
                    object obj5;
                    if (acceptedValues.TryGetValue(str2, out obj5))
                    {
                        builder.Append(flag5 ? '?' : '&');
                        flag5 = false;
                        builder.Append(Uri.EscapeDataString(str2));
                        builder.Append('=');
                        builder.Append(Uri.EscapeDataString(Convert.ToString(obj5, CultureInfo.InvariantCulture)));
                    }
                }
            }
            return new BoundUrl { Url = builder.ToString(), Values = acceptedValues };
        }

        private static string EscapeReservedCharacters(Match m)
        {
            return ("%" + Convert.ToUInt16(m.Value[0]).ToString("x2", CultureInfo.InvariantCulture));
        }

        private static bool ForEachParameter(IList<PathSegment> pathSegments, Func<ParameterSubsegment, bool> action)
        {
            for (int i = 0; i < pathSegments.Count; i++)
            {
                PathSegment segment = pathSegments[i];
                if (!(segment is SeparatorPathSegment))
                {
                    ContentPathSegment segment2 = segment as ContentPathSegment;
                    if (segment2 != null)
                    {
                        foreach (PathSubsegment subsegment in segment2.Subsegments)
                        {
                            if (!(subsegment is LiteralSubsegment))
                            {
                                ParameterSubsegment arg = subsegment as ParameterSubsegment;
                                if ((arg != null) && !action(arg))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static ParameterSubsegment GetParameterSubsegment(IList<PathSegment> pathSegments, string parameterName)
        {
            ParameterSubsegment foundParameterSubsegment = null;
            ForEachParameter(pathSegments, delegate(ParameterSubsegment parameterSubsegment)
            {
                if (string.Equals(parameterName, parameterSubsegment.ParameterName, StringComparison.OrdinalIgnoreCase))
                {
                    foundParameterSubsegment = parameterSubsegment;
                    return false;
                }
                return true;
            });
            return foundParameterSubsegment;
        }

        private static bool IsParameterRequired(ParameterSubsegment parameterSubsegment, RouteValueDictionary defaultValues, out object defaultValue)
        {
            if (parameterSubsegment.IsCatchAll)
            {
                defaultValue = null;
                return false;
            }
            return !defaultValues.TryGetValue(parameterSubsegment.ParameterName, out defaultValue);
        }

        private static bool IsRoutePartNonEmpty(object routePart)
        {
            string str = routePart as string;
            if (str != null)
            {
                return (str.Length > 0);
            }
            return (routePart != null);
        }

        public RouteValueDictionary Match(string virtualPath, RouteValueDictionary defaultValues)
        {
            IList<string> source = RouteParser.SplitUrlToPathSegmentStrings(virtualPath);
            if (defaultValues == null)
            {
                defaultValues = new RouteValueDictionary();
            }
            RouteValueDictionary matchedValues = new RouteValueDictionary();
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < this.PathSegments.Count; i++)
            {
                PathSegment segment = this.PathSegments[i];
                if (source.Count <= i)
                {
                    flag = true;
                }
                string a = flag ? null : source[i];
                if (segment is SeparatorPathSegment)
                {
                    if (!flag && !string.Equals(a, "/", StringComparison.Ordinal))
                    {
                        return null;
                    }
                }
                else
                {
                    ContentPathSegment contentPathSegment = segment as ContentPathSegment;
                    if (contentPathSegment != null)
                    {
                        if (contentPathSegment.IsCatchAll)
                        {
                            this.MatchCatchAll(contentPathSegment, source.Skip<string>(i), defaultValues, matchedValues);
                            flag2 = true;
                        }
                        else if (!this.MatchContentPathSegment(contentPathSegment, a, defaultValues, matchedValues))
                        {
                            return null;
                        }
                    }
                }
            }
            if (!flag2 && (this.PathSegments.Count < source.Count))
            {
                for (int j = this.PathSegments.Count; j < source.Count; j++)
                {
                    if (!RouteParser.IsSeparator(source[j]))
                    {
                        return null;
                    }
                }
            }
            if (defaultValues != null)
            {
                foreach (KeyValuePair<string, object> pair in defaultValues)
                {
                    if (!matchedValues.ContainsKey(pair.Key))
                    {
                        matchedValues.Add(pair.Key, pair.Value);
                    }
                }
            }
            return matchedValues;
        }

        private void MatchCatchAll(ContentPathSegment contentPathSegment, IEnumerable<string> remainingRequestSegments, RouteValueDictionary defaultValues, RouteValueDictionary matchedValues)
        {
            object obj2;
            string str = string.Join(string.Empty, remainingRequestSegments.ToArray<string>());
            ParameterSubsegment subsegment = contentPathSegment.Subsegments[0] as ParameterSubsegment;
            if (str.Length > 0)
            {
                obj2 = str;
            }
            else
            {
                defaultValues.TryGetValue(subsegment.ParameterName, out obj2);
            }
            matchedValues.Add(subsegment.ParameterName, obj2);
        }

        private bool MatchContentPathSegment(ContentPathSegment routeSegment, string requestPathSegment, RouteValueDictionary defaultValues, RouteValueDictionary matchedValues)
        {
            if (string.IsNullOrEmpty(requestPathSegment))
            {
                if (routeSegment.Subsegments.Count <= 1)
                {
                    object obj2;
                    ParameterSubsegment subsegment = routeSegment.Subsegments[0] as ParameterSubsegment;
                    if (subsegment == null)
                    {
                        return false;
                    }
                    if (defaultValues.TryGetValue(subsegment.ParameterName, out obj2))
                    {
                        matchedValues.Add(subsegment.ParameterName, obj2);
                        return true;
                    }
                }
                return false;
            }
            int length = requestPathSegment.Length;
            int num2 = routeSegment.Subsegments.Count - 1;
            ParameterSubsegment subsegment2 = null;
            LiteralSubsegment subsegment3 = null;
            while (num2 >= 0)
            {
                int num3 = length;
                ParameterSubsegment subsegment4 = routeSegment.Subsegments[num2] as ParameterSubsegment;
                if (subsegment4 != null)
                {
                    subsegment2 = subsegment4;
                }
                else
                {
                    LiteralSubsegment subsegment5 = routeSegment.Subsegments[num2] as LiteralSubsegment;
                    if (subsegment5 != null)
                    {
                        subsegment3 = subsegment5;
                        int startIndex = length - 1;
                        if (subsegment2 != null)
                        {
                            startIndex--;
                        }
                        if (startIndex < 0)
                        {
                            return false;
                        }
                        int num5 = requestPathSegment.LastIndexOf(subsegment5.Literal, startIndex, StringComparison.OrdinalIgnoreCase);
                        if (num5 == -1)
                        {
                            return false;
                        }
                        if ((num2 == (routeSegment.Subsegments.Count - 1)) && ((num5 + subsegment5.Literal.Length) != requestPathSegment.Length))
                        {
                            return false;
                        }
                        num3 = num5;
                    }
                }
                if ((subsegment2 != null) && (((subsegment3 != null) && (subsegment4 == null)) || (num2 == 0)))
                {
                    int num6;
                    int num7;
                    if (subsegment3 == null)
                    {
                        if (num2 == 0)
                        {
                            num6 = 0;
                        }
                        else
                        {
                            num6 = num3 + subsegment3.Literal.Length;
                        }
                        num7 = length;
                    }
                    else if ((num2 == 0) && (subsegment4 != null))
                    {
                        num6 = 0;
                        num7 = length;
                    }
                    else
                    {
                        num6 = num3 + subsegment3.Literal.Length;
                        num7 = length - num6;
                    }
                    string str = requestPathSegment.Substring(num6, num7);
                    if (string.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                    matchedValues.Add(subsegment2.ParameterName, str);
                    subsegment2 = null;
                    subsegment3 = null;
                }
                length = num3;
                num2--;
            }
            if (length != 0)
            {
                return (routeSegment.Subsegments[0] is ParameterSubsegment);
            }
            return true;
        }

        private static bool RoutePartsEqual(object a, object b)
        {
            string str = a as string;
            string str2 = b as string;
            if ((str != null) && (str2 != null))
            {
                return string.Equals(str, str2, StringComparison.OrdinalIgnoreCase);
            }
            if ((a != null) && (b != null))
            {
                return a.Equals(b);
            }
            return (a == b);
        }

        private static string UrlEncode(string str)
        {
            return Regex.Replace(Uri.EscapeUriString(str), "([#;?:@&=+$,])", new MatchEvaluator(ParsedRoute.EscapeReservedCharacters));
        }

        // Properties
        private IList<PathSegment> PathSegments { get; set; }
    }



}
