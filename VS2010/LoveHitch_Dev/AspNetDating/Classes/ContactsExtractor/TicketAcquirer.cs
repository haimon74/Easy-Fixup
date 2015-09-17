using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace AspNetDating.Classes.ContactsExtractor
{
    internal class TicketAcquirer
    {
        private const string applicationId = "10";
        // An arbitrary value that will be defined in the next non-alpha release

        public string GetTicket(NetworkCredential credentail)
        {
            string soapEnvelope =
                @"<s:Envelope
    xmlns:s = ""https://www.w3.org/2003/05/soap-envelope""
    xmlns:wsse = ""https://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""
    xmlns:saml = ""urn:oasis:names:tc:SAML:1.0:assertion""
    xmlns:wsp = ""http://schemas.xmlsoap.org/ws/2004/09/policy""
    xmlns:wsu = ""https://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""
    xmlns:wsa = ""https://www.w3.org/2005/08/addressing""
    xmlns:wssc = ""http://schemas.xmlsoap.org/ws/2005/02/sc""
    xmlns:wst = ""http://schemas.xmlsoap.org/ws/2005/02/trust"">
    <s:Header>
        <wlid:ClientInfo xmlns:wlid = ""http://schemas.microsoft.com/wlid"">
            <wlid:ApplicationID>" +
                applicationId +
                @"</wlid:ApplicationID>
        </wlid:ClientInfo>
        <wsa:Action s:mustUnderstand = ""1"">http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</wsa:Action>
        <wsa:To s:mustUnderstand = ""1"">https://dev.login.live.com/wstlogin.srf</wsa:To>
        <wsse:Security>
            <wsse:UsernameToken wsu:Id = ""user"">
                <wsse:Username>" +
                credentail.UserName + @"</wsse:Username>
                <wsse:Password>" + credentail.Password +
                @"</wsse:Password>
            </wsse:UsernameToken>
        </wsse:Security>
    </s:Header>
    <s:Body>
        <wst:RequestSecurityToken Id = ""RST0"">
            <wst:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</wst:RequestType>
            <wsp:AppliesTo>
                <wsa:EndpointReference>
                    <wsa:Address>https://live.com</wsa:Address>
                </wsa:EndpointReference>
            </wsp:AppliesTo>
            <wsp:PolicyReference URI = ""MBI""></wsp:PolicyReference>
        </wst:RequestSecurityToken>
    </s:Body>
</s:Envelope>";

            const string url = @"https://dev.login.live.com/wstlogin.srf";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=UTF-8";
            request.Timeout = 10*1000; // Wait for at most 10 seconds
            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
            request.GetRequestStream().Close();
            WebResponse response;
            response = request.GetResponse();
            string xml;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                xml = reader.ReadToEnd();
            }
            response.Close();
            var document = new XmlDocument();
            document.LoadXml(xml);
            var nsManager = new XmlNamespaceManager(document.NameTable);
            nsManager.AddNamespace("wsse",
                                   "https://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            XmlNode node = document.SelectSingleNode(@"//wsse:BinarySecurityToken/text()", nsManager);
            if (node == null)
            {
                return null; // The wsse:BinarySecurityToken element is missing. Examine the xml for error information
            }
            else
            {
                return node.Value;
            }
        }
    }
}