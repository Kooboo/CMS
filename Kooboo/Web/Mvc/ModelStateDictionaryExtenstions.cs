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
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc
{
    public static  class ModelStateDictionaryExtenstions
    {
        public static List<string> GetErrorMessages(this ModelStateDictionary modelStates)
        {
            var messages = new List<string>();

            foreach (var state in modelStates)
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (var e in state.Value.Errors)
                    {

                        messages.Add(state.Key+":"+e.ErrorMessage);
                    }
                }
            }

            return messages;
        }
    }
}
