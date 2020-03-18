using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "AuthType = Office365; Username = arun@cloudfortytwo.com; Password = Th1nkPad201805; Url = https://cloudfortytwo.crm11.dynamics.com/";

            CrmServiceClient client = new CrmServiceClient(connectionString);

            if (!client.IsReady)
            {
                Console.WriteLine(client.LastCrmError);
                Console.WriteLine(client.LastCrmException);
                Console.ReadLine();
                return;
            }

            // CreateContact(client);
            GetContactsUsingFetchQuery(client);

            Console.ReadLine();
        }

        private static void GetContactsUsingFetchQuery(CrmServiceClient client)
        {
            FetchExpression fetchQuery = new FetchExpression(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='contact'>
                <attribute name='fullname' />
                <attribute name='telephone1' />
                <attribute name='contactid' />
                <attribute name='address1_city' />
                <order attribute='fullname' descending='false' />
                <filter type='and'>
                  <condition attribute='address1_city' operator='like' value='B%' />
                </filter>
              </entity>
            </fetch>");

            EntityCollection collection = client.RetrieveMultiple(fetchQuery);

            foreach (Entity contact in collection.Entities)
            {
                Console.WriteLine($"{contact.Attributes["fullname"]} lives in {contact.Attributes["address1_city"]}");
            }
        }

        private static void CreateContact(CrmServiceClient client)
        {
            Entity contact = new Entity("contact");
            contact.Attributes.Add("lastname", "Xrm Client App");
            contact.Attributes.Add("firstname", "Arun");
            contact.Attributes.Add("emailaddress1", "test@contact.com");

            Guid guid = client.Create(contact);

            Console.WriteLine(guid);
        }
    }
}
