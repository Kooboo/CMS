using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.SiteKernel.SiteFlow;
using Moq;
using Kooboo.CMS.SiteKernel.SiteFlow.Args;

namespace Kooboo.CMS.Site.Tests.SiteFlow
{
    /// <summary>
    /// Summary description for SiteRequestAdapterTests
    /// </summary>
    [TestClass]
    public class SiteRequestAdapterTests
    {
        [TestMethod]
        public void Test_Trigger_BeginSiteRequest_Event_Before_RenewHttpContext()
        {
            ISiteRequestFlow siteRequestFlow = new Mock<ISiteRequestFlow>().Object;
            var eventTriggered = false;
            var mockEvent = new Mock<ISiteRequestFlowEvents>();
            mockEvent.Setup(it => it.BeginSiteRequest(It.IsAny<object>(), It.IsAny<BeginSiteRequestEventArgs>()))
                .Callback(() => { eventTriggered = true; });
            ISiteRequestFlowEvents[] events = new ISiteRequestFlowEvents[] { mockEvent.Object };

            var siteRequestFlowAdapter = new SiteRequestFlowAdapter(siteRequestFlow, events);

            var newHttpContext = siteRequestFlowAdapter.RenewHttpContext(null);

            Assert.IsTrue(eventTriggered);
        }
    }
}
