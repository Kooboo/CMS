using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using System.Web.Script.Serialization;

namespace Kooboo.Web.Mvc
{
    public class ModelClientMetadataBuilder<TModel> : ModelClientMetadataBuilder
    {
        public ModelClientMetadataBuilder(string formAction,string formId)
        {
            this.FieldValidators = new Dictionary<string, FieldValidationMetadata>();
            this.FormAction = formAction;
            this.FormId = formId;
        }

        string FormAction
        {
            get;
            set;
        }

        string FormId
        {
            get;
            set;
        }

        FieldValidationMetadata GetValidationMetadataForField(string fieldName, bool createIfNotFound)
        {
            //if (String.IsNullOrEmpty(fieldName))
            //{
            //    throw Error.ParameterCannotBeNullOrEmpty("fieldName");
            //}

            FieldValidationMetadata metadata;
            if (!FieldValidators.TryGetValue(fieldName, out metadata))
            {
                if (createIfNotFound)
                {
                    metadata = new FieldValidationMetadata()
                    {
                        FieldName = fieldName
                    };
                    FieldValidators[fieldName] = metadata;
                }
            }
            return metadata;
        }

        Dictionary<string, FieldValidationMetadata> FieldValidators
        {
            get;
            set;
        }

        public string ToMetadata()
        {
            string format = "\r\n//<![CDATA[\r\nif (!window.mvcClientValidationMetadata) {{ window.mvcClientValidationMetadata = []; }}\r\nwindow.mvcClientValidationMetadata.push({0});\r\n//]]>\r\n".Replace("\r\n", Environment.NewLine);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            SortedDictionary<string, object> dict = new SortedDictionary<string, object>() {
                { "Fields", FieldValidators.Values },
                { "FormId",this.FormId},
                { "FormAction", this.FormAction}
            };
          

            var jsonValidationMetadata = serializer.Serialize(dict);
            
            string str3 = string.Format(CultureInfo.InvariantCulture, format, new object[] { jsonValidationMetadata });

            return str3;

        }      

        public ModelClientMetadataBuilder<TModel> ValidateFor<TProperty>(Expression<Func<TModel, TProperty>> expression,string element=null)
        {
           var modelMetadata  = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());

           if (element == null)
           {
               MemberExpression call;

               var body = expression.Body;

               if (body.NodeType == ExpressionType.Lambda)
               {
                   body = ((LambdaExpression)body).Body;
               }

               if (body.NodeType == ExpressionType.MemberAccess)
               {
                   call = (MemberExpression)body;
                   element = call.Member.Name;
               }
               else
               {
                   throw new NodeTypeNotSupportException(body.NodeType);
               }
           }
           AppendFieldValidationMetadata(element, modelMetadata);

            return this;   
        }

        private void AppendFieldValidationMetadata(string element, ModelMetadata modelMetadata)
        {
            FieldValidationMetadata fieldMetadata = GetValidationMetadataForField(element, true /* createIfNotFound */);

            // write rules to context object
            IEnumerable<ModelValidator> validators = ModelValidatorProviders.Providers.GetValidators(modelMetadata, new ViewContext());
            foreach (ModelClientValidationRule rule in validators.SelectMany(v => v.GetClientValidationRules()))
            {
                fieldMetadata.ValidationRules.Add(rule);
            }
        }

        public ModelClientMetadataBuilder<TModel> Validate(string element, ValidationAttribute[] attributes)
        {
            

            //FieldValidationMetadata fieldMetadata = GetValidationMetadataForField(element, true /* createIfNotFound */);
            //foreach (ModelClientValidationRule rule in attributes.OfType<IClientValidatable>().SelectMany(i=>i.GetClientValidationRules()))
            //{
            //    fieldMetadata.ValidationRules.Add(rule);
            //}

            return this;
        }

        public ModelClientMetadataBuilder<TModel> Validate(string element, FieldValidationMetadata metadata)
        {
            var item = GetValidationMetadataForField(element,false);
            if (item == null)
            {
                this.FieldValidators.Add(element, metadata);
            }

            return this;
            
        }
    }

    public abstract class ModelClientMetadataBuilder
    {
        public static ModelClientMetadataBuilder<TModel> BuildFor<TModel>(string formAction,string formId=null)
        {
            return new ModelClientMetadataBuilder<TModel>(formAction, formId);
        }
    }
}
