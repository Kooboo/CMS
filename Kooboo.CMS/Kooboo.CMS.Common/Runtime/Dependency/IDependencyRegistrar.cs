#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.CMS.Common.Runtime.Dependency
{
    public interface IDependencyRegistrar
    {
        void Register(IContainerManager containerManager, ITypeFinder typeFinder);

        int Order { get; }
    }
}
