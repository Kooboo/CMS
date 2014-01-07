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
    public static class ModelMetadataExtensions
    {
        public static ISelectListDataSource GetDataSource(this ModelMetadata modelMetadata)
        {
            ISelectListDataSource dataSource = null;
            if (modelMetadata is KoobooModelMetadata)
            {
                var dataSourceAttribute = ((KoobooModelMetadata)modelMetadata).DataSourceAttribute;
                if (dataSourceAttribute != null)
                {
                    dataSource = (ISelectListDataSource)TypeActivator.CreateInstance(dataSourceAttribute.DataSourceType);
                }
                if (dataSource == null)
                {
                    dataSource = ((KoobooModelMetadata)modelMetadata).DataSource;
                }
            }
            else
            {
            }
            if (dataSource == null)
            {
                return new EmptySelectListDataSource();
            }
            else
            {
                return dataSource;
            }
        }
        public static Type GetDataSourceType(this ModelMetadata modelMetadata)
        {
            Type dataSourceType = null;
            if (modelMetadata is KoobooModelMetadata)
            {
                var dataSourceAttribute = ((KoobooModelMetadata)modelMetadata).DataSourceAttribute;
                dataSourceType = dataSourceAttribute.DataSourceType;
            }
            else
            {
            }
            if (dataSourceType == null)
            {
                dataSourceType = typeof(EmptySelectListDataSource);
            }

            return dataSourceType;
        }
    }
}
