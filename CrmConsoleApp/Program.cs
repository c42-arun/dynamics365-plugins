﻿using Cloud42;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Activities.Expressions;
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
            // GetContactsUsingFetchQuery(client);
            // GetContactsUsingLinq(client);
            // GetContactsWithAccountsUsingLinq(client);
            // GetContactsWithAccountsUsingLinqAndStronglyTypedClasses(client);
            BatchCreateEntitiesUsingServiceExecute(client);

            Console.ReadLine();
        }

        private static void GetContactsUsingLinq(CrmServiceClient client)
        {
            using (OrganizationServiceContext context = new OrganizationServiceContext(client))
            {
                // get all contacts
                var contacts = context.CreateQuery("contact").ToList();

                int id = 1;
                foreach (var c in contacts)
                {
                    if (!c.Attributes.ContainsKey("fullname"))
                        Console.Write($"{id}: ");
                    else
                        Console.WriteLine(c["fullname"]);

                    id++;
                }
            }
        }

        private static void GetContactsWithAccountsUsingLinq(CrmServiceClient client)
        {
            using (OrganizationServiceContext context = new OrganizationServiceContext(client))
            {
                var records = from c in context.CreateQuery("contact")
                              join
                              a in context.CreateQuery("account")
                              on c["parentcustomerid"] equals a["accountid"]
                              where c["parentcustomerid"] != null
                              select new
                              {
                                  ContactName = c["fullname"],
                                  AccountName = a["name"]
                              };

                foreach (var r in records)
                {
                    Console.WriteLine($"{r.ContactName} - {r.AccountName}");
                }
            }
        }

        /// <summary>
        /// CrmSvcUtil.exe /interactivelogin /out:CrmEntities.cs /namespace:Cloud42 /serviceContextName:ServiceContext
        /// </summary>
        /// <param name="client"></param>
        private static void GetContactsWithAccountsUsingLinqAndStronglyTypedClasses(CrmServiceClient client)
        {
            using (ServiceContext context = new ServiceContext(client))
            {
                var records = from c in context.ContactSet
                              join
                              a in context.AccountSet
                              on c.ParentCustomerId.Id equals a.AccountId
                              where c.ParentCustomerId != null
                              select new
                              {
                                  ContactName = c.FullName,
                                  AccountName = a.Name
                              };

                foreach (var r in records)
                {
                    Console.WriteLine($"{r.ContactName} - {r.AccountName}");
                }
            }
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

            // RetrieveMultiple() is simply a helper wrapper around Execute(<RetrieveMultipleRequest>)
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

            // Create() is simply a helper wrapper around Execute(<CreateRequest>)
            Guid guid = client.Create(contact);

            Console.WriteLine(guid);
        }

        private static void BatchCreateEntitiesUsingServiceExecute(CrmServiceClient client)
        {
            Entity contact = new Entity("contact");
            contact.Attributes.Add("lastname", "Kumar");
            contact.Attributes.Add("firstname", "Arun");
            contact.Attributes.Add("emailaddress1", "test2@contact.com");

            Account account = new Account
            {
                Name = "Cloud 42 Main Account"
            };

            ExecuteMultipleRequest multipleRequest = new ExecuteMultipleRequest() { 
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings { ContinueOnError = true, ReturnResponses = true }
            };
            multipleRequest.Requests.AddRange(new[] {
                new CreateRequest { Target = contact },
                new CreateRequest { Target = account }
            });

            ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse) client.Execute(multipleRequest);

            foreach(var response in multipleResponse.Responses)
                Console.WriteLine(response.Response);
        }
    }
}
