using Microsoft.Xrm.Sdk;
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

            Entity contact = new Entity("contact");
            contact.Attributes.Add("lastname", "Xrm Client App");
            contact.Attributes.Add("firstname", "Arun");
            contact.Attributes.Add("emailaddress1", "test@contact.com");

            Guid guid = client.Create(contact);

            Console.WriteLine(guid);
            Console.ReadLine();
        }
    }
}
