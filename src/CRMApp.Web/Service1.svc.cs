using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.ServiceModel.Description;

namespace CRMApp.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        IOrganizationService _service;

        public string GetData(int value)
        {
            ConnectToMSCRM(
                Environment.GetEnvironmentVariable("CRMUsername"),
                Environment.GetEnvironmentVariable("CRMPassword"),
                Environment.GetEnvironmentVariable("CRMSoapOrgServiceUri"));

            var userId = ((WhoAmIResponse)_service.Execute(new WhoAmIRequest())).UserId;

            if (userId != Guid.Empty)
            {
                Console.WriteLine("Connection Established Successfully");
            }

            //return string.Format("You entered: {0}", value);

            return userId.ToString();
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        private void ConnectToMSCRM(string username, string password, string soapOrgServiceUri)
        {
            try
            {
                var credentials = new ClientCredentials();
                credentials.UserName.UserName = username;
                credentials.UserName.Password = password;
                Uri serviceUri = new Uri(soapOrgServiceUri);
                // See https://community.dynamics.com/crm/f/microsoft-dynamics-crm-forum/281686/how-resolve-an-existing-connection-was-forcibly-closed-by-the-remote-host-in-ssrs-report/807188 if it throws
                var proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                _service = (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
