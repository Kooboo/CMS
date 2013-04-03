using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Threading;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleControllerActionInvoker : ControllerActionInvoker
    {
        public ModuleControllerActionInvoker()
        {
        }
        private static void ValidateRequest(HttpRequestBase request)
        {
            request.ValidateInput();
        }
        #region GetFilter
        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        protected FilterInfo GetFilters(ControllerContext controllerContext, string actionName)
        {
            ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(controllerContext);
            ActionDescriptor actionDescriptor = this.FindAction(controllerContext, controllerDescriptor, actionName);
            if (actionDescriptor == null)
            {
                return null;
            }
            return this.GetFilters(controllerContext, actionDescriptor);
        }

        private IEnumerable<T> GetFilter<T>(ReflectedActionDescriptor actionDescriptor)
    where T : FilterAttribute
        {
            T[] customAttributes = (T[])actionDescriptor.MethodInfo.ReflectedType.GetCustomAttributes(typeof(T), true);
            T[] second = (T[])actionDescriptor.MethodInfo.GetCustomAttributes(typeof(T), true);
            List<T> allFilters = (from attr in customAttributes.Concat<T>(second)
                                  orderby attr.Order
                                  select attr).ToList<T>();
            return allFilters;
        }
        #endregion

        #region InvokeActionWithoutExecuteResult
        public override bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Invokes the action return ActionResult
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public virtual ModuleActionInvokedContext InvokeActionWithoutExecuteResult(ControllerContext controllerContext, string actionName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(controllerContext);
            ActionDescriptor actionDescriptor = this.FindAction(controllerContext, controllerDescriptor, actionName);
            if (actionDescriptor == null)
            {
                return null;
            }
            FilterInfo filters = this.GetFilters(controllerContext, actionDescriptor);

            try
            {
                //Default AuthorizationFilter
                AuthorizationContext context = this.InvokeAuthorizationFilters(controllerContext, filters.AuthorizationFilters, actionDescriptor);
                if (context.Result != null)
                {
                    return new ModuleActionInvokedContext(controllerContext, context.Result);
                }

                ActionResult authorizationResult = null;//this.OnAuthorization(controllerContext, GetFilter<FunctionAttribute>((ReflectedActionDescriptor)actionDescriptor));
                if (authorizationResult != null)
                {
                    return new ModuleActionInvokedContext(controllerContext, authorizationResult);
                }
                else
                {
                    if (controllerContext.Controller.ValidateRequest)
                    {
                        ValidateRequest(controllerContext.HttpContext.Request);
                    }
                    IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, actionDescriptor);
                    ActionExecutedContext context2 = this.InvokeActionMethodWithFilters(controllerContext, filters.ActionFilters, actionDescriptor, parameterValues);
                    //this.InvokeActionResultWithFilters(controllerContext, filters.ResultFilters, context2.Result);
                    return new ModuleActionInvokedContext(controllerContext, context2.Result);
                }
            }
            catch (Exception exception)
            {
                ExceptionContext context3 = this.InvokeExceptionFilters(controllerContext, filters.ExceptionFilters, exception);
                if (!context3.ExceptionHandled)
                {
                    throw;
                }
                return new ModuleActionInvokedContext(controllerContext, context3.Result);
            }
        }
        //protected virtual ActionResult OnAuthorization(ControllerContext controllerContext, IEnumerable<FunctionAttribute> functions)
        //{
        //    if (functions != null && functions.Count() > 0)
        //    {
        //        var lastFunction = functions.Last();
        //        var modulePermission = ModuleInfo.Permissions.Where(mp => mp.FunctionName == lastFunction.Name).FirstOrDefault();
        //        var user = controllerContext.RequestContext.HttpContext.User;
        //        bool authorizated = ModuleInfo.IsAuthorized(lastFunction.Name, user);
        //        if (!authorizated)
        //        {
        //            ActionResult actionResult = null;

        //            if (actionResult == null && !StringExtensions.IsNullOrEmptyTrim(ModuleInfo.ModuleSettings.UnauthorizedUrl))
        //            {
        //                actionResult = ((ModuleController)controllerContext.Controller).ActionResultHelper.Redirect(controllerContext.HttpContext.Response.ApplyAppPathModifier(ModuleInfo.ModuleSettings.UnauthorizedUrl));
        //            }
        //            if (actionResult == null)
        //            {
        //                actionResult = new ContentResult() { Content = Resources.ModuleUnauthorizated };
        //            }
        //            return actionResult;
        //        }
        //    }
        //    return null;
        //} 
        #endregion

        #region ExecuteActionResult
        /// <summary>
        /// Executes the action result.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionResult">The action result.</param>
        public ModuleResultExecutedContext ExecuteActionResult(ModuleActionInvokedContext actionInvokedContext)
        {
            var actionName = actionInvokedContext.ControllerContext.RouteData.GetRequiredString("action");
            FilterInfo filters = this.GetFilters(actionInvokedContext.ControllerContext, actionName);
            return this.CustomInvokeActionResultWithFilters(actionInvokedContext.ControllerContext, filters.ResultFilters, actionInvokedContext.ActionResult);
        }
        internal static ModuleResultExecutedContext InvokeActionResultFilter(IResultFilter filter, ResultExecutingContext preContext, Func<ModuleResultExecutedContext> continuation)
        {
            filter.OnResultExecuting(preContext);
            if (preContext.Cancel)
            {
                return new ModuleResultExecutedContext(preContext, preContext.Result, true /* canceled */, null /* exception */, "");
            }

            bool wasError = false;
            ModuleResultExecutedContext postContext = null;
            try
            {
                postContext = continuation();
            }
            catch (ThreadAbortException)
            {
                // This type of exception occurs as a result of Response.Redirect(), but we special-case so that
                // the filters don't see this as an error.
                postContext = new ModuleResultExecutedContext(preContext, preContext.Result, false /* canceled */, null /* exception */, "");
                filter.OnResultExecuted(postContext);
                throw;
            }
            catch (Exception ex)
            {
                wasError = true;
                postContext = new ModuleResultExecutedContext(preContext, preContext.Result, false /* canceled */, ex, "");
                filter.OnResultExecuted(postContext);
                if (!postContext.ExceptionHandled)
                {
                    throw;
                }
            }
            if (!wasError)
            {
                filter.OnResultExecuted(postContext);
            }
            return postContext;
        }

        protected virtual ModuleResultExecutedContext CustomInvokeActionResultWithFilters(ControllerContext controllerContext, IList<IResultFilter> filters, ActionResult actionResult)
        {
            ResultExecutingContext preContext = new ResultExecutingContext(controllerContext, actionResult);
            Func<ModuleResultExecutedContext> continuation = delegate
            {
                var html = ModuleActionResultExecutor.ExecuteResult(controllerContext, actionResult);
                return new ModuleResultExecutedContext(controllerContext, actionResult, false /* canceled */, null /* exception */, html);
            };

            // need to reverse the filter list because the continuations are built up backward
            Func<ModuleResultExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
                (next, filter) => () => InvokeActionResultFilter(filter, preContext, next));
            return thunk();
        }


        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            throw new NotSupportedException();
        }
        protected override ResultExecutedContext InvokeActionResultWithFilters(ControllerContext controllerContext, IList<IResultFilter> filters, ActionResult actionResult)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
