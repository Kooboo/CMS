using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections;

namespace Kooboo.Web
{
    public class HttpBrowserCapabilitiesBaseWrapper : HttpBrowserCapabilitiesBase
    {
        // Fields
        private HttpBrowserCapabilitiesBase _browser;

        // Methods
        public HttpBrowserCapabilitiesBaseWrapper(HttpBrowserCapabilitiesBase httpBrowserCapabilities)
        {
            if (httpBrowserCapabilities == null)
            {
                throw new ArgumentNullException("httpBrowserCapabilities");
            }
            this._browser = httpBrowserCapabilities;
        }

        public override void AddBrowser(string browserName)
        {
            this._browser.AddBrowser(browserName);
        }

        public override int CompareFilters(string filter1, string filter2)
        {
            return ((IFilterResolutionService)this._browser).CompareFilters(filter1, filter2);
        }

        public override HtmlTextWriter CreateHtmlTextWriter(TextWriter w)
        {
            return this._browser.CreateHtmlTextWriter(w);
        }

        public override void DisableOptimizedCacheKey()
        {
            this._browser.DisableOptimizedCacheKey();
        }

        public override bool EvaluateFilter(string filterName)
        {
            return ((IFilterResolutionService)this._browser).EvaluateFilter(filterName);
        }

        public override Version[] GetClrVersions()
        {
            return this._browser.GetClrVersions();
        }

        public override bool IsBrowser(string browserName)
        {
            return this._browser.IsBrowser(browserName);
        }

        // Properties
        public override bool ActiveXControls
        {
            get
            {
                return this._browser.ActiveXControls;
            }
        }

        public override IDictionary Adapters
        {
            get
            {
                return this._browser.Adapters;
            }
        }

        public override bool AOL
        {
            get
            {
                return this._browser.AOL;
            }
        }

        public override bool BackgroundSounds
        {
            get
            {
                return this._browser.BackgroundSounds;
            }
        }

        public override bool Beta
        {
            get
            {
                return this._browser.Beta;
            }
        }

        public override string Browser
        {
            get
            {
                return this._browser.Browser;
            }
        }

        public override ArrayList Browsers
        {
            get
            {
                return this._browser.Browsers;
            }
        }

        public override bool CanCombineFormsInDeck
        {
            get
            {
                return this._browser.CanCombineFormsInDeck;
            }
        }

        public override bool CanInitiateVoiceCall
        {
            get
            {
                return this._browser.CanInitiateVoiceCall;
            }
        }

        public override bool CanRenderAfterInputOrSelectElement
        {
            get
            {
                return this._browser.CanRenderAfterInputOrSelectElement;
            }
        }

        public override bool CanRenderEmptySelects
        {
            get
            {
                return this._browser.CanRenderEmptySelects;
            }
        }

        public override bool CanRenderInputAndSelectElementsTogether
        {
            get
            {
                return this._browser.CanRenderInputAndSelectElementsTogether;
            }
        }

        public override bool CanRenderMixedSelects
        {
            get
            {
                return this._browser.CanRenderMixedSelects;
            }
        }

        public override bool CanRenderOneventAndPrevElementsTogether
        {
            get
            {
                return this._browser.CanRenderOneventAndPrevElementsTogether;
            }
        }

        public override bool CanRenderPostBackCards
        {
            get
            {
                return this._browser.CanRenderPostBackCards;
            }
        }

        public override bool CanRenderSetvarZeroWithMultiSelectionList
        {
            get
            {
                return this._browser.CanRenderSetvarZeroWithMultiSelectionList;
            }
        }

        public override bool CanSendMail
        {
            get
            {
                return this._browser.CanSendMail;
            }
        }

        public override IDictionary Capabilities
        {
            get
            {
                return this._browser.Capabilities;
            }
            set
            {
                this._browser.Capabilities = value;
            }
        }

        public override bool CDF
        {
            get
            {
                return this._browser.CDF;
            }
        }

        public override Version ClrVersion
        {
            get
            {
                return this._browser.ClrVersion;
            }
        }

        public override bool Cookies
        {
            get
            {
                return this._browser.Cookies;
            }
        }

        public override bool Crawler
        {
            get
            {
                return this._browser.Crawler;
            }
        }

        public override int DefaultSubmitButtonLimit
        {
            get
            {
                return this._browser.DefaultSubmitButtonLimit;
            }
        }

        public override Version EcmaScriptVersion
        {
            get
            {
                return this._browser.EcmaScriptVersion;
            }
        }

        public override bool Frames
        {
            get
            {
                return this._browser.Frames;
            }
        }

        public override int GatewayMajorVersion
        {
            get
            {
                return this._browser.GatewayMajorVersion;
            }
        }

        public override double GatewayMinorVersion
        {
            get
            {
                return this._browser.GatewayMinorVersion;
            }
        }

        public override string GatewayVersion
        {
            get
            {
                return this._browser.GatewayVersion;
            }
        }

        public override bool HasBackButton
        {
            get
            {
                return this._browser.HasBackButton;
            }
        }

        public override bool HidesRightAlignedMultiselectScrollbars
        {
            get
            {
                return this._browser.HidesRightAlignedMultiselectScrollbars;
            }
        }

        public override string HtmlTextWriter
        {
            get
            {
                return this._browser.HtmlTextWriter;
            }
            set
            {
                this._browser.HtmlTextWriter = value;
            }
        }

        public override string Id
        {
            get
            {
                return this._browser.Id;
            }
        }

        public override string InputType
        {
            get
            {
                return this._browser.InputType;
            }
        }

        public override bool IsColor
        {
            get
            {
                return this._browser.IsColor;
            }
        }

        public override bool IsMobileDevice
        {
            get
            {
                return this._browser.IsMobileDevice;
            }
        }

        public override string this[string key]
        {
            get
            {
                return this._browser[key];
            }
        }

        public override bool JavaApplets
        {
            get
            {
                return this._browser.JavaApplets;
            }
        }

        public override Version JScriptVersion
        {
            get
            {
                return this._browser.JScriptVersion;
            }
        }

        public override int MajorVersion
        {
            get
            {
                return this._browser.MajorVersion;
            }
        }

        public override int MaximumHrefLength
        {
            get
            {
                return this._browser.MaximumHrefLength;
            }
        }

        public override int MaximumRenderedPageSize
        {
            get
            {
                return this._browser.MaximumRenderedPageSize;
            }
        }

        public override int MaximumSoftkeyLabelLength
        {
            get
            {
                return this._browser.MaximumSoftkeyLabelLength;
            }
        }

        public override double MinorVersion
        {
            get
            {
                return this._browser.MinorVersion;
            }
        }

        public override string MinorVersionString
        {
            get
            {
                return this._browser.MinorVersionString;
            }
        }

        public override string MobileDeviceManufacturer
        {
            get
            {
                return this._browser.MobileDeviceManufacturer;
            }
        }

        public override string MobileDeviceModel
        {
            get
            {
                return this._browser.MobileDeviceModel;
            }
        }

        public override Version MSDomVersion
        {
            get
            {
                return this._browser.MSDomVersion;
            }
        }

        public override int NumberOfSoftkeys
        {
            get
            {
                return this._browser.NumberOfSoftkeys;
            }
        }

        public override string Platform
        {
            get
            {
                return this._browser.Platform;
            }
        }

        public override string PreferredImageMime
        {
            get
            {
                return this._browser.PreferredImageMime;
            }
        }

        public override string PreferredRenderingMime
        {
            get
            {
                return this._browser.PreferredRenderingMime;
            }
        }

        public override string PreferredRenderingType
        {
            get
            {
                return this._browser.PreferredRenderingType;
            }
        }

        public override string PreferredRequestEncoding
        {
            get
            {
                return this._browser.PreferredRequestEncoding;
            }
        }

        public override string PreferredResponseEncoding
        {
            get
            {
                return this._browser.PreferredResponseEncoding;
            }
        }

        public override bool RendersBreakBeforeWmlSelectAndInput
        {
            get
            {
                return this._browser.RendersBreakBeforeWmlSelectAndInput;
            }
        }

        public override bool RendersBreaksAfterHtmlLists
        {
            get
            {
                return this._browser.RendersBreaksAfterHtmlLists;
            }
        }

        public override bool RendersBreaksAfterWmlAnchor
        {
            get
            {
                return this._browser.RendersBreaksAfterWmlAnchor;
            }
        }

        public override bool RendersBreaksAfterWmlInput
        {
            get
            {
                return this._browser.RendersBreaksAfterWmlInput;
            }
        }

        public override bool RendersWmlDoAcceptsInline
        {
            get
            {
                return this._browser.RendersWmlDoAcceptsInline;
            }
        }

        public override bool RendersWmlSelectsAsMenuCards
        {
            get
            {
                return this._browser.RendersWmlSelectsAsMenuCards;
            }
        }

        public override string RequiredMetaTagNameValue
        {
            get
            {
                return this._browser.RequiredMetaTagNameValue;
            }
        }

        public override bool RequiresAttributeColonSubstitution
        {
            get
            {
                return this._browser.RequiresAttributeColonSubstitution;
            }
        }

        public override bool RequiresContentTypeMetaTag
        {
            get
            {
                return this._browser.RequiresContentTypeMetaTag;
            }
        }

        public override bool RequiresControlStateInSession
        {
            get
            {
                return this._browser.RequiresControlStateInSession;
            }
        }

        public override bool RequiresDBCSCharacter
        {
            get
            {
                return this._browser.RequiresDBCSCharacter;
            }
        }

        public override bool RequiresHtmlAdaptiveErrorReporting
        {
            get
            {
                return this._browser.RequiresHtmlAdaptiveErrorReporting;
            }
        }

        public override bool RequiresLeadingPageBreak
        {
            get
            {
                return this._browser.RequiresLeadingPageBreak;
            }
        }

        public override bool RequiresNoBreakInFormatting
        {
            get
            {
                return this._browser.RequiresNoBreakInFormatting;
            }
        }

        public override bool RequiresOutputOptimization
        {
            get
            {
                return this._browser.RequiresOutputOptimization;
            }
        }

        public override bool RequiresPhoneNumbersAsPlainText
        {
            get
            {
                return this._browser.RequiresPhoneNumbersAsPlainText;
            }
        }

        public override bool RequiresSpecialViewStateEncoding
        {
            get
            {
                return this._browser.RequiresSpecialViewStateEncoding;
            }
        }

        public override bool RequiresUniqueFilePathSuffix
        {
            get
            {
                return this._browser.RequiresUniqueFilePathSuffix;
            }
        }

        public override bool RequiresUniqueHtmlCheckboxNames
        {
            get
            {
                return this._browser.RequiresUniqueHtmlCheckboxNames;
            }
        }

        public override bool RequiresUniqueHtmlInputNames
        {
            get
            {
                return this._browser.RequiresUniqueHtmlInputNames;
            }
        }

        public override bool RequiresUrlEncodedPostfieldValues
        {
            get
            {
                return this._browser.RequiresUrlEncodedPostfieldValues;
            }
        }

        public override int ScreenBitDepth
        {
            get
            {
                return this._browser.ScreenBitDepth;
            }
        }

        public override int ScreenCharactersHeight
        {
            get
            {
                return this._browser.ScreenCharactersHeight;
            }
        }

        public override int ScreenCharactersWidth
        {
            get
            {
                return this._browser.ScreenCharactersWidth;
            }
        }

        public override int ScreenPixelsHeight
        {
            get
            {
                return this._browser.ScreenPixelsHeight;
            }
        }

        public override int ScreenPixelsWidth
        {
            get
            {
                return this._browser.ScreenPixelsWidth;
            }
        }

        public override bool SupportsAccesskeyAttribute
        {
            get
            {
                return this._browser.SupportsAccesskeyAttribute;
            }
        }

        public override bool SupportsBodyColor
        {
            get
            {
                return this._browser.SupportsBodyColor;
            }
        }

        public override bool SupportsBold
        {
            get
            {
                return this._browser.SupportsBold;
            }
        }

        public override bool SupportsCacheControlMetaTag
        {
            get
            {
                return this._browser.SupportsCacheControlMetaTag;
            }
        }

        public override bool SupportsCallback
        {
            get
            {
                return this._browser.SupportsCallback;
            }
        }

        public override bool SupportsCss
        {
            get
            {
                return this._browser.SupportsCss;
            }
        }

        public override bool SupportsDivAlign
        {
            get
            {
                return this._browser.SupportsDivAlign;
            }
        }

        public override bool SupportsDivNoWrap
        {
            get
            {
                return this._browser.SupportsDivNoWrap;
            }
        }

        public override bool SupportsEmptyStringInCookieValue
        {
            get
            {
                return this._browser.SupportsEmptyStringInCookieValue;
            }
        }

        public override bool SupportsFontColor
        {
            get
            {
                return this._browser.SupportsFontColor;
            }
        }

        public override bool SupportsFontName
        {
            get
            {
                return this._browser.SupportsFontName;
            }
        }

        public override bool SupportsFontSize
        {
            get
            {
                return this._browser.SupportsFontSize;
            }
        }

        public override bool SupportsImageSubmit
        {
            get
            {
                return this._browser.SupportsImageSubmit;
            }
        }

        public override bool SupportsIModeSymbols
        {
            get
            {
                return this._browser.SupportsIModeSymbols;
            }
        }

        public override bool SupportsInputIStyle
        {
            get
            {
                return this._browser.SupportsInputIStyle;
            }
        }

        public override bool SupportsInputMode
        {
            get
            {
                return this._browser.SupportsInputMode;
            }
        }

        public override bool SupportsItalic
        {
            get
            {
                return this._browser.SupportsItalic;
            }
        }

        public override bool SupportsJPhoneMultiMediaAttributes
        {
            get
            {
                return this._browser.SupportsJPhoneMultiMediaAttributes;
            }
        }

        public override bool SupportsJPhoneSymbols
        {
            get
            {
                return this._browser.SupportsJPhoneSymbols;
            }
        }

        public override bool SupportsQueryStringInFormAction
        {
            get
            {
                return this._browser.SupportsQueryStringInFormAction;
            }
        }

        public override bool SupportsRedirectWithCookie
        {
            get
            {
                return this._browser.SupportsRedirectWithCookie;
            }
        }

        public override bool SupportsSelectMultiple
        {
            get
            {
                return this._browser.SupportsSelectMultiple;
            }
        }

        public override bool SupportsUncheck
        {
            get
            {
                return this._browser.SupportsUncheck;
            }
        }

        public override bool SupportsXmlHttp
        {
            get
            {
                return this._browser.SupportsXmlHttp;
            }
        }

        public override bool Tables
        {
            get
            {
                return this._browser.Tables;
            }
        }

        public override Type TagWriter
        {
            get
            {
                return this._browser.TagWriter;
            }
        }

        public override string Type
        {
            get
            {
                return this._browser.Type;
            }
        }

        public override bool UseOptimizedCacheKey
        {
            get
            {
                return this._browser.UseOptimizedCacheKey;
            }
        }

        public override bool VBScript
        {
            get
            {
                return this._browser.VBScript;
            }
        }

        public override string Version
        {
            get
            {
                return this._browser.Version;
            }
        }

        public override Version W3CDomVersion
        {
            get
            {
                return this._browser.W3CDomVersion;
            }
        }

        public override bool Win16
        {
            get
            {
                return this._browser.Win16;
            }
        }

        public override bool Win32
        {
            get
            {
                return this._browser.Win32;
            }
        }
    }
}