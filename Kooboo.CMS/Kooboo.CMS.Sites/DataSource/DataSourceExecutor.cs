#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Dynamic;
using Kooboo.CMS.Sites.DataSource.Misc;
using Kooboo.CMS.Common.Formula;
using System.Collections;

namespace Kooboo.CMS.Sites.DataSource
{
    public class DataSourceExecutor
    {
        public static string ModelName = "model";
        public static void Execute(ViewDataDictionary viewData, DataSourceContext dataSourceContext, IEnumerable<DataSourceSetting> dataSourceSettings)
        {
            foreach (var item in dataSourceSettings)
            {
                var data = ExecuteDataSource(item, dataSourceContext);
                if (item.DataName.EqualsOrNullEmpty(ModelName, StringComparison.CurrentCultureIgnoreCase))
                {
                    viewData.Model = data;
                }
                viewData[item.DataName] = data;
            }
        }
        private static object ExecuteDataSource(DataSourceSetting dataSourceSetting, DataSourceContext dataSourceContext)
        {
            var data = dataSourceSetting.DataSource.Execute(dataSourceContext);

            if (dataSourceSetting.Relations != null && dataSourceSetting.Relations.Count > 0)
            {
                if (data is IEnumerable
                    && !IsDictionary(data.GetType()))
                {
                    List<object> list = new List<object>();
                    foreach (var item in (IEnumerable)data)
                    {
                        list.Add(PopulateRelations(item, dataSourceSetting, dataSourceContext));
                    }
                    data = list;
                }
                else
                {
                    data = PopulateRelations(data, dataSourceSetting, dataSourceContext);
                }
            }
            return data;
        }

        private static bool IsDictionary(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return true;
            }
            var t = type;
            while (t != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    return true;
                }
                t = t.BaseType;
            }
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (IsDictionary(interfaceType))
                {
                    return true;
                }
            }
            return false;
        }

        private static object PopulateRelations(object data, DataSourceSetting dataSourceSetting, DataSourceContext dataSourceContext)
        {
            DynamicObject dynamicObject = new Kooboo.Dynamic.ExpandoObjectWrapper(data);
            dynamicObject.TrySetMember(new SetMemberBinderWrapper("_RawObject_"), data);
            foreach (var relation in dataSourceSetting.Relations)
            {
                var relatedDataSource = new DataSourceSetting(dataSourceSetting.Site, relation.TargetDataSourceName).LastVersion().AsActual();
                if (relatedDataSource != null)
                {
                    Func<object> getRelation = () =>
                    {
                        var parameterValues = PopulateParameters(data, relation.ParameterValues);
                        var valueProvider = new ValueProviderCollection() { new DictionaryValueProvider<object>(parameterValues, System.Globalization.CultureInfo.CurrentCulture), dataSourceContext.ValueProvider };
                        var relationDataSourceContext = new DataSourceContext(dataSourceContext.Site, dataSourceContext.Page) { ValueProvider = valueProvider };
                        return ExecuteDataSource(relatedDataSource, relationDataSourceContext);
                    };

                    if (relation.LazyLoad == true)
                    {
                        dynamicObject.TrySetMember(new SetMemberBinderWrapper("get_" + relation.TargetDataSourceName), getRelation);
                    }
                    else
                    {
                        dynamicObject.TrySetMember(new SetMemberBinderWrapper(relation.TargetDataSourceName), getRelation());
                    }
                }
            }
            return dynamicObject;
        }

        private static IDictionary<string, object> PopulateParameters(object data, Dictionary<string, string> parameters)
        {
            Dictionary<string, object> parameterValues = new Dictionary<string, object>();
            if (parameters != null)
            {
                FormulaParser parser = new FormulaParser();
                var valueProvider = new MvcValueProvider(new DynamicObjectValueProvider(new Kooboo.Dynamic.ExpandoObjectWrapper(data)));
                foreach (var item in parameters)
                {
                    parameterValues[item.Key] = parser.Populate(item.Value, valueProvider);
                }
            }
            return parameterValues;
        }
    }
}
