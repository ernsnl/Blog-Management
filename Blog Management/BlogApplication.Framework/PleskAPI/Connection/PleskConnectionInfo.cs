using System.Xml.Schema;

namespace BlogApplication.Framework.PleskAPI.Connection
{
    public class PleskConnectionInfo
    {
        public string Hostname = "188.121.43.28";   // Panel Hostname
        public string Login = "ernsnl";               // Administrator's Login
        public string Password = "3736E25081993e";                // Administrator's Password
        public string Protocol = "1.6.6.0";     // API RPC Version Protocol. 

        // Handler for receiving information about document type definition (DTD),
        // XML-Data Reduced (XDR) schema, and XML Schema definition language (XSD) 
        // schema validation errors. 

        public string AgentEntryPoint { get { return "https://" + Hostname + ":8443/enterprise/control/agent.php"; } }
        public string InputValidationSchema { get { return "https://" + Hostname + ":8443/schemas/rpc/" + Protocol + "/agent_input.xsd"; } }
        public string OutputValidationSchema { get { return "https://" + Hostname + ":8443/schemas/rpc/" + Protocol + "/agent_output.xsd"; } }
    }
}