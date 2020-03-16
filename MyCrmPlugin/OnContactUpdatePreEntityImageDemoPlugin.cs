using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyCrmPlugin
{
    public class OnContactUpdatePreEntityImageDemoPlugin : IPlugin
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
                Entity updatedEntity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    string originalBusinessPhone = string.Empty;
                    string modifiedBusinessPhone = string.Empty;

                    // Plug-in business logic goes here.
                    if (updatedEntity.Attributes.Contains("telephone1"))
                        modifiedBusinessPhone = updatedEntity.Attributes["telephone1"].ToString();

                    Entity preImageOriginalEntity = context.PreEntityImages["PreImageWith_telephone1"];
                    if (preImageOriginalEntity.Attributes.Contains("telephone1"))
                        originalBusinessPhone = preImageOriginalEntity.Attributes["telephone1"].ToString();

                    // only way to show a message seems to be via throwing an exception
                    throw new InvalidPluginExecutionException($"Phone number changed from {originalBusinessPhone} to {modifiedBusinessPhone}");
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
