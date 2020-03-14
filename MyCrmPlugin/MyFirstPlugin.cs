using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyCrmPlugin
{
    /// <summary>
    /// Pre-Operation plugin
    /// </summary>
    public class MyFirstPlugin : IPlugin
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
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.
                    string firstName = string.Empty;

                    // Read form attributes - check for exists first
                    if (entity.Attributes.Contains("firstname"))
                        firstName = entity.Attributes["firstname"].ToString();

                    // required attribute, no check required
                    string lastName = entity.Attributes["lastname"].ToString();

                    // Assign to attributes
                    entity.Attributes["description"] = $"Welcome {firstName} {lastName}!";
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyFirstPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyFirstPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
