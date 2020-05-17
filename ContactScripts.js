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