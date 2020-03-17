using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Linq;

namespace MyCustomWorkflows
{
    /// <summary>
    /// 
    /// https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/workflow/sample-create-custom-workflow-activity
    /// </summary>
    public class GetTaxWorkflow : CodeActivity
    {
        [Input("Key")]
        public InArgument<string> Key { get; set; }

        [Output("TaxRate")]
        public OutArgument<string> TaxRate { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // read input parameters
            string configKey = Key.Get(executionContext);

            // Get configuration setting from CRM DB entity "contoso_configuration"
            QueryByAttribute query = new QueryByAttribute("contoso_configuration")
            {
                ColumnSet = new ColumnSet(new[] { "contoso_value" })
            };
            query.AddAttributeValue("contoso_name", configKey);

            EntityCollection entityCollection = service.RetrieveMultiple(query);

            if (entityCollection.Entities.Count != 1)
            {
                tracingService.Trace($"Incorrect configuration for key: {configKey}");
                return;
            }

            Entity configEntity = entityCollection.Entities.First();

            // set output parameters
            TaxRate.Set(executionContext, configEntity.Attributes["contoso_value"].ToString());
        }
    }
}
