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
using System.Web.Mvc;


namespace Kooboo.CMS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldError
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class JsonResultData
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResultData" /> class.
        /// </summary>
        public JsonResultData()
        {
            this.Success = true;
            Messages = new string[0];
            FieldErrors = new FieldError[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResultData" /> class.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        public JsonResultData(ModelStateDictionary modelState)
            : this()
        {
            this.AddModelState(modelState);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JsonResultData" /> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public string[] Messages { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [reload page].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reload page]; otherwise, <c>false</c>.
        /// </value>
        public bool ReloadPage { get; set; }

        private bool redirectToOpener = true;
        /// <summary>
        /// Gets or sets a value indicating whether [redirect to opener].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [redirect to opener]; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectToOpener { get { return redirectToOpener; } set { redirectToOpener = value; } }

        /// <summary>
        /// Gets or sets the field errors.
        /// </summary>
        public FieldError[] FieldErrors { get; set; }

        /// <summary>
        /// Gets or sets the open URL.
        /// Use in the preview url
        /// </summary>
        /// <value>
        /// The open URL.
        /// </value>
        public string OpenUrl { get; set; }

        public bool ClosePopup { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the state of the model.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        public void AddModelState(ModelStateDictionary modelState)
        {
            foreach (var ms in modelState)
            {
                foreach (var err in ms.Value.Errors)
                {
                    this.AddFieldError(ms.Key, err.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Adds the field error.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public void AddFieldError(string fieldName, string message)
        {
            Success = false;
            FieldErrors = FieldErrors.Concat(new[] { new FieldError() { FieldName = fieldName, ErrorMessage = message } }).ToArray();
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public void AddMessage(string message)
        {
            Messages = Messages.Concat(new[] { message }).ToArray();

        }
        public void AddErrorMessage(string message)
        {
            Messages = Messages.Concat(new[] { message }).ToArray();

            Success = false;
        }
        /// <summary>
        /// Adds the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public void AddException(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
            AddErrorMessage(e.Message);
        }

        #endregion
    }

    public static class JsonResultEntryExtensions
    {
        public static JsonResultData RunWithTry(this JsonResultData jsonResultEntry, Action<JsonResultData> runMethod)
        {
            try
            {
                runMethod(jsonResultEntry);
            }
            catch (Exception e)
            {
                jsonResultEntry.AddException(e);
            }
            return jsonResultEntry;
        }
    }
}
