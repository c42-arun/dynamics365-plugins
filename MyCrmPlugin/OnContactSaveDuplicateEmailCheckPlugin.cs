using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyCrmPlugin
{
    /// <summary>
    /// Pre-Validation plugin
    /// </summary>
    public class OnContactSaveDuplicateEmailCheckPlugin : IPlugin
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
                    if (contactEntity.Attributes.Contains("emailaddress1"))
                    {
                        string emailAddress1 = contactEntity.Attributes["emailaddress1"].ToString();

                        // find if there any existing contacts have the same email
                        QueryExpression query = new QueryExpression("contact")
                        {
                            ColumnSet = new ColumnSet(new [] {"emailaddress1"})
                        };

                        query.Criteria.AddCondition(new ConditionExpression("emailaddress1", ConditionOperator.Equal, emailAddress1));
                        
                        EntityCollection contactsWithSameEmail = service.RetrieveMultiple(query);

                        if (contactsWithSameEmail.Entities.Any())
                        {
                            throw new InvalidPluginExecutionException("Contact with same email already exists");
                        }
                    }
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
