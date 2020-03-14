using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace MyCrmPlugin
{
    /// <summary>
    /// Post-Operation plugin
    /// </summary>
    public class OnContactCreateNewTaskPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity contactEntity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.
                    Entity newTaskEntity = new Entity("task");

                    // set attributes
                    newTaskEntity.Attributes.Add("subject", "Follow-up");
                    newTaskEntity.Attributes.Add("description", "Please follow up within 2 days of creation");

                    newTaskEntity.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));

                    newTaskEntity.Attributes.Add("prioritycode", new OptionSetValue(2));

                    //newTaskEntity.Attributes["regardingobjectid"] = new EntityReference("contact", contactEntity.Id);
                    newTaskEntity.Attributes.Add("regardingobjectid", contactEntity.ToEntityReference());

                    service.Create(newTaskEntity);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException(ex.Detail.Message);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("OnContactCreateNewTaskPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
