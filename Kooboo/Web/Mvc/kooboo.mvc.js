 
 /*
 Kooboo Validation
 */

Sys.Mvc.ValidatorRegistry.validators["compare"] = function (rule) {
    // initialization code can go here.
    var compareTo = rule.ValidationParameters["compareTo"];

    // we return the function that actually does the validation 
    return function (value, context) {
        debugger;
        return rule.ErrorMessage;
    }
}