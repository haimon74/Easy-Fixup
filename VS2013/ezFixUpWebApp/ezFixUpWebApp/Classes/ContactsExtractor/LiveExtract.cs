using System;
using System.Net;
using System.Xml;

namespace ezFixUp.Classes.ContactsExtractor
{
    public class LiveExtract : IMailContactExtract
    {
        #region IMailContactExtract Members

        public bool Extract(NetworkCredential credential, out MailContactList list)
        {
            list = new MailContactList();

            bool result = false;

            try
            {
                var ticketAcquirer = new TicketAcquirer();
                string ticket = ticketAcquirer.GetTicket(credential);
                if (string.IsNullOrEmpty(ticket))
                {
                    return false;
                }

                var urib = new UriBuilder();
                urib.Scheme = "HTTPS";
                urib.Path = string.Format("/{0}/LiveContacts", credential.UserName);
                urib.Host = "cumulus.services.live.com";
                urib.Port = 443;

                var request = (HttpWebRequest) WebRequest.Create((Uri) urib.Uri);

                string authHeader = string.Format("WLID1.0 t=\"{0}\"", ticket);
                request.Headers.Add("Authorization", authHeader);

                WebResponse response = request.GetResponse();
                if (response.ContentLength != 0)
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(response.GetResponseStream());
                    XmlNodeList contacts = xmlDocument.SelectNodes("/LiveContacts/Contacts/Contact");
                    foreach (XmlNode node in contacts)
                    {
                        XmlNode firstName = node.SelectSingleNode("Profiles/Personal/FirstName");
                        XmlNode lastName = node.SelectSingleNode("Profiles/Personal/LastName");
                        XmlNode firstMail = node.SelectSingleNode("Emails/Email/Address");

                        var mailContact = new MailContact();
                        mailContact.Name = string.Format("{0} {1}", firstName.InnerText, lastName.InnerText);
                        mailContact.Email = firstMail.InnerText;
                        list.Add(mailContact);
                    }
                }
                result = true;
            }
            catch
            {
            }
            return result;
        }

        #endregion
    }
}