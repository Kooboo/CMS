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
using System.Resources;
using System.Collections.Specialized;

namespace Kooboo
{
    /// <summary>
    /// To reader system resources
    /// </summary>
    public static class SR
    {
        public static NameValueCollection resources = new NameValueCollection();
        static SR()
        {
            resources["RequiredAttribute_ValidationError"] = "The {0} field is required.";
            resources["StringLengthAttribute_ValidationError"] = "The field {0} must be a string with a maximum length of {1}.";
            resources["RangeAttribute_ValidationError"] = "The field {0} must be between {1} and {2}.";
            resources["RegexAttribute_ValidationError"] = "The field {0} must match the regular expression '{1}'.";
            resources["ClientDataTypeModelValidatorProvider_FieldMustBeNumeric"] = "The field {0} must be a number.";
            resources["RangeAttribute_MinGreaterThanMax"] = "The maximum value '{0}' must be greater than or equal to the minimum value '{1}'.";
            resources["RangeAttribute_Must_Set_Min_And_Max"] = "The minimum and maximum values must be set.";
            resources["RangeAttribute_Must_Set_Operand_Type"] = "The OperandType must be set when strings are used for minimum and maximum values.";
            resources["RangeAttribute_ArbitraryTypeNotIComparable"] = "The type {0} must implement {1}.";

            //System_Web_Mvc_Resources
            resources["Common_ValueNotValidForProperty"] = "The value '{0}' is invalid.";
            resources["ClientDataTypeModelValidatorProvider_FieldMustBeNumeric"] = "The field {0} must be a number.";
            resources["ViewPageHttpHandlerWrapper_ExceptionOccurred"] = "Execution of the child request failed. Please examine the InnerException for more information.";
            resources["RedirectAction_CannotRedirectInChildAction"] = "Child actions are not allowed to perform redirect actions.";
            resources["Common_NoRouteMatched"] = "No route in the route table matches the supplied values.";
            resources["RemoteAttribute_NoUrlFound"] = "No url for remote validation could be found.";
            resources["Controller_UnknownAction"] = "A public action method '{0}' was not found on controller '{1}'.";
            resources["Common_NullOrEmpty"] = "Value cannot be null or empty.";

            //System_Web_Resources
            resources["Webevent_event_custom_event_details"] = "Custom event details: ";
            resources["Path_not_found"] = "Path '{0}' was not found.";
            resources["Webevent_event_request_url"] = "Request URL: {0}";
            resources["Webevent_event_request_path"] = "Request path: {0}";
            resources["Webevent_event_user_host_address"] = "User host address: {0}";
            resources["Webevent_event_user"] = "User: {0}";
            resources["Webevent_event_is_authenticated"] = "Is authenticated: True";
            resources["Webevent_event_is_not_authenticated"] = "Is authenticated: False";
            resources["Webevent_event_authentication_type"] = "Authentication Type: {0}";
            resources["Webevent_event_thread_account_name"] = "Thread account name: {0}";
            resources["Webevent_event_thread_id"] = "Thread ID: {0}";
            resources["Webevent_event_is_impersonating"] = "Is impersonating: True";
            resources["Webevent_event_is_not_impersonating"] = "Is impersonating: False";
            resources["Webevent_event_stack_trace"] = "Stack trace: {0}";
            resources["Webevent_event_application_domain"] = "Application domain: {0}";
            resources["Webevent_event_trust_level"] = "Trust level: {0}";
            resources["Webevent_event_application_virtual_path"] = "Application Virtual Path: {0}";
            resources["Webevent_event_application_path"] = "Application Path: {0}";
            resources["Webevent_event_machine_name"] = "Machine name: {0}";
            resources["Webevent_event_code"] = "Event code: {0}";
            resources["Webevent_event_message"] = "Event message: {0}";
            resources["Webevent_event_time"] = "Event time: {0}";
            resources["Webevent_event_time_Utc"] = "Event time (UTC): {0}";
            resources["Webevent_event_id"] = "Event ID: {0}";
            resources["Webevent_event_sequence"] = "Event sequence: {0}";
            resources["Webevent_event_detail_code"] = "Event detail code: {0}";
            resources["Webevent_event_application_information"] = "Application information:";
            resources["Webevent_event_exception_information"] = "Exception information:";
            resources["Webevent_event_inner_exception_information"] = "Inner exception information (level {0}):";
            resources["Webevent_event_exception_type"] = "Exception type: {0}";
            resources["Webevent_event_exception_message"] = "Exception message: {0}";
            resources["Webevent_event_request_information"] = "Request information:";
            resources["Webevent_event_thread_information"] = "Thread information:";

            
            //System_Linq_Resources
            resources["NoElements"] = "Sequence contains no elements";
        }
        public static string GetString(string key)
        {
            string s = resources[key];
            if (string.IsNullOrEmpty(s))
            {
                s = key;
            }
            return s;
        }
        // Mono does not support the ResourceManager api
        //static SR()
        //{
        //    System_Web_Resources = new ResourceManager("System.Web", typeof(System.Web.HttpContext).Assembly);
        //    System_Web_Mvc_Resources = new ResourceManager("System.Web.Mvc.Resources.MvcResources", typeof(System.Web.Mvc.Controller).Assembly);
        //    System_ComponentModel_DataAnnotations_Resources = new ResourceManager("System.ComponentModel.DataAnnotations.Resources.DataAnnotationsResources", typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly);            
        //    System_Linq_Resources = new ResourceManager("System.Linq", typeof(System.Linq.Enumerable).Assembly);
        //}
        //public static ResourceManager System_Linq_Resources { get; private set; }
        //public static ResourceManager System_Web_Resources { get; private set; }
        //public static ResourceManager System_Web_Mvc_Resources { get; private set; }
        //public static ResourceManager System_ComponentModel_DataAnnotations_Resources { get; private set; }
    }
}
