using System.Net;

namespace ezFixUp.Classes.ContactsExtractor
{
    public interface IMailContactExtract
    {
        bool Extract(NetworkCredential credential, out MailContactList list);
    }
}