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
using NH=NHibernate;

namespace Kooboo.eCommerce.NHibernate
{
    public class InitializeDbTask : Kooboo.CMS.Common.Runtime.IStartupTask
    {
        public void Execute()
        {
            NH.Cfg.Configuration _configuration = SessionFactory.GetConfiguration();
            //NH.Tool.hbm2ddl.SchemaExport se = new NH.Tool.hbm2ddl.SchemaExport(_configuration);
            //se.Execute(false, false, true);
            NH.Tool.hbm2ddl.SchemaUpdate su = new NH.Tool.hbm2ddl.SchemaUpdate(_configuration);
            su.Execute(false, true);
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
