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

using Kooboo.CMS.Content.Persistence;
using System.Web.Mvc;
using System.ComponentModel;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Misc;
namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(Repository))]
    public class Repository_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [Remote("IsNameAvailable", "Repository")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Name { get; set; }


        [DisplayName("Display name")]
        public string DisplayName { get; set; }

        [DisplayName("Enable broadcasting")]
        [Description("Enable content broadcasting and sharing between content repositories.")]
        public bool EnableBroadcasting { get; set; }

        //[Description("Customize the CMS content editing page and the default front site content display")]
        //[DisplayName("Custom template")]
        //public bool EnableCustomTemplate { get; set; }

        [Description("Disallowed userkey chars. Replace chars that match this regular expression into hyphens")]
        [DisplayName("Userkey escape chars")]
        public string UserKeyReplacePattern { get; set; }

        [DisplayName("Userkey hyphens")]
        public string UserKeyHyphens { get; set; }

        [DisplayName("Enable versioning")]
        [Required]
        [Description("Enable content versioning")]
        public bool? EnableVersioning { get; set; }

        [Required]
        [DisplayName("Enable workflow")]
        [Description("Enable workflow control on folder content adding.")]
        public bool? EnableWorkflow { get; set; }

        [DisplayName("Strict content permission")]
        [Description("User can not manage the content when the folder permission is empty.")]
        public bool StrictContentPermission { get; set; }
        [DisplayName("Show hidden folders")]
        [Description("Show the all folders including the hidden folders.")]
        public bool ShowHiddenFolders { get; set; }
    }

}