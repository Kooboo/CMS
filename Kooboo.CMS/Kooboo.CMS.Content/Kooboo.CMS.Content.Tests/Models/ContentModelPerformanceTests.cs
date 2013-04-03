using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.Content.Tests.Models
{
    [TestClass]
    public class ContentModelPerformanceTests
    {
        private class ContentObject
        {
            public string UUID { get; set; }
            public string UserKey { get; set; }
        }
        [TestMethod]
        public void TestWritePerformance()
        {
            int count = 100000;
            Stopwatch classStopwatch = new Stopwatch();
            classStopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                dynamic content = new ContentObject();
                content.UUID = Guid.NewGuid().ToString();
                content.UserKey = "userkey";
            }
            classStopwatch.Stop();

            Stopwatch dynamicStopwatch = new Stopwatch();
            dynamicStopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                dynamic content = new TextContent();

                content.UUID = Guid.NewGuid().ToString();
                content.UserKey = "userkey";
            }

            dynamicStopwatch.Stop();

            Console.WriteLine("class write:{0}ms", classStopwatch.ElapsedMilliseconds);
            Console.WriteLine("dynamic object write:{0}ms", dynamicStopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TestReadPerformance()
        {
            int count = 100000;
            var contentObject = new ContentObject();
            contentObject.UUID = Guid.NewGuid().ToString();
            contentObject.UserKey = "userkey";
            Stopwatch classStopwatch = new Stopwatch();
            classStopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                var uuid = contentObject.UUID;
                var userKey = contentObject.UserKey;
            }
            classStopwatch.Stop();


            var dynamicContent = new TextContent();
            dynamicContent.UUID = Guid.NewGuid().ToString();
            dynamicContent.UserKey = "userkey";
            Stopwatch dynamicStopwatch = new Stopwatch();
            dynamicStopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                var uuid = dynamicContent.UUID;
                var userKey = dynamicContent.UserKey;
            }
            dynamicStopwatch.Stop();

            Console.WriteLine("class read:{0}ms", classStopwatch.ElapsedMilliseconds);
            Console.WriteLine("dynamic object read:{0}ms", dynamicStopwatch.ElapsedMilliseconds);

        }
    }
}
