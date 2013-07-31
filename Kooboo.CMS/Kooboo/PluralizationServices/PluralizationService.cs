#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Globalization;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PluralizationService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo Culture
        {
            get;
            protected set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified word is plural.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is plural; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsPlural(string word);
        /// <summary>
        /// Determines whether the specified word is singular.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is singular; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsSingular(string word);
        /// <summary>
        /// Pluralizes the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public abstract string Pluralize(string word);
        /// <summary>
        /// Singularizes the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public abstract string Singularize(string word);
        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static PluralizationService CreateService(CultureInfo culture)
        {
            if (culture.TwoLetterISOLanguageName == "en")
            {
                return new EnglishPluralizationService();
            }
            throw new NotImplementedException("We don't support locales other than english yet");
        }
        #endregion
    }
}
