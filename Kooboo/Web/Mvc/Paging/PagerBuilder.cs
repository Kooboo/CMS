/*
 ASP.NET MvcPager control
 Copyright:2009-2010 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
 */
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

namespace Kooboo.Web.Mvc.Paging
{
    public class PagerBuilder
    {
        private readonly HtmlHelper _html;
        private readonly AjaxHelper _ajax;
        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly PagerOptions _pagerOptions;
        private readonly RouteValueDictionary _routeValues;
        private readonly string _routeName;
        private readonly int _startPageIndex = 1;
        private readonly int _endPageIndex = 1;
        private readonly bool _msAjaxPaging;
        private readonly AjaxOptions _ajaxOptions;
        private IDictionary<string, object> _htmlAttributes;
        private const string CopyrightText = "";
        private const string JqueryScriptCheckItemKey = "_MvcPager_CheckjQueryScript";
        private const string ScriptPageIndexName = "*_MvcPager_PageIndex_*";
        private const string JqCheckScript = "if(typeof(jQuery)==\"undefined\"){alert(\"jQuery library not detected, please make sure it loaded prior to MvcPager!\");}";
        private const string GoToPageScript = "function _MvcPager_GoToPage(_pib,_mp){var pageIndex;if(_pib.tagName==\"SELECT\"){pageIndex=_pib.options[_pib.selectedIndex].value;}else{pageIndex=_pib.value;var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(!r.test(pageIndex)){alert(\"%InvalidPageIndexErrorMessage%\");return;}else if(RegExp.$1<1||RegExp.$1>_mp){alert(\"%PageIndexOutOfRangeErrorMessage%\");return;}}var _hl=document.getElementById(_pib.id+'link').childNodes[0];var _lh=_hl.href;_hl.href=_lh.replace('" + ScriptPageIndexName + "',pageIndex);%ClickHandler%;_hl.href=_lh;}";
        private const string KeyDownScript = "function _MvcPager_Keydown(e){var _kc,_pib;if(window.event){_kc=e.keyCode;_pib=e.srcElement;}else if(e.which){_kc=e.which;_pib=e.target;}var validKey=(_kc==8||_kc==46||_kc==37||_kc==39||(_kc>=48&&_kc<=57)||(_kc>=96&&_kc<=105));if(!validKey){if(_kc==13){ _MvcPager_GoToPage(_pib,%_pagedList.TotalPageCount%);}if(e.preventDefault){e.preventDefault();}else{event.returnValue=false;}}}";

        private IPagedList _pagedList;
        /// <summary>
        /// used when PagedList is null
        /// </summary>
        public PagerBuilder(HtmlHelper html, AjaxHelper ajax, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            _html = html;
            _ajax = ajax;
            _pagerOptions = pagerOptions;
            _htmlAttributes = htmlAttributes;
        }

        public PagerBuilder(HtmlHelper html, AjaxHelper ajax, string actionName, string controllerName,
            IPagedList pagedList, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            _msAjaxPaging = (ajax != null);
            if (String.IsNullOrEmpty(actionName))
            {
                if (ajax != null)
                    actionName = (string)ajax.ViewContext.RouteData.Values["action"];
                else
                    actionName = (string)html.ViewContext.RouteData.Values["action"];
            }
            if (String.IsNullOrEmpty(controllerName))
            {
                if (ajax != null)
                    controllerName = (string)ajax.ViewContext.RouteData.Values["controller"];
                else
                    controllerName = (string)html.ViewContext.RouteData.Values["controller"];
            }
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();

            _html = html;
            _ajax = ajax;
            _actionName = actionName;
            _controllerName = controllerName;
            _pagedList = pagedList;
            _pagerOptions = pagerOptions;
            _routeName = routeName;
            _routeValues = routeValues;
            _ajaxOptions = ajaxOptions;
            _htmlAttributes = htmlAttributes;

            // start page index
            _startPageIndex = pagedList.CurrentPageIndex - (pagerOptions.NumericPagerItemCount / 2);
            if (_startPageIndex + pagerOptions.NumericPagerItemCount > pagedList.TotalPageCount)
                _startPageIndex = pagedList.TotalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            if (_startPageIndex < 1)
                _startPageIndex = 1;

            // end page index
            _endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            if (_endPageIndex > pagedList.TotalPageCount)
                _endPageIndex = pagedList.TotalPageCount;
        }
        //non Ajax pager builder
        public PagerBuilder(HtmlHelper helper, string actionName, string controllerName, IPagedList pagedList
            , PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
            : this(helper, null, actionName, controllerName,
                pagedList, pagerOptions, routeName, routeValues, null, htmlAttributes) { }
        //Microsoft Ajax pager builder
        internal PagerBuilder(AjaxHelper helper, string actionName, string controllerName, IPagedList pagedList
            , PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
            : this(null, helper, actionName,
                controllerName, pagedList, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }
        //jQuery Ajax pager builder
        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, IPagedList pagedList
            , PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
            : this(helper, null, actionName,
                controllerName, pagedList, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }

        private void AddPrevious(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.PrevPageText, _pagedList.CurrentPageIndex - 1, _pagedList.CurrentPageIndex == 1, PagerItemType.PrevPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        private void AddFirst(ICollection<PagerItem> results)
        {
            PagerItem item = new PagerItem(_pagerOptions.FirstPageText, 1, _pagedList.CurrentPageIndex == 1, PagerItemType.FirstPage);

            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        private void AddMoreBefore(ICollection<PagerItem> results)
        {
            if (_startPageIndex > 1 && _pagerOptions.ShowMorePagerItems)
            {
                var index = _startPageIndex - 1;
                if (index < 1) index = 1;
                PagerItem item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddPageNumbers(ICollection<PagerItem> results)
        {
            for (var pageIndex = _startPageIndex; pageIndex <= _endPageIndex; pageIndex++)
            {
                var text = pageIndex.ToString();
                if (pageIndex == _pagedList.CurrentPageIndex && !string.IsNullOrEmpty(_pagerOptions.CurrentPageNumberFormatString))
                    text = String.Format(_pagerOptions.CurrentPageNumberFormatString, text);
                else if (!string.IsNullOrEmpty(_pagerOptions.PageNumberFormatString))
                    text = String.Format(_pagerOptions.PageNumberFormatString, text);
                var item = new PagerItem(text, pageIndex, false, PagerItemType.NumericPage);
                results.Add(item);
            }
        }

        private void AddMoreAfter(ICollection<PagerItem> results)
        {
            if (_endPageIndex < _pagedList.TotalPageCount)
            {
                var index = _startPageIndex + _pagerOptions.NumericPagerItemCount;
                if (index > _pagedList.TotalPageCount) { index = _pagedList.TotalPageCount; }
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        private void AddNext(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.NextPageText, _pagedList.CurrentPageIndex + 1, _pagedList.CurrentPageIndex >= _pagedList.TotalPageCount, PagerItemType.NextPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        private void AddLast(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.LastPageText, _pagedList.TotalPageCount, _pagedList.CurrentPageIndex >= _pagedList.TotalPageCount, PagerItemType.LastPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        /// <summary>
        /// generate paging url
        /// </summary>
        /// <param name="pageIndex">page index to generate navigate url</param>
        /// <returns>navigated url for pager item</returns>
        protected virtual string GenerateUrl(int pageIndex)
        {
            //return null if  page index larger than total page count or page index is current page index
            if (pageIndex > _pagedList.TotalPageCount || pageIndex == _pagedList.CurrentPageIndex)
                return null;

            var routeValues = _routeValues ?? new RouteValueDictionary();

            // set route value of page index parameter name in url,pageIndex==0 is a special case
            if (pageIndex == 0)
                routeValues[_pagerOptions.PageIndexParameterName] = ScriptPageIndexName;
            else
                routeValues[_pagerOptions.PageIndexParameterName] = pageIndex;

            var rq = _html.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in rq.Keys)
            {
                if (key != _pagerOptions.PageIndexParameterName)
                    routeValues[key] = rq[key];
            }
            // Add action
            routeValues["action"] = _actionName;

            // Add controller
            routeValues["controller"] = _controllerName;

            // Return link
            var urlHelper = new UrlHelper(_html.ViewContext.RequestContext);
            if (!string.IsNullOrEmpty(_routeName))
                return urlHelper.RouteUrl(_routeName, routeValues);
            return urlHelper.RouteUrl(routeValues);
        }

        /// <summary>
        /// render paging control
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString RenderPager()
        {
            if (_pagedList == null)
            {
                return MvcHtmlString.Create("");
            }

            string statisticsText = "";
            if (!string.IsNullOrEmpty(this._pagerOptions.StatisticsTextFormatString))
            {
                statisticsText = string.Format(this._pagerOptions.StatisticsTextFormatString
                    , this._pagedList.StartRecordIndex
                    , this._pagedList.EndRecordIndex
                    , this._pagedList.TotalItemCount);
            }


            //return null if total page count less than or equal to 1
            if (_pagedList.TotalPageCount <= 1 && _pagerOptions.AutoHide)
                return MvcHtmlString.Create(CopyrightText);
            //Display error message if pageIndex out of range
            if ((_pagedList.CurrentPageIndex > _pagedList.TotalPageCount && _pagedList.TotalPageCount > 0) || _pagedList.CurrentPageIndex < 1)
            {
                return
                    MvcHtmlString.Create(string.Format("{0}<div style=\"color:red;font-weight:bold\">{1}</div>{0}",
                                         CopyrightText, _pagerOptions.PageIndexOutOfRangeErrorMessage));
            }           

            var pagerItems = new List<PagerItem>();
            //First page
            if (_pagerOptions.ShowFirstLast)
                AddFirst(pagerItems);

            // Prev page
            if (_pagerOptions.ShowPrevNext)
                AddPrevious(pagerItems);

            if (_pagerOptions.ShowNumericPagerItems)
            {
                if (_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 1)
                    pagerItems.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));

                // more page before numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreBefore(pagerItems);

                // numeric page
                AddPageNumbers(pagerItems);

                // more page after numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreAfter(pagerItems);

                if (_pagerOptions.AlwaysShowFirstLastPageNumber && _endPageIndex < _pagedList.TotalPageCount)
                    pagerItems.Add(new PagerItem(_pagedList.TotalPageCount.ToString(), _pagedList.TotalPageCount, false,
                                                 PagerItemType.NumericPage));
            }

            // Next page
            if (_pagerOptions.ShowPrevNext)
                AddNext(pagerItems);

            //Last page
            if (_pagerOptions.ShowFirstLast)
                AddLast(pagerItems);

            var sb = new StringBuilder();
            if (_msAjaxPaging)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateMsAjaxPagerElement(item));
                }
            }
            else if (_pagerOptions.UseJqueryAjax)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateJqAjaxPagerElement(item));
                }
            }
            else
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GeneratePagerElement(item));
                }
            }
            var tb = new TagBuilder(_pagerOptions.ContainerTagName);
            if (!string.IsNullOrEmpty(_pagerOptions.Id))
                tb.GenerateId(_pagerOptions.Id);
            if (!string.IsNullOrEmpty(_pagerOptions.CssClass))
                tb.AddCssClass(_pagerOptions.CssClass);
            if (!string.IsNullOrEmpty(_pagerOptions.HorizontalAlign))
            {
                string strAlign = "text-align:" + _pagerOptions.HorizontalAlign.ToLower();
                if (_htmlAttributes == null)
                    _htmlAttributes = new RouteValueDictionary { { "style", strAlign } };
                else
                {
                    if (_htmlAttributes.Keys.Contains("style"))
                        _htmlAttributes["style"] += ";" + strAlign;
                }
            }
            tb.MergeAttributes(_htmlAttributes, true);
            string pagerScript = string.Empty;
            if (_pagerOptions.UseJqueryAjax &&
                (string)_html.ViewContext.HttpContext.Items[JqueryScriptCheckItemKey] != "1")
            {
                pagerScript = JqCheckScript;
                _html.ViewContext.HttpContext.Items[JqueryScriptCheckItemKey] = "1";
            }
            if (_pagerOptions.ShowPageIndexBox)
            {
                sb.Append(BuildGoToPageSection(ref pagerScript));
            }
            else
                sb.Length -= _pagerOptions.SeparatorHtml.Length;
            tb.InnerHtml = statisticsText + sb.ToString();
            if (!string.IsNullOrEmpty(pagerScript))
                pagerScript = "<script language=\"javascript\" type=\"text/javascript\">" + pagerScript + "</script>";
            return MvcHtmlString.Create(CopyrightText + pagerScript + tb.ToString(TagRenderMode.Normal) + CopyrightText);
        }

        private string BuildGoToPageSection(ref string pagerScript)
        {
            const string ctrlIndexName = "_MvcPager_ControlIndex";
            ViewContext viewCtx = _msAjaxPaging
                                      ? _ajax.ViewContext
                                      : _html.ViewContext;
            int ctrlIndex;
            if (int.TryParse((string)viewCtx.HttpContext.Items[ctrlIndexName], out ctrlIndex))
                ++ctrlIndex;
            viewCtx.HttpContext.Items[ctrlIndexName] = ctrlIndex.ToString();

            string controlId = "_MvcPager_Ctrl" + ctrlIndex;
            string scriptLink = GenerateAnchor(new PagerItem("0", 0, false, PagerItemType.NumericPage));
            string linkClickTrigger = "_hl.click()";
            string browserName = viewCtx.HttpContext.Request.Browser.Browser.ToLower();
            if (browserName.Contains("safari") || browserName.Contains("firefox"))
                linkClickTrigger =
                    "var evt=document.createEvent('MouseEvents');evt.initEvent('click',true,true);_hl.dispatchEvent(evt)";

            if (ctrlIndex == 0)
            {
                pagerScript += KeyDownScript.Replace("%_pagedList.TotalPageCount%", _pagedList.TotalPageCount.ToString()) + GoToPageScript.Replace("%InvalidPageIndexErrorMessage%",
                                                                      _pagerOptions.InvalidPageIndexErrorMessage).Replace("%PageIndexOutOfRangeErrorMessage%",
                                                       _pagerOptions.PageIndexOutOfRangeErrorMessage).Replace("%ClickHandler%", linkClickTrigger);
            }
            string onChangeScript = null;
            if (!_pagerOptions.ShowGoButton)
                onChangeScript = " onchange=\"_MvcPager_GoToPage(this," + _pagedList.TotalPageCount + ")\"";
            StringBuilder piBuilder = new StringBuilder();
            if (_pagerOptions.PageIndexBoxType == PageIndexBoxType.DropDownList)
            {
                // start page index
                int startIndex = _pagedList.CurrentPageIndex - (_pagerOptions.MaximumPageIndexItems / 2);
                if (startIndex + _pagerOptions.MaximumPageIndexItems > _pagedList.TotalPageCount)
                    startIndex = _pagedList.TotalPageCount + 1 - _pagerOptions.MaximumPageIndexItems;
                if (startIndex < 1)
                    startIndex = 1;

                // end page index
                int endIndex = startIndex + _pagerOptions.MaximumPageIndexItems - 1;
                if (endIndex > _pagedList.TotalPageCount)
                    endIndex = _pagedList.TotalPageCount;
                piBuilder.AppendFormat("<select id=\"{0}\"{1}>", controlId + "_pib", onChangeScript);
                for (int i = startIndex; i <= endIndex; i++)
                {
                    piBuilder.AppendFormat("<option value=\"{0}\"", i);
                    if (i == _pagedList.CurrentPageIndex)
                        piBuilder.Append(" selected=\"selected\"");
                    piBuilder.AppendFormat(">{0}</option>", i);
                }
                piBuilder.Append("</select>");
            }
            else
                piBuilder.AppendFormat(
                    "<input type=\"text\" id=\"{0}\" value=\"{1}\" onkeydown=\"_MvcPager_Keydown(event)\"{2}/>",
                    controlId + "_pib", _pagedList.CurrentPageIndex, onChangeScript);
            string outHtml;
            if (!string.IsNullOrEmpty(_pagerOptions.PageIndexBoxWrapperFormatString))
            {
                outHtml = string.Format(_pagerOptions.PageIndexBoxWrapperFormatString, piBuilder);
                piBuilder = new StringBuilder(outHtml);
            }

            if (_pagerOptions.ShowGoButton)
                piBuilder.AppendFormat(
                    "<input type=\"button\" value=\"{0}\" onclick=\"_MvcPager_GoToPage(document.getElementById('{1}')," + _pagedList.TotalPageCount + ")\"/>",
                    _pagerOptions.GoButtonText
                    , controlId + "_pib");
            piBuilder.AppendFormat("<span id=\"{0}\" style=\"display:none;width:0px;height:0px\">{1}</span>",
                                   controlId + "_piblink", scriptLink);
            if (!string.IsNullOrEmpty(_pagerOptions.GoToPageSectionWrapperFormatString) ||
                !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
            {
                outHtml = string.Format(
                    _pagerOptions.GoToPageSectionWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString,
                    piBuilder);
            }
            else
                outHtml = piBuilder.ToString();
            return outHtml;
        }


        private string GenerateAnchor(PagerItem item)
        {
            if (_msAjaxPaging)
            {
                var routeValues = GetCurrentRouteValues(_ajax.ViewContext);
                if (item.PageIndex == 0)
                    routeValues[_pagerOptions.PageIndexParameterName] = ScriptPageIndexName;
                else
                    routeValues[_pagerOptions.PageIndexParameterName] = item.PageIndex;
                if (!string.IsNullOrEmpty(_routeName))
                    return _ajax.RouteLink(item.Text, _routeName, routeValues, _ajaxOptions).ToString();
                return _ajax.RouteLink(item.Text, routeValues, _ajaxOptions).ToString();
            }
            string url = GenerateUrl(item.PageIndex);
            if (_pagerOptions.UseJqueryAjax)
            {
                StringBuilder scriptBuilder = new StringBuilder();
                //ignore OnSuccess property
                if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure) || !string.IsNullOrEmpty(_ajaxOptions.OnBegin) || (!string.IsNullOrEmpty(_ajaxOptions.OnComplete) && _ajaxOptions.HttpMethod.ToUpper() != "GET"))
                {
                    scriptBuilder.Append("$.ajax({type:\'").Append(_ajaxOptions.HttpMethod.ToUpper() == "GET" ? "get" : "post");
                    scriptBuilder.Append("\',url:$(this).attr(\'href\'),success:function(data,status,xhr){$(\'#");
                    scriptBuilder.Append(_ajaxOptions.UpdateTargetId).Append("\').html(data);}");
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure))
                        scriptBuilder.Append(",error:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnFailure));
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnBegin))
                        scriptBuilder.Append(",beforeSend:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnBegin));
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                        scriptBuilder.Append(",complete:").Append(
                            HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                    scriptBuilder.Append("});return false;");
                }
                else
                {
                    if (_ajaxOptions.HttpMethod.ToUpper() == "GET")
                    {
                        scriptBuilder.Append("$(\'#").Append(_ajaxOptions.UpdateTargetId);
                        scriptBuilder.Append("\').load($(this).attr(\'href\')");
                        if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                            scriptBuilder.Append(",").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                        scriptBuilder.Append(");return false;");
                    }
                    else
                    {
                        scriptBuilder.Append("$.post($(this).attr(\'href\'), function(data) {$(\'#");
                        scriptBuilder.Append(_ajaxOptions.UpdateTargetId);
                        scriptBuilder.Append("\').html(data);});return false;");
                    }
                }
                return string.IsNullOrEmpty(url)
                           ? _html.Encode(item.Text)
                           : String.Format(CultureInfo.InvariantCulture,
                                           "<a href=\"{0}\" onclick=\"{1}\">{2}</a>",
                                           url, scriptBuilder, item.Text);
            }
            return "<a href=\"" + url +
                   "\" onclick=\"window.open(this.attributes.getNamedItem('href').value,'_self')\"></a>";
        }

        private MvcHtmlString GeneratePagerElement(PagerItem item)
        {
            //pager item link
            string url = GenerateUrl(item.PageIndex);
            if (item.Disabled) //first,last,next or previous page
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\" class=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item,
                                             string.IsNullOrEmpty(url)
                                                 ? _html.Encode(item.Text)
                                                 : String.Format("<a href='{0}'>{1}</a>", url, item.Text));
        }

        private MvcHtmlString GenerateJqAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\" class=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        private MvcHtmlString GenerateMsAjaxPagerElement(PagerItem item)
        {
            if (item.PageIndex == _pagedList.CurrentPageIndex && !item.Disabled) //current page index
                return CreateWrappedPagerElement(item, item.Text);
            if (item.Disabled)
                return CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\" class=\"disabled\">{0}</a>", item.Text));

            // return null if current page index less than 1 or large than total page count
            if (item.PageIndex < 1 || item.PageIndex > _pagedList.TotalPageCount)
                return null;

            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        private MvcHtmlString CreateWrappedPagerElement(PagerItem item, string el)
        {
            string navStr = el;
            switch (item.Type)
            {
                case PagerItemType.FirstPage:
                case PagerItemType.LastPage:
                case PagerItemType.NextPage:
                case PagerItemType.PrevPage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.NavigationPagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString)))
                        navStr =
                            string.Format(
                                _pagerOptions.NavigationPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
                case PagerItemType.MorePage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.MorePagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString)))
                        navStr =
                            string.Format(
                                _pagerOptions.MorePagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
                case PagerItemType.NumericPage:
                    if (item.PageIndex == _pagedList.CurrentPageIndex &&
                        (!string.IsNullOrEmpty(_pagerOptions.CurrentPagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))) //current page
                        navStr =
                            string.Format(
                                _pagerOptions.CurrentPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    else if (!string.IsNullOrEmpty(_pagerOptions.NumericPagerItemWrapperFormatString) ||
                             !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
                        navStr =
                            string.Format(
                                _pagerOptions.NumericPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
            }
            return MvcHtmlString.Create(navStr + _pagerOptions.SeparatorHtml);
        }


        private RouteValueDictionary GetCurrentRouteValues(ViewContext viewContext)
        {
            var routeValues = _routeValues ?? new RouteValueDictionary();
            var rq = viewContext.HttpContext.Request.QueryString;
            foreach (string key in rq.Keys)
            {
                // add other url query string parameters (not include PageIndexParameterName parameter value and X-Requested-With=XMLHttpRequest ajax parameter) to route value collection
                if (key != _pagerOptions.PageIndexParameterName &&
                    (key.ToLower() != "x-requested-with" && rq[key].ToLower() != "xmlhttprequest"))
                {
                    routeValues[key] = rq[key];
                }
            }
            // action
            routeValues["action"] = _actionName;
            // controller
            routeValues["controller"] = _controllerName;
            return routeValues;
        }
    }
}