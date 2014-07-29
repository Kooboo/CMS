#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Web2.Models
{
    public class AllPropertiesResolver : DefaultContractResolver
    {
        protected override IList<Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            foreach (var p in properties)
                p.Ignored = false;

            return properties;
        }

        protected override Newtonsoft.Json.Serialization.JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            if (contract is Newtonsoft.Json.Serialization.JsonStringContract)
                return CreateObjectContract(objectType);
            return contract;
        }
    }
}
