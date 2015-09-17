/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AspNetDating.Classes
{
    [Serializable]
    public class Settings
    {
        #region Properties

        private string username;

        [NonSerialized] private User user;

        public User User
        {
            get
            {
                if (user == null)
                    user = User.Load(username);
                return user;
            }
            set
            {
                user = value;
                username = value.Username;
            }
        }

        public bool NotificationEmails = true;

        #endregion

        public Settings()
        {
        }

        public static string ToXml(Settings settings)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xmls = new XmlSerializer(typeof (Settings));
            xmls.Serialize(ms, settings);
            ms.Position = 0;
            Byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            string xmlString = Encoding.ASCII.GetString(buffer);
            return xmlString;
        }

        public static Settings ToObject(string xml)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xmls = new XmlSerializer(typeof (Settings));

            Byte[] buffer = Encoding.ASCII.GetBytes(xml);
            ms.Position = 0;
            ms.Write(buffer, 0, buffer.Length);

            return (Settings) xmls.Deserialize(ms);
        }
    }
}