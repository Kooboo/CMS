using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Kooboo.Drawing
{
    public class DefaultWebPageCaptureProviderFactory : IWebPageCaptureProviderFactory
    {
        public IWebPageCaptureProvider CreateProvider()
        {
            return new DefaultWebPageCaptureProvider();
        }
    }

    public class DefaultWebPageCaptureProvider : IWebPageCaptureProvider
    {
        public EventHandler<WebPageCapturedEventArgs> _captured;

        private WebBrowser _browser;

        public DefaultWebPageCaptureProvider()
        {
            _browser = new WebBrowser();
            _browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
            _browser.ScrollBarsEnabled = false;
        }

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

        public static bool SetWinINETCookieString(string sURL, string sName, string sData)
        {
            return InternetSetCookie(sURL, sName, sData);
        }

        private static object SetCookiesLocker = new object();

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

        public class DefaultImage : IWebPageCaptureImage
        {
            private Image _image;
            public DefaultImage(Image image)
            {
                _image = image;
            }

            public void Save(string path, System.Drawing.Imaging.ImageFormat format)
            {
                _image.Save(path, format);
            }
        }

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
    }


}
