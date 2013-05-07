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
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.EventBus
{
    public static class PublisherExtensions
    {
        public static void PreAdd<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new PreAdd<T>(entity));
        }

        public static void PreUpdate<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new PreUpdate<T>(entity));
        }

        public static void PreDelete<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new PreDelete<T>(entity));
        }

        public static void Added<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new Added<T>(entity));
        }

        public static void Updated<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new Updated<T>(entity));
        }

        public static void Deleted<T>(this IEventPublisher publisher, T entity)
        {
            publisher.Publish(new Deleted<T>(entity));
        }
    }
}
