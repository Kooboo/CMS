#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.ServiceModel.Configuration;


namespace System.ServiceModel.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonpBindingExtension : BindingElementExtensionElement
    {
        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Type" /> object that represents the custom binding element.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" /> object that represents the custom binding type.</returns>
        public override Type BindingElementType
        {
            get { return typeof(JSONPBindingElement); }

        }

        /// <summary>
        /// When overridden in a derived class, returns a custom binding element object.
        /// </summary>
        /// <returns>
        /// A custom <see cref="T:System.ServiceModel.Channels.BindingElement" /> object.
        /// </returns>
        protected override System.ServiceModel.Channels.BindingElement CreateBindingElement()
        {
            return new JSONPBindingElement();
        }
    }
}
