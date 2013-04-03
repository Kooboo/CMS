using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public enum AttachState
    {
        /// <summary>
        /// The entity is null, so reject to attach it.
        /// </summary>
        Rejected,
        /// <summary>
        /// The same enitykey has been attached.
        /// </summary>
        EntityKeyAttached,
        /// <summary>
        /// the entity has been attached.
        /// </summary>
        Attached,
    }
}
