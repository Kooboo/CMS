using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Models
{
    public interface IPersistable
    {
        bool IsDummy { get; }
        void Init(IPersistable source);
        void OnSaved();
        void OnSaving();
    }
}
