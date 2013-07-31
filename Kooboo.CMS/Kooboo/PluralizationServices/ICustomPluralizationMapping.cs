#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
    /// <summary>
    /// 
    /// </summary>
	public interface ICustomPluralizationMapping
	{
        /// <summary>
        /// Adds the word.
        /// </summary>
        /// <param name="singular">The singular.</param>
        /// <param name="plural">The plural.</param>
		void AddWord(string singular, string plural);
	}
}
