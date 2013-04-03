using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;

namespace Kooboo.CMS.Sites
{
    public class FieldError
    {
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class JsonResultEntry
    {
        public JsonResultEntry()
        {
            this.Success = true;
            Messages = new string[0];
            FieldErrors = new FieldError[0];
        }

        public JsonResultEntry(ModelStateDictionary modelState)
            : this()
        {
            this.AddModelState(modelState);
        }

        public JsonResultEntry SetFailed()
        {
            this.Success = false;
            return this;
        }

        public JsonResultEntry SetSuccess()
        {
            //this.Messages = new string[1] { "Submit successfully!".Localize() };
            this.Success = true;
            return this;
        }

        public JsonResultEntry AddModelState(ModelStateDictionary modelState)
        {
            foreach (var ms in modelState)
            {
                foreach (var err in ms.Value.Errors)
                {
                    this.AddFieldError(ms.Key, err.ErrorMessage);
                }
            }

            return this;
        }

        public bool Success { get; set; }
        public string[] Messages { get; set; }
        public object Model { get; set; }
        public string RedirectUrl { get; set; }
        public bool ReloadPage { get; set; }
        private bool redirectToOpener = true;
        public bool RedirectToOpener { get { return redirectToOpener; } set { redirectToOpener = value; } }
        public FieldError[] FieldErrors { get; set; }
        public JsonResultEntry AddFieldError(string fieldName, string message)
        {
            Success = false;
            FieldErrors = FieldErrors.Concat(new[] { new FieldError() { FieldName = fieldName, ErrorMessage = message } }).ToArray();
            return this;
        }
        public JsonResultEntry AddMessage(string message)
        {
            Messages = Messages.Concat(new[] { message }).ToArray();
            return this;
        }
        public JsonResultEntry AddException(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
            //the exception message can be wrapped.
            this.Success = false;
            return AddMessage(e.Message);
        }
    }
}