namespace AspNetDating.Classes.ContactsExtractor
{
    public class MailContact
    {
        private string _email = string.Empty;
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
    }
}