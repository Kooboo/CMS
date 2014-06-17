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
        public static void Execute(ViewDataDictionary viewData, DataSourceContext dataSourceContext, IEnumerable<ExecutingDataSource> dataSources)
        {
            foreach (var item in dataSources)
            {
                var data = ExecuteDataSource(item, dataSourceContext);
                if (item.DataSourceSetting.DataName.EqualsOrNullEmpty(ModelName, StringComparison.CurrentCultureIgnoreCase))
                {
                    viewData.Model = data;
                }
                viewData[item.DataSourceSetting.DataName] = data;
            }
        }
        private static object ExecuteDataSource(ExecutingDataSource dataSource, DataSourceContext dataSourceContext)
        {
            var data = dataSource.DataSourceSetting.DataSource.Execute(dataSourceContext);
            if (data != null && dataSource.IncludedRelations != null && dataSource.IncludedRelations.Count() > 0)
            {
                if (data is IEnumerable
                    && !IsDictionary(data.GetType()))
                {
                    List<object> list = new List<object>();
                    foreach (var item in (IEnumerable)data)
                    {
                        list.Add(PopulateRelations(item, dataSource, dataSourceContext));
                    }
                    data = list;
                }
                else
                {
                    data = PopulateRelations(data, dataSource, dataSourceContext);
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

        private static object PopulateRelations(object data, ExecutingDataSource dataSource, DataSourceContext dataSourceContext)
        {
            DynamicObject dynamicObject = new Kooboo.Dynamic.ExpandoObjectWrapper(data);
            dynamicObject.TrySetMember(new SetMemberBinderWrapper("_RawObject_"), data);
            foreach (var relation in dataSource.IncludedRelations)
            {
                var relatedDataSource = new DataSourceSetting(dataSource.DataSourceSetting.Site, relation.TargetDataSourceName).LastVersion().AsActual();
                if (relatedDataSource != null)
                {
                    Func<object> getRelation = () =>
                    {
                        var parameterValues = PopulateParameters(data, relation.ParameterValues);
                        var valueProvider = new ValueProviderCollection() { new DictionaryValueProvider<object>(parameterValues, System.Globalization.CultureInfo.CurrentCulture), dataSourceContext.ValueProvider };
                        var relationDataSourceContext = new DataSourceContext(dataSourceContext.Site, dataSourceContext.Page) { ValueProvider = valueProvider };
                        return ExecuteDataSource(new ExecutingDataSource(relatedDataSource, relatedDataSource.Relations), relationDataSourceContext);
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
