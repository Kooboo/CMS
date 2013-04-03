using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Web.Mvc
{
    public static class ControllerExtensions
    {
      

        //public static string GetMethodName<TController>(this Expression<Action<TController>> action)
        //{

        //    var actionExpression = action.Body;

        //    if (action.Body.NodeType == ExpressionType.Lambda)
        //    {
        //        actionExpression = ((LambdaExpression)action.Body).Body;
        //    }

        //    if (actionExpression.NodeType == ExpressionType.Call)
        //    {
        //        return ((MethodCallExpression)actionExpression).Method.Name;
        //    }
        //    else
        //    {
        //        throw new NodeTypeNotSupportException(actionExpression.NodeType);
        //    }
        //}

        //public static string GetControllerName(this Type controller)
        //{
        //    if (controller.Name.EndsWith("Controller"))
        //    {
        //        return controller.Name.Substring(0, controller.Name.Length - 10);
        //    }
        //    else
        //    {
        //        throw new InvaildControllerNameException();
        //    }
        //}

        //public static string GetRequestValue(this ControllerBase controller, string name)
        //{
        //    var value = controller.ValueProvider.GetValue(name);
        //    return value == null ? string.Empty : value.AttemptedValue;
        //}
        
    }
}
