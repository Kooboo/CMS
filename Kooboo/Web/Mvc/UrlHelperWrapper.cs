using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Reflection;

namespace Kooboo.Web.Mvc
{
    class UrlHelperWrapper
    {
        UrlHelper UrlHelper
        {
            get;
            set;
        }

        internal UrlHelperWrapper(RequestContext requestContext)           
        {
            this.UrlHelper = new UrlHelper(requestContext);
        }

        public UrlHelperWrapper(UrlHelper helper)
        {
            this.UrlHelper = helper;
        }

        public string Action<TController>(Expression<Func<TController, ActionResult>> action) where TController : IController
        {
            var actionName = string.Empty;
            var controllerName = string.Empty;
            MethodCallExpression call;

            var body = action.Body;

            if (body.NodeType == ExpressionType.Lambda)
            {
                body = ((LambdaExpression)action.Body).Body;
            }

            if (body.NodeType == ExpressionType.Call)
            {
                call = (MethodCallExpression)body;
                actionName = call.Method.Name;
            }
            else
            {
                throw new NodeTypeNotSupportException(body.NodeType);
            }

            RouteValueDictionary routeValues = new RouteValueDictionary();



            ParameterInfo[] parameters = call.Method.GetParameters();

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    Expression arg = call.Arguments[i];
                    object value = null;
                    ConstantExpression ce = arg as ConstantExpression;
                    if (ce != null)
                    {
                        // If argument is a constant expression, just get the value
                        value = ce.Value;
                    }
                    else
                    {
                        if (string.Equals(arg.ToString(), "UrlParameter`1.Empty", StringComparison.InvariantCulture))
                        {
                            value = null;
                        }
                        else
                        {
                            Expression<Func<object, object>> lambdaExpr = Expression.Lambda<Func<object, object>>(Expression.Convert(arg, typeof(object)), _unusedParameterExpr);
                            value = lambdaExpr.Compile()(null);
                        }
                    }

                    if (value != null)
                    {
                        routeValues.Add(parameters[i].Name, value);
                    }
                }
            }
            

            var controller = typeof(TController);
            if (controller.Name.EndsWith("Controller"))
            {
                controllerName = controller.Name.Substring(0, controller.Name.Length - 10);
            }
            else
            {
                controllerName = controller.Name;
            }

            return this.UrlHelper.Action(actionName, controllerName, routeValues);
        }

        private static readonly ParameterExpression _unusedParameterExpr = Expression.Parameter(typeof(object), "_unused");
 
    }

    public class UrlParameter<T> where T:struct
    {
        public static T Empty
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
