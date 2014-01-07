#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Web;

namespace Kooboo.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public class WebPageCapture
    {
        #region Fields
        private static object _timerLock = new object();
        private static Timer _timer;
        private static object _runningLock = new object();
        private static DateTime _lastRunningTime;        
        private static Thread _runningThread;

        internal static readonly ConcurrentQueue<WebPageCaptureTask> Tasks = new System.Collections.Concurrent.ConcurrentQueue<WebPageCaptureTask>();
        #endregion

        #region .ctor
        static WebPageCapture()
        {
            ProviderFactory = new DefaultWebPageCaptureProviderFactory();
        }
        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the provider factory.
        /// </summary>
        /// <value>
        /// The provider factory.
        /// </value>
        public static IWebPageCaptureProviderFactory ProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets the on error.
        /// </summary>
        /// <value>
        /// The on error.
        /// </value>
        public static Action<Exception> OnError { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Captures the in thread.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="captured">The captured.</param>
        /// <param name="cookies">The cookies.</param>
        /// <param name="onError">The on error.</param>
        public static void CaptureInThread(string url, EventHandler<WebPageCapturedEventArgs> captured,
            HttpCookieCollection cookies = null, Action<Exception> onError = null)
        {
            Tasks.Enqueue(new WebPageCaptureTask
            {
                Url = url,
                Cookies = cookies,
                Captured = captured,
                OnError = onError
            });

            if (_timer == null)
            {
                lock (_timerLock)
                {
                    if (_timer == null)
                    {
                        StartTimer();
                    }
                }
            }
        }

        private static void StartTimer()
        {
            _timer = new Timer(new TimerCallback(o =>
            {
                // Capture thread not running or timeout
                if ((_runningThread == null || IsTimeout()) && Tasks.Count > 0)
                {
                    lock (_runningLock)
                    {
                        var isTimeout = IsTimeout();
                        if ((_runningThread == null || isTimeout) && Tasks.Count > 0)
                        {
                            if (_runningThread != null && isTimeout)
                            {
                                _runningThread.Abort();
                            }

                            // We do not capture in timer directly for brower component need to run in STA thread.
                            _lastRunningTime = DateTime.UtcNow;
                            _runningThread = new Thread(CaptureThread);
                            _runningThread.SetApartmentState(ApartmentState.STA);
                            _runningThread.Start();
                        }
                    }
                }
            }), null, 0, 1000);
        }

        private static bool IsTimeout()
        {
            return DateTime.UtcNow.Subtract(_lastRunningTime).TotalSeconds > 20;
        }

        private static void CaptureThread()
        {
            IWebPageCaptureProvider provider = null;
            try
            {
                provider = WebPageCapture.ProviderFactory.CreateProvider();
                WebPageCaptureTask task;
                while (Tasks.TryDequeue(out task))
                {
                    _lastRunningTime = DateTime.UtcNow;

                    try
                    {
                        provider.Navigate(task.Url, task.Cookies, task.Captured);
                    }
                    catch (Exception ex)
                    {
                        if (task.OnError != null)
                        {
                            task.OnError(ex);
                        }
                        else if (OnError != null)
                        {
                            OnError(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(ex);
                }
            }
            finally
            {
                if (provider != null)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                    }
                }

                _runningThread = null;
            }
        } 
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class WebPageCaptureTask
    {
        public string Url { get; set; }

        public HttpCookieCollection Cookies { get; set; }

        public EventHandler<WebPageCapturedEventArgs> Captured { get; set; }

        public Action<Exception> OnError { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWebPageCaptureProviderFactory
    {
        /// <summary>
        /// Creates the provider.
        /// </summary>
        /// <returns></returns>
        IWebPageCaptureProvider CreateProvider();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWebPageCaptureProvider : IDisposable
    {
        /// <summary>
        /// Navigates the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="captured">The captured.</param>
        void Navigate(string url, HttpCookieCollection cookie, EventHandler<WebPageCapturedEventArgs> captured);
    }

    /// <summary>
    /// 
    /// </summary>
    public class WebPageCapturedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public IWebPageCaptureImage Image { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWebPageCaptureImage
    {
        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="format">The format.</param>
        void Save(string path, System.Drawing.Imaging.ImageFormat format);
    }
}
