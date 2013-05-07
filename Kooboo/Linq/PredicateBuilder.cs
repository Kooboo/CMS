#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

//http://www.albahari.com/nutshell/predicatebuilder.aspx
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Linq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public static class PredicateBuilder
    {
        #region Methods
        /// <summary>
        /// Trues this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        /// <summary>
        /// Falses this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Ors the specified expr1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1">The expr1.</param>
        /// <param name="expr2">The expr2.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Ands the specified expr1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1">The expr1.</param>
        /// <param name="expr2">The expr2.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        } 
        #endregion
    }
}
