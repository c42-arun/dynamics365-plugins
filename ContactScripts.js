function SayHello(executionContext) {
    var formContext = executionContext.getFormContext();

    var firstName = formContext.getAttribute("firstname").getValue();

    alert("Hello " + firstName);
}
