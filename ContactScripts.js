//function SayHello(executionContext) {
//    var formContext = executionContext.getFormContext();

//    var firstName = formContext.getAttribute("firstname").getValue();

//    alert("Hello " + firstName);
//}

var Sdk = window.Sdk || {};
// JS immeditaely invoked function (IIF)
(function () {
    // 'this' is now the Sdk object
    this.formOnLoad = function (executionContext) {
        //var formContext = executionContext.getFormContext();

        //var firstName = formContext.getAttribute("firstname").getValue();

        //alert("Hello " + firstName);

        var formContext = executionContext.getFormContext();

        var lookupArray = formContext.getAttribute("parentcustomerid").getValue();
        if (lookupArray[0] !== null) {
            var account = lookupArray[0];

            formContext.ui.setFormNotification("GUID of account is " + account.id, "INFO", "1");
            formContext.ui.setFormNotification("Name of account is " + account.name, "INFO", "2");
            formContext.ui.setFormNotification("Entity type is " + account.entityType, "INFO", "3");
        }
    }

    this.shippingMethodOnChange = (executionContext) => {
        var formContext = executionContext.getFormContext();

        if (formContext.getAttribute("address1_shippingmethodcode").getText() === "FedEx") {
            formContext.getControl("address1_freighttermscode").setDisabled(true);
        } else {
            formContext.getControl("address1_freighttermscode").setDisabled(false);
        }
    }
}).call(Sdk);