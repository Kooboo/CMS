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

namespace Kooboo.CMS.Common.Runtime.Mvc
{
    public class MvcDependencyAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IEngine _engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectFilterAttributeFilterProvider"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public MvcDependencyAttributeFilterProvider(IEngine engine)
        {
            this._engine = engine;
        }

        /// <summary>
        /// Gets the controller attributes.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>The filters defined by attributes</returns>
        protected override IEnumerable<FilterAttribute> GetControllerAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this._engine.InjectProperties(attribute);
            }

            return attributes;
        }

        /// <summary>
        /// Gets the action attributes.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>The filters defined by attributes.</returns>
        protected override IEnumerable<FilterAttribute> GetActionAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this._engine.InjectProperties(attribute);
            }

            return attributes;
        }
    }
}
