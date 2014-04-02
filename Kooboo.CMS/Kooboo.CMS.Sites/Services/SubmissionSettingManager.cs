#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Services
{
    public class SubmissionSettingManager : ManagerBase<SubmissionSetting, ISubmissionSettingProvider>
    {
        #region .ctor
        ISubmissionSettingProvider _provider;
        public SubmissionSettingManager(ISubmissionSettingProvider provider)
            : base(provider)
        {
            _provider = provider;
        }
        #endregion

        #region All
        public override IEnumerable<SubmissionSetting> All(Models.Site site, string filterName)
        {
            var list = _provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.Name.Contains(filterName));
            }
            return list;
        }

        #endregion

        #region Get
        public override SubmissionSetting Get(Models.Site site, string name)
        {
            SubmissionSetting submission = null;
            while (submission == null && site != null)
            {
                submission = _provider.Get(new SubmissionSetting() { Site = site, Name = name });
                if (submission == null)
                {
                    site = site.Parent;
                }
            }
            return submission;
        }
        #endregion

        #region Update
        public override void Update(Models.Site site, SubmissionSetting @new, SubmissionSetting old)
        {
            @new.Site = site;
            @old.Site = site;
            _provider.Update(@new, old);
        }
        #endregion

        #region Import/export
        public virtual void Import(Site site, Stream zipStream, bool @override)
        {
            Provider.Import(site, zipStream, @override);
        }
        public virtual void Export(Site site, IEnumerable<SubmissionSetting> submissionSettings, System.IO.Stream outputStream)
        {
            Provider.Export(site, submissionSettings, outputStream);
        }
        #endregion
    }
}
