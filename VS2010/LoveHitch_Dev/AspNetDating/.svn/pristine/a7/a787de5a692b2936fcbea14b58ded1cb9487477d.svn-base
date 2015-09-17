using System;
using System.Net;
using Google.Contacts;
using Google.GData.Client;

namespace AspNetDating.Classes.ContactsExtractor
{
    public class GmailExtract : IMailContactExtract
    {
        #region IMailContactExtract Members

        public bool Extract(NetworkCredential credential, out MailContactList list)
        {
            bool result = false;
            list = new MailContactList();

            try
            {
                var rs = new RequestSettings("eStream-AspNetDating", credential.UserName, credential.Password)
                             {AutoPaging = true};

                var cr = new ContactsRequest(rs);

                Feed<Contact> f = cr.GetContacts();
                foreach (Contact e in f.Entries)
                {
                    foreach (var email in e.Emails)
                    {
                        var mailContact = new MailContact {Email = email.Address, Name = e.Title};
                        list.Add(mailContact);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Global.Logger.LogError(ex);
            }

            return result;
        }

        #endregion
    }
}