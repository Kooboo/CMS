using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace Kooboo.CMS.Sites.Tests.Persistence
{
    [DataContract]
    public class PersistenceObject : ISiteObject, IPersistable
    {
        public PersistenceObject()
        {

        }
        public PersistenceObject(Site site, string name)
        {

        }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Body { get; set; }

        #region Comparable override
        public static bool operator ==(PersistenceObject obj1, PersistenceObject obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(PersistenceObject obj1, PersistenceObject obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PersistenceObject))
            {
                return false;
            }
            if (obj != null)
            {
                var o = (PersistenceObject)obj;
                if (this.Name.EqualsOrNullEmpty(o.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region ISiteObject
        public Site Site
        {
            get;
            set;
        }
        #endregion

        #region IPersistable

        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            isDummy = false;
            this.Site = ((PersistenceObject)source).Site;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }
        #endregion
    }
    [TestClass]
    public class JsonListFileStorageTests
    {
        [TestMethod]
        public void Test_Add_Update_Delete()
        {
            ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test_Add_Update_Delete." + Guid.NewGuid().ToString() + ".json");
            JsonListFileStorage<PersistenceObject> storage = new JsonListFileStorage<PersistenceObject>(filePath, locker);

            var site = new Site("Site1");

            var obj = new PersistenceObject(site, "persistence1") { Body = Guid.NewGuid().ToString() };
            //test add
            storage.Add(obj);

            //test GetList
            Assert.AreEqual(1, storage.GetList(site).Count());

            //test Get
            var gotObj1 = storage.Get(new PersistenceObject(site, "persistence1"));
            Assert.AreEqual(obj, gotObj1);
            Assert.AreEqual(obj.Body, gotObj1.Body);

            //test Update
            gotObj1.Body = gotObj1.Body + DateTime.Now.ToString();
            storage.Update(gotObj1, obj);
            var gotObj2 = storage.Get(new PersistenceObject(site, "persistence1"));
            Assert.AreNotEqual(obj.Body, gotObj2.Body);
            Assert.AreEqual(gotObj1.Body, gotObj2.Body);

            storage.Remove(new PersistenceObject(site, "persistence1"));
            var gotObj3 = storage.Get(new PersistenceObject(site, "persistence1"));
            Assert.IsNull(gotObj3);
        }
    }
}
