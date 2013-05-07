#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
namespace Kooboo.CMS.Common.Runtime
{
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
