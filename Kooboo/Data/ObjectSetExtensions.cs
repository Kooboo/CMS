using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq.Expressions;
using Kooboo.Reflection;

namespace Kooboo.Data
{
    public static class ObjectSetExtensions
    {

        public static EntityKey CreateEntityKey<TEntity>(this ObjectSet<TEntity> objectSet, TEntity entity) where TEntity : class
        {
            return objectSet.Context.CreateEntityKey(objectSet.EntitySet.Name, entity);
        }

        public static EntityKey CreateEntityKey<TEntity>(this ObjectSet<TEntity> objectSet, params Expression<Func<object>>[] entityKeyExpressions) where TEntity : class
        {

            var members = new List<EntityKeyMember>();

            foreach (var expression in entityKeyExpressions)
            {
                var keyName = string.Empty;

                var invokeExpression = expression.Body;

                if (expression.Body.NodeType == ExpressionType.Convert)
                {
                    invokeExpression = ((UnaryExpression)expression.Body).Operand;

                }
                switch (invokeExpression.NodeType)
                {

                    case ExpressionType.MemberAccess:
                        keyName = ((MemberExpression)invokeExpression).Member.Name;
                        break;
                    case ExpressionType.Parameter:
                        keyName = ((ParameterExpression)invokeExpression).Name;
                        break;
                    case ExpressionType.Call:
                        keyName = ((MethodCallExpression)invokeExpression).Method.Name;
                        break;
                    default:
                        throw new NodeTypeNotSupportException(expression.Body.NodeType);
                }



                members.Add(new EntityKeyMember() { Key = keyName, Value = expression.Compile()() });
            }

            EntityKey key = new EntityKey()
            {
                EntityContainerName = objectSet.EntitySet.EntityContainer.Name,
                EntitySetName = objectSet.EntitySet.Name,
                EntityKeyValues = members.ToArray()
            };

            return key;
        }

        public static TEntity GetObjectByKey<TEntity>(this ObjectSet<TEntity> objectSet, EntityKey entityKey) where TEntity : class
        {
            var obj = default(object);

            if (objectSet.Context.TryGetObjectByKey(entityKey, out obj))
            {
                return obj as TEntity;
            }
            else
            {
                return null;
            }

        }

        public static TEntity GetObjectByKey<TEntity>(this ObjectSet<TEntity> objectSet, params Expression<Func<object>>[] entityKeyExpressions) where TEntity : class
        {
            return objectSet.GetObjectByKey(objectSet.CreateEntityKey(entityKeyExpressions));
        }

        public static void Attach<TEntity>(this ObjectSet<TEntity> objectSet, TEntity entity, Action<TEntity> callback) where TEntity : class
        {
            if (entity != null)
            {
                objectSet.Attach(entity, callback, objectSet.CreateEntityKey(entity));
            }

        }

        public static void Attach<TEntity>(this ObjectSet<TEntity> objectSet, TEntity entity, Action<TEntity> callback, params Expression<Func<object>>[] entityKeyExpressions) where TEntity : class
        {
            objectSet.Attach(entity, callback, objectSet.CreateEntityKey(entityKeyExpressions));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="objectSet"></param>
        /// <param name="entity"></param>
        /// <param name="callback"></param>
        /// <param name="entityKey"></param>
        public static void Attach<TEntity>(this ObjectSet<TEntity> objectSet, TEntity entity, Action<TEntity> callback, EntityKey entityKey) where TEntity : class
        {
            if (entity != null)
            {
                var obj = entity;
                if (!objectSet.HasAttached(obj))
                {

                    if (objectSet.HasAttached(entityKey))
                    {
                        obj = objectSet.GetObjectByKey(entityKey);

                    }
                    else
                    {
                        objectSet.Attach(obj);
                    }
                }

                if (callback != null)
                {
                    callback(obj);
                }
            }



        }

        //public static void Attach<TEntity>(this ObjectSet<TEntity> objectSet, TEntity entity, Action<TEntity> callback, params Expression<Func<object>>[] entityKeyExpressions) where TEntity : class
        //{
        //    objectSet.Attach(entity, callback, objectSet.EntityKey(entityKeyExpressions));
        //}


        public static void AttachAll<TEntity>(this ObjectSet<TEntity> objectSet, IAssociatableCollection<TEntity> entities, Func<ObjectSet<TEntity>, TEntity, bool> predicate) where TEntity : class
        {
            var cascadableCollection = entities as ICascadableCollection<TEntity>;

            foreach (var i in entities.ToArray())
            {
                if (!objectSet.HasAttached(i))
                {
                    var entityKey = objectSet.CreateEntityKey(i);

                    if (objectSet.HasAttached(entityKey))
                    {
                        if (cascadableCollection != null)
                        {
                            cascadableCollection.Remove(i, false);
                        }
                        else
                        {
                            entities.Remove(i);
                        }
                        entities.Add(objectSet.GetObjectByKey(entityKey));
                    }
                    else
                    {
                        if (predicate(objectSet, i))
                        {
                            objectSet.Attach(i);
                        }
                    }
                }
            }

            if (cascadableCollection != null)
            {
                foreach (var i in cascadableCollection.Added)
                {
                    if (objectSet.HasAttached(i))
                    {
                        objectSet.DeleteObject(i);
                    }
                    else
                    {
                        var entityKey = objectSet.CreateEntityKey(i);

                        if (objectSet.HasAttached(entityKey))
                        {
                            objectSet.DeleteObject(objectSet.GetObjectByKey(entityKey));
                        }
                        else
                        {
                            if (predicate(objectSet, i))
                            {
                                objectSet.Attach(i);
                                objectSet.DeleteObject(i);
                            }
                        }
                    }



                }
            }
        }



        public static void CasadeDelete<TEntity>(this ObjectSet<TEntity> objectSet, ICollection<TEntity> entities) where TEntity : class
        {
            if (entities.Count == 0)
            {
                foreach (var entry in objectSet.Context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(e => e.IsRelationship))// && e.EntitySet.Name == objectSet.EntitySet.Name))
                {
                    entry.AcceptChanges();
                }
            }
            else
            {
                foreach (var i in entities.ToArray())
                {
                    objectSet.DeleteObject(i);
                }
            }
        }





        public static bool HasAttached<TEntity>(this ObjectSet<TEntity> objectSet, EntityKey entityKey) where TEntity : class
        {
            var state = default(ObjectStateEntry);

            return objectSet.Context.ObjectStateManager.TryGetObjectStateEntry(entityKey, out state);
        }

        //public static bool HasAttached<TEntity>(this ObjectSet<TEntity> objectSet,  params Expression<Func<object>>[] entityKeyExpressions) where TEntity : class
        //{
        //    return objectSet.HasAttached(objectSet.EntityKey(entityKeyExpressions));
        //}

        public static bool HasAttached<TEntity>(this ObjectSet<TEntity> list, TEntity entity) where TEntity : class
        {
            var state = default(ObjectStateEntry);
            return list.Context.ObjectStateManager.TryGetObjectStateEntry(entity, out state);
        }

        public static bool TryAddObject<TEntity>(this ObjectSet<TEntity> list, TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                return false;
            }
            else
            {
                list.AddObject(entity);
                return true;
            }
        }

        public static bool TryDeleteObject<TEntity>(this ObjectSet<TEntity> list, TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                return false;
            }
            else
            {
                list.DeleteObject(entity);
                return true;
            }
        }


        //public static bool TryAttach<TEntity>(this ObjectSet<TEntity> list, TEntity entity) where TEntity : class
        //{
        //    return list.TryAttach(entity, false);
        //}
        public static bool TryDetach<TEntity>(this ObjectSet<TEntity> list, TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                return true;
            }
            else
            {
                list.Detach(entity);
                return true;
            }
        }

        public static bool TryDetachAll<TEntity>(this ObjectSet<TEntity> list, ICollection<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                list.TryDetach(entity);
            }

            return true;
        }

        public static AttachState TryAttach<TEntity>(this ObjectSet<TEntity> list, TEntity entity, bool isSetEntryModified = false) where TEntity : class
        {
            if (entity == null)
            {
                return AttachState.Rejected;
            }

            if (list.HasAttached(entity))
            {
                if (isSetEntryModified)
                {
                    list.Context.TryAttachChanges(entity);
                }
                return AttachState.Attached;
            }
            else
            {
                var entityKey = list.CreateEntityKey(entity);
                if (list.HasAttached(entityKey))
                {
                    return AttachState.EntityKeyAttached;
                }

                list.Attach(entity);
                if (isSetEntryModified)
                {
                    list.Context.TryAttachChanges(entity);
                }

                return AttachState.Attached;
            }
        }

        public static bool TryAttachChanges(this ObjectContext context, object item)
        {
            ObjectStateEntry entry;

            if (context.ObjectStateManager.TryGetObjectStateEntry(item, out entry))
            {
                var related = entry.RelationshipManager.GetAllRelatedEnds().ToList();

                for (int i = 0; i < entry.CurrentValues.FieldCount; i++)
                {
                    bool isKey = false;

                    string name = entry.CurrentValues.GetName(i);


                    foreach (var keyPair in entry.EntityKey.EntityKeyValues)
                    {
                        if (string.Compare(name, keyPair.Key, true) == 0)
                        {
                            isKey = true;
                            break;
                        }
                    }
                    if (!isKey)
                    {
                        entry.SetModifiedProperty(name);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool TryAttachAll<TParent, TChild>(this ObjectSet<TChild> list, TParent parent, Func<ICascadableCollection<TChild>> selector, Func<ObjectSet<TChild>, TChild, bool> predicate)
            where TParent : class
            where TChild : class
        {

            var relations = selector();

            var updated = relations.Updated.ToArray();
            var added = relations.Added.ToArray();
            var removed = relations.Removed.ToArray();

            relations.Clear();


            foreach (var i in added)
            {
                if (predicate(list, i))
                {
                    list.TryAttach(i);
                }
                relations.Add(i);
            }

            foreach (var i in removed)
            {
                if (predicate(list, i))
                {
                    list.TryAttach(i);
                    relations.Remove(i);
                    list.TryDeleteObject(i);
                }
            }

            foreach (var i in updated)
            {
                list.TryAttach(i, true);
            }

            return true;

        }


        public static bool TryJoinAll<TParent, TChild>(this ObjectSet<TChild> list, TParent parent, Expression<Func<TParent, object>> childrenSelector)
            where TParent : class
            where TChild : class
        {


            var children = childrenSelector.Compile()(parent);
           

            //todo: add cascadable implement
            if (children is ICascadableCollection<TChild>)
            {
                var cascadeCollection = children as ICascadableCollection<TChild>;
                foreach (var i in cascadeCollection.Added)
                {
                    var state = list.TryAttach(i);
                    if (state == AttachState.Attached)
                    {
                        cascadeCollection.Add(i);
                    }
                    else if( state == AttachState.EntityKeyAttached)
                    {
                        var entityKey = list.CreateEntityKey(i);
                        var attachedEntity = list.GetObjectByKey(entityKey);
                        cascadeCollection.Add(attachedEntity);
                    }
                }

                foreach (var i in cascadeCollection.Removed)
                {
                    var state = list.TryAttach(i);
                    if (state == AttachState.Attached)
                    {
                        cascadeCollection.Remove(i);
                    }
                    else if( state == AttachState.EntityKeyAttached)
                    {
                        var entityKey = list.CreateEntityKey(i);
                        var attachedEntity = list.GetObjectByKey(entityKey);
                        cascadeCollection.Remove(attachedEntity);
                    }
                }

                return true;

            }
            else if (children is IAssociatableCollection<TChild>)
            {
                var associationCollection = children as IAssociatableCollection<TChild>;

                foreach (var i in associationCollection.Added)
                {
                    var state = list.TryAttach(i);
                    if (state == AttachState.Attached)
                    {
                        associationCollection.Add(i);
                    }
                    else if (state == AttachState.EntityKeyAttached)
                    {
                        var entityKey = list.CreateEntityKey(i);
                        var attachedEntity = list.GetObjectByKey(entityKey);
                        associationCollection.Add(attachedEntity);
                    }
                }

                foreach (var i in associationCollection.Removed)
                {
                    var state = list.TryAttach(i);
                    if (state == AttachState.Attached)
                    {
                        
                        associationCollection.Remove(i);
                        list.Context.ObjectStateManager.ChangeRelationshipState(parent, i, childrenSelector, EntityState.Deleted);
                    }
                    else if (state == AttachState.EntityKeyAttached)
                    {
                        var entityKey = list.CreateEntityKey(i);
                        var attachedEntity = list.GetObjectByKey(entityKey) as TChild;

                        associationCollection.Remove(attachedEntity);
                        list.Context.ObjectStateManager.ChangeRelationshipState(parent, attachedEntity, childrenSelector, EntityState.Deleted);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryFixAll<TEntity, TForeginKey>(this ObjectSet<TEntity> list, ICascadableCollection<TEntity> collection, TForeginKey value, Expression<Func<TEntity, TForeginKey>> field)
            where TEntity : class
        {


            var fieldName = ((MemberExpression)field.Body).Member.Name;

            foreach (var i in collection.Added)
            {
                list.TryAddObject(i);
                i.Members().Properties[fieldName] = value;
            }

            foreach (var i in collection.Removed)
            {
                list.TryAttach(i);
                collection.Remove(i);
                list.TryDeleteObject(i);
            }

            foreach (var i in collection.Updated)
            {
                list.TryAttach(i,true);
                
            }

            return true;
        }

    

        public static bool TryFixAll<TEntity, TForeginKey>(this ObjectSet<TEntity> list, IAssociatableCollection<TEntity> collection, TForeginKey value, Expression<Func<TEntity, TForeginKey>> field)
      where TEntity : class
        {

            var fieldName = ((MemberExpression)field.Body).Member.Name;

            foreach (var i in collection.Added)
            {
                list.TryAttach(i, true);
                
                i.Members().Properties[fieldName] = value;
            }

            foreach (var i in collection.Removed)
            {
                list.TryAttach(i);
                i.Members().Properties[fieldName] = null;
            }

            return true;
        }


        public static bool TryRemoveAll<TParent, TChild>(this ObjectSet<TChild> list, TParent parent, Expression<Func<TParent, object>> selector)
            where TParent : class
            where TChild : class
        {
            list.Context.LoadProperty<TParent>(parent, selector);

            var collection = selector.Compile()(parent) as ICollection<TChild>;

            var isCascadeDelete = collection is ICascadableCollection<TChild>;


            foreach (var i in collection.ToArray())
            {
                list.TryAttach(i);

                if (isCascadeDelete)
                {
                    list.TryDeleteObject(i);
                }
            }

            return true;
        }
    }


}
