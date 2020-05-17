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
        var formContext = executionContext.getFormContext();

        var firstName = formContext.getAttribute("firstname").getValue();

        alert("Hello " + firstName);
    }
}).call(Sdk);