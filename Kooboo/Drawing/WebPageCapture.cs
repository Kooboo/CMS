using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Drawing;

namespace Kooboo.Drawing
{
    public class WebPageCapture
    {
        private static object _timerLock = new object();
        private static Timer _timer;
        private static object _runningLock = new object();
        private static DateTime _lastRunningTime;
        private static bool _isRunning = false;
        private static Thread _runningThread;

        static WebPageCapture()
        {
            ProviderFactory = new DefaultWebPageCaptureProviderFactory();
        }

        internal static readonly ConcurrentQueue<WebPageCaptureTask> Tasks = new System.Collections.Concurrent.ConcurrentQueue<WebPageCaptureTask>();

        public static IWebPageCaptureProviderFactory ProviderFactory { get; set; }

        public static Action<Exception> OnError { get; set; }

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
    }

    internal class WebPageCaptureTask
    {
        public string Url { get; set; }

        public HttpCookieCollection Cookies { get; set; }

        public EventHandler<WebPageCapturedEventArgs> Captured { get; set; }

        public Action<Exception> OnError { get; set; }
    }

    public interface IWebPageCaptureProviderFactory
    {
        IWebPageCaptureProvider CreateProvider();
    }

    public interface IWebPageCaptureProvider : IDisposable
    {
        void Navigate(string url, HttpCookieCollection cookie, EventHandler<WebPageCapturedEventArgs> captured);
    }

    public class WebPageCapturedEventArgs : EventArgs
    {
        public IWebPageCaptureImage Image { get; set; }
    }

    public interface IWebPageCaptureImage
    {
        void Save(string path, System.Drawing.Imaging.ImageFormat format);
    }
}
