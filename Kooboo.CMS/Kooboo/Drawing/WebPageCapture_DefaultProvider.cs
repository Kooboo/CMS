#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Windows.Forms;

namespace Kooboo.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultWebPageCaptureProviderFactory : IWebPageCaptureProviderFactory
    {
        public IWebPageCaptureProvider CreateProvider()
        {
            return new DefaultWebPageCaptureProvider();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultWebPageCaptureProvider : IWebPageCaptureProvider
    {
        #region Fields
        public EventHandler<WebPageCapturedEventArgs> _captured;

        private WebBrowser _browser;

        private static object SetCookiesLocker = new object();
        #endregion

        #region .ctor
        public DefaultWebPageCaptureProvider()
        {
            _browser = new WebBrowser();
            _browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
            _browser.ScrollBarsEnabled = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Navigates the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="captured">The captured.</param>
        public void Navigate(string url, HttpCookieCollection cookie, EventHandler<WebPageCapturedEventArgs> captured)
        {
            _captured = captured;

            if (cookie != null)
            {
                SetCookies(cookie, url);
            }

            _browser.Navigate(url);

            while (_browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _browser.Dispose();
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            Rectangle body = browser.Document.Body.ScrollRectangle;

            browser.Width = body.Width;
            browser.Height = body.Height;

            using (var bitmap = new Bitmap(body.Width, body.Height))
            {
                IViewObject ivo = browser.Document.DomDocument as IViewObject;

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //get the handle to the device context and draw
                    IntPtr hdc = g.GetHdc();
                    ivo.Draw(1, -1, IntPtr.Zero, IntPtr.Zero,
                                IntPtr.Zero, hdc, ref body,
                                ref body, IntPtr.Zero, 0);
                    g.ReleaseHdc(hdc);
                }

                if (_captured != null)
                {
                    WebPageCapturedEventArgs args = new WebPageCapturedEventArgs()
                    {
                        Image = new DefaultImage(bitmap)
                    };
                    _captured(this, args);
                }
            }
        }

        /// <summary>
        /// Sets the win INET cookie string.
        /// </summary>
        /// <param name="sURL">The s URL.</param>
        /// <param name="sName">Name of the s.</param>
        /// <param name="sData">The s data.</param>
        /// <returns></returns>
        public static bool SetWinINETCookieString(string sURL, string sName, string sData)
        {
            return InternetSetCookie(sURL, sName, sData);
        }



        /// <summary>
        /// Sets the cookies.
        /// </summary>
        /// <param name="cookies">The cookies.</param>
        /// <param name="url">The URL.</param>
        public static void SetCookies(HttpCookieCollection cookies, string url = null)
        {
            lock (SetCookiesLocker)
            {
                foreach (var key in cookies.AllKeys)
                {
                    var cookie = cookies[key];
                    SetWinINETCookieString(url ?? cookie.Path, cookie.Name, cookie.Value);
                }
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string url, string name, string data);

        #endregion

        #region DefaultImage Class

        /// <summary>
        /// 
        /// </summary>
        public class DefaultImage : IWebPageCaptureImage
        {
            private Image _image;
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultImage" /> class.
            /// </summary>
            /// <param name="image">The image.</param>
            public DefaultImage(Image image)
            {
                _image = image;
            }

            /// <summary>
            /// Saves the specified path.
            /// </summary>
            /// <param name="path">The path.</param>
            /// <param name="format">The format.</param>
            public void Save(string path, System.Drawing.Imaging.ImageFormat format)
            {
                _image.Save(path, format);
            }
        }
        #endregion

        #region IViewObject
        [ComVisible(true), ComImport()]
        [GuidAttribute("0000010d-0000-0000-C000-000000000046")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IViewObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Draw(
                [MarshalAs(UnmanagedType.U4)] UInt32 dwDrawAspect,
                int lindex,
                IntPtr pvAspect,
                [In] IntPtr ptd,
                IntPtr hdcTargetDev,
                IntPtr hdcDraw,
                [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcBounds,
                [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcWBounds,
                IntPtr pfnContinue,
                [MarshalAs(UnmanagedType.U4)] UInt32 dwContinue);
            [PreserveSig]
            int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
               int lindex, IntPtr pvAspect, [In] IntPtr ptd,
                IntPtr hicTargetDev, [Out] IntPtr ppColorSet);
            [PreserveSig]
            int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                            int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);
            [PreserveSig]
            int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);
            void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
              [In, MarshalAs(UnmanagedType.U4)] int advf,
              [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
            void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
              [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
              [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
        }
        #endregion
    }


}
