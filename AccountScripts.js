// JavaScript source code
var Sdk = window.Sdk || {};
(function () {
    this.mainPhoneChange = (executionContext) => {
        var formContext = executionContext.getFormContext();
        var mainPhone = formContext.getAttribute("telephone1").getValue();

        //var expression = new RegExp("^[2-9]\d{2}-\d{3}-\d{4}$");
        //if (!expression.test(mainPhone)) {
        //    alert("Please enter phone number in US format");
        //}
        if (mainPhone.length > 10) {
            //alert("Phone number should be < 10 ");
            formContext.getControl("telephone1").setNotification("Phone number should be < 10 ", "telephone1Notification");
            formContext.ui.setFormNotification("Check errors", "INFO", "formNotification");
        } else {
            formContext.getControl("telephone1").clearNotification("telephone1Notification");
            formContext.ui.clearFormNotification("formNotification");
        }
    }
}).call(Sdk);