#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
// 从System.Web.Routing名称空间提取出来的类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Routing
{
    public static class RouteParser
    {
        // Methods
        private static string GetLiteral(string segmentLiteral)
        {
            string str = segmentLiteral.Replace("{{", "").Replace("}}", "");
            if (!str.Contains("{") && !str.Contains("}"))
            {
                return segmentLiteral.Replace("{{", "{").Replace("}}", "}");
            }
            return null;
        }

        private static int IndexOfFirstOpenParameter(string segment, int startIndex)
        {
            while (true)
            {
                startIndex = segment.IndexOf('{', startIndex);
                if (startIndex == -1)
                {
                    return -1;
                }
                if (((startIndex + 1) == segment.Length) || (((startIndex + 1) < segment.Length) && (segment[startIndex + 1] != '{')))
                {
                    return startIndex;
                }
                startIndex += 2;
            }
        }

        internal static bool IsSeparator(string s)
        {
            return string.Equals(s, "/", StringComparison.Ordinal);
        }

        private static bool IsValidParameterName(string parameterName)
        {
            if (parameterName.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < parameterName.Length; i++)
            {
                switch (parameterName[i])
                {
                    case '/':
                    case '{':
                    case '}':
                        return false;
                }
            }
            return true;
        }

        public static ParsedRoute Parse(string routeUrl)
        {
            if (routeUrl == null)
            {
                routeUrl = string.Empty;
            }
            if ((routeUrl.StartsWith("~", StringComparison.Ordinal) || routeUrl.StartsWith("/", StringComparison.Ordinal)) || (routeUrl.IndexOf('?') != -1))
            {
                throw new ArgumentException("Route_InvalidRouteUrl", "routeUrl");
            }
            IList<string> pathSegments = SplitUrlToPathSegmentStrings(routeUrl);
            Exception exception = ValidateUrlParts(pathSegments);
            if (exception != null)
            {
                throw exception;
            }
            return new ParsedRoute(SplitUrlToPathSegments(pathSegments));
        }

        private static IList<PathSubsegment> ParseUrlSegment(string segment, out Exception exception)
        {
            int startIndex = 0;
            List<PathSubsegment> list = new List<PathSubsegment>();
            while (startIndex < segment.Length)
            {
                int num2 = IndexOfFirstOpenParameter(segment, startIndex);
                if (num2 == -1)
                {
                    string str = GetLiteral(segment.Substring(startIndex));
                    if (str == null)
                    {
                        exception = new ArgumentException("Route_MismatchedParameter", "routeUrl");
                        return null;
                    }
                    if (str.Length > 0)
                    {
                        list.Add(new LiteralSubsegment(str));
                    }
                    break;
                }
                int index = segment.IndexOf('}', num2 + 1);
                if (index == -1)
                {
                    exception = new ArgumentException("Route_MismatchedParameter", "routeUrl");
                    return null;
                }
                string literal = GetLiteral(segment.Substring(startIndex, num2 - startIndex));
                if (literal == null)
                {
                    exception = new ArgumentException("Route_MismatchedParameter", "routeUrl");
                    return null;
                }
                if (literal.Length > 0)
                {
                    list.Add(new LiteralSubsegment(literal));
                }
                string parameterName = segment.Substring(num2 + 1, (index - num2) - 1);
                list.Add(new ParameterSubsegment(parameterName));
                startIndex = index + 1;
            }
            exception = null;
            return list;
        }

        private static IList<PathSegment> SplitUrlToPathSegments(IList<string> urlParts)
        {
            List<PathSegment> list = new List<PathSegment>();
            foreach (string str in urlParts)
            {
                if (IsSeparator(str))
                {
                    list.Add(new SeparatorPathSegment());
                }
                else
                {
                    Exception exception;
                    IList<PathSubsegment> subsegments = ParseUrlSegment(str, out exception);
                    list.Add(new ContentPathSegment(subsegments));
                }
            }
            return list;
        }

        internal static IList<string> SplitUrlToPathSegmentStrings(string url)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(url))
            {
                int index;
                for (int i = 0; i < url.Length; i = index + 1)
                {
                    index = url.IndexOf('/', i);
                    if (index == -1)
                    {
                        string str = url.Substring(i);
                        if (str.Length > 0)
                        {
                            list.Add(str);
                        }
                        return list;
                    }
                    string item = url.Substring(i, index - i);
                    if (item.Length > 0)
                    {
                        list.Add(item);
                    }
                    list.Add("/");
                }
            }
            return list;
        }

        private static Exception ValidateUrlParts(IList<string> pathSegments)
        {
            HashSet<string> usedParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            bool? nullable = null;
            bool flag = false;
            foreach (string str in pathSegments)
            {
                bool flag2;
                if (flag)
                {
                    return new ArgumentException("Route_CatchAllMustBeLast", "routeUrl");
                }
                if (!nullable.HasValue)
                {
                    nullable = new bool?(IsSeparator(str));
                    flag2 = nullable.Value;
                }
                else
                {
                    flag2 = IsSeparator(str);
                    if (flag2 && nullable.Value)
                    {
                        return new ArgumentException("Route_CannotHaveConsecutiveSeparators", "routeUrl");
                    }
                    nullable = new bool?(flag2);
                }
                if (!flag2)
                {
                    Exception exception;
                    IList<PathSubsegment> pathSubsegments = ParseUrlSegment(str, out exception);
                    if (exception != null)
                    {
                        return exception;
                    }
                    exception = ValidateUrlSegment(pathSubsegments, usedParameterNames, str);
                    if (exception != null)
                    {
                        return exception;
                    }
                    flag = pathSubsegments.Any<PathSubsegment>(seg => (seg is ParameterSubsegment) && ((ParameterSubsegment)seg).IsCatchAll);
                }
            }
            return null;
        }

        private static Exception ValidateUrlSegment(IList<PathSubsegment> pathSubsegments, HashSet<string> usedParameterNames, string pathSegment)
        {
            bool flag = false;
            Type type = null;
            foreach (PathSubsegment subsegment in pathSubsegments)
            {
                if ((type != null) && (type == subsegment.GetType()))
                {
                    return new ArgumentException("Route_CannotHaveConsecutiveParameters", "routeUrl");
                }
                type = subsegment.GetType();
                if (!(subsegment is LiteralSubsegment))
                {
                    ParameterSubsegment subsegment3 = subsegment as ParameterSubsegment;
                    if (subsegment3 != null)
                    {
                        string parameterName = subsegment3.ParameterName;
                        if (subsegment3.IsCatchAll)
                        {
                            flag = true;
                        }
                        if (!IsValidParameterName(parameterName))
                        {
                            return new ArgumentException("Route_InvalidParameterName", "routeUrl");
                        }
                        if (usedParameterNames.Contains(parameterName))
                        {
                            return new ArgumentException("Route_RepeatedParameter", "routeUrl");
                        }
                        usedParameterNames.Add(parameterName);
                    }
                }
            }
            if (flag && (pathSubsegments.Count != 1))
            {
                return new ArgumentException("Route_CannotHaveCatchAllInMultiSegment", "routeUrl");
            }
            return null;
        }
    }

}
