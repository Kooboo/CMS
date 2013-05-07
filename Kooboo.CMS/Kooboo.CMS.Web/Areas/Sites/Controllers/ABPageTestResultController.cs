#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Web.Areas.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "A/B Test", Order = 1)]
    public class ABPageTestResultController : ManageControllerBase<ABPageTestResult, ABPageTestResultManager>
    {
        #region .ctor
        ABPageTestResultManager _abPageTestResultManager;
        public ABPageTestResultController(ABPageTestResultManager abPageTestResultManager)
            : base(abPageTestResultManager)
        {
            this._abPageTestResultManager = abPageTestResultManager;
        }
        #endregion
        protected override IEnumerable<ABPageTestResult> List(string search, string sortField, string sortDir)
        {
            var list = base.List(search, sortField, sortDir);
            return list.Select(it => it.AsActual());
        }
        #region HitReport
        public ActionResult HitReport(string uuid, string sortField, string sortDir)
        {
            var testResult = _abPageTestResultManager.Get(Site, uuid);

            var list = testResult.PageHits.AsQueryable().SortBy(sortField, sortDir);

            var report = list.Select(it => new ABPageTestHitsReport()
            {
                PageName = it.PageName,
                ShowTimes = it.ShowTimes,
                ShowRate = it.ShowTimes / (testResult.TotalShowTimes * 1.0),
                HitTimes = it.HitTimes,
                HitRate = it.HitTimes / (testResult.TotalHitTimes * 1.0)
            });
            ViewBag.TestResult = testResult;
            return View(report);
        }
        #endregion
    }
}