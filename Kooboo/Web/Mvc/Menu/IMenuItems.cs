using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.Menu
{
    public interface IMenuItems : IEnumerable<MenuItem>
    {        
    }

    public class MenuItems : List<MenuItem>, IMenuItems
    { }

}
