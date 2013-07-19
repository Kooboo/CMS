#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ImportRepositoryModel
    {
        [Display(Name = "Delete the old content database")]
        [Description("The old content database which is not used by other sites will be deleted.")]
        public bool DeleteTheOldRepository
        {
            get;
            set;
        }

        [Remote("IsRepositoryAvaliable", "Site")]
        [Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("UploadOrSelect")]
        [DataSource(typeof(RepositoryPackagesDatasource))]
        public string File { get; set; }


    }
}
