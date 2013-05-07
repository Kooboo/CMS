#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class PagePublishViewModel
    {
        public string UUID { get; set; }
        
        [Display(Name = "Use draft version")]
        public bool PublishDraft { get; set; }

        public bool PublishSchedule { get; set; }

        [UIHint("Date")]
        [Display(Name="Publish date")]
        
        public DateTime PublishDate { get; set; }

        [UIHint("TimeSpan")]
        [Display(Name="Publish time")]
        public string PublishTime { get; set; }

        public bool Period { get; set; }
        
        [Display(Name="Offline date")]
        [UIHint("Date")]
        public DateTime OfflineDate { get; set; }
        
        [UIHint("TimeSpan")]
        [Display(Name="Offline time")]
        public string OfflineTime { get; set; }
    }
}