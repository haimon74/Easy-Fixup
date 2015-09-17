using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using OAuth.Net.Common;
using OAuth.Net.Components;
using OAuth.Net.Consumer;
using AspNetDating.Classes;

namespace AspNetDating.Classes.MySpace
{
    [DataContract(Name = "UserDataContainer", Namespace = "")]
    public class UserDataContainer
    {
        [DataMember(Name = "entry")]
        public UserData Entry { get; set; }
    }

    [DataContract(Name = "FriendsDataContainer", Namespace = "")]
    public class FriendsDataContainer
    {
        [DataMember(Name = "entry")]
        public List<Friend> Entry { get; set; }
    }

    [DataContract(Name = "UserData", Namespace = "")]
    public class UserData
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public Name Name { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "nickname")]
        public string NickName { get; set; }

        [DataMember(Name = "gender")]
        public string GenderString { get; set; }

        public User.eGender? Gender 
        {
            get
            {
                if (!String.IsNullOrEmpty(GenderString))
                {
                    switch (GenderString.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "male":
                            return User.eGender.Male;
                        case "female":
                            return User.eGender.Female;
                        default:
                            return null;
                    }
                }

                return null;
            }
        }

        [DataMember(Name = "dateOfBirth")]
        public string DateOfBirthString { get; set; }

        public DateTime? DateOfBirth
        {
            get
            {
                if (DateOfBirthString == null)
                    return null;

                return DateTime.Parse(DateOfBirthString, CultureInfo.InvariantCulture);
            }
        }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "photos")]
        public List<Photo> Photos { get; set; }

        public string PrimaryPhotoURL
        {
            get
            {
                if (Photos != null)
                {
                    Photo primaryPhoto = Photos.Where(p => p.Type == "large").FirstOrDefault();

                    if (primaryPhoto != null)
                        return primaryPhoto.PhotoURL;
                }

                return null;
            }
        }

        [DataMember(Name = "emails")]
        public List<EmailAddress> Emails { get; set; }

        public string PrimaryEmail
        {
            get
            {
                if (Emails != null)
                {
                    EmailAddress primaryEmail = Emails.Where(e => e.Primary).FirstOrDefault();

                    if (primaryEmail != null)
                        return primaryEmail.Email;

                    if (Emails.Count > 0)
                        return Emails[0].Email;
                }

                return null;
            }
        }
    }

    [DataContract(Name = "Friend", Namespace = "")]
    public class Friend
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
    }
 
    [DataContract(Name = "Name", Namespace = "")]
    public class Name
    {
        [DataMember(Name = "givenName")]
        public string GivenName { get; set; }
        [DataMember(Name = "familyName")]
        public string FamilyName { get; set; }        
    }

    [DataContract(Name = "Photo", Namespace = "")]
    public class Photo
    {
        [DataMember(Name = "value")]
        public string PhotoURL { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }          
    }

    [DataContract(Name = "Email", Namespace = "")]
    public class EmailAddress
    {
        [DataMember(Name = "value")]
        public string Email { get; set; }
        [DataMember(Name = "primary")]
        public bool Primary { get; set; }
    }

    public static class DataAvailability
    {
        private static readonly OAuthService GetService =
            OAuthService.Create(new Uri("https://api.myspace.com/request_token"),
            new Uri("https://api.myspace.com/authorize"),
            new Uri("https://api.myspace.com/access_token"),
            "GET", false, string.Empty, "HMAC-SHA1", "1.0",
            new OAuthConsumer(Properties.Settings.Default.MySpace_Key, Properties.Settings.Default.MySpace_Secret));

        private static readonly OAuthService PostService =
            OAuthService.Create( DataAvailability.GetService.RequestTokenUrl, DataAvailability.GetService.AuthorizationUrl,
            DataAvailability.GetService.AccessTokenUrl, "POST", DataAvailability.GetService.UseAuthorizationHeader,
            DataAvailability.GetService.Realm, DataAvailability.GetService.SignatureMethod, DataAvailability.GetService.OAuthVersion,
            DataAvailability.GetService.Consumer);

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "callback", Justification = "Callback is a domain term")]
        public static UserData GetUserData(HttpContext context, Uri callback) 
        {
            if (context == null) throw new ArgumentNullException("context");
            if (callback == null) throw new ArgumentNullException("callback");

            var request = OAuthRequest.Create(new Uri("https://api.myspace.com/v2/people/@me/@self"),
                DataAvailability.GetService, context.Session["MySpace_request_token"] as IToken,
                context.Session["MySpace_access_token"] as IToken);

            var parameters = new NameValueCollection()
                {
                    {"fields", "id,name,nickname,status,photos,emails,dateOfBirth,gender"}
                };

            OAuthResponse response = request.GetResource(parameters);

            if (response.HasProtectedResource) 
            { 
                // Store the access token
                context.Session["MySpace_access_token"] = response.Token;
                
                string jsonString;

                using (Stream stream = response.ProtectedResource.GetResponseStream())
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    jsonString = Encoding.UTF8.GetString(buffer);
                }

                var serializer = new DataContractJsonSerializer(typeof(UserDataContainer));
                
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    var myspaceData = (UserDataContainer)serializer.ReadObject(ms);
                    return myspaceData.Entry;
                }
            }
            else
            {
                context.Session["MySpace_request_token"] = response.Token;  
                throw new AuthorizationRequiredException()
                          {
                              AuthorizationUri = GetService.BuildAuthorizationUrl(response.Token, callback)
                          };
            } 
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "callback", Justification = "Callback is a domain term")]
        public static string[] GetFriends(HttpContext context, Uri callback)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (callback == null) throw new ArgumentNullException("callback");

            var request = OAuthRequest.Create(new Uri("https://api.myspace.com/v2/people/@me/@friends"),
                                              DataAvailability.GetService,
                                              context.Session["MySpace_request_token"] as IToken,
                                              context.Session["MySpace_access_token"] as IToken);

            var parameters = new NameValueCollection()
                                 {
                                     {"fields", "id"}
                                 };

            OAuthResponse response = request.GetResource(parameters);

            if (response.HasProtectedResource)
            {
                // Store the access token
                context.Session["MySpace_access_token"] = response.Token;

                string jsonString;

                using (Stream stream = response.ProtectedResource.GetResponseStream())
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    jsonString = Encoding.UTF8.GetString(buffer);
                }

                var serializer = new DataContractJsonSerializer(typeof(FriendsDataContainer));

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    var myspaceData = (FriendsDataContainer)serializer.ReadObject(ms);
                    return (from friend in myspaceData.Entry select friend.ID).ToArray();
                }
            }
            else
            {
                context.Session["MySpace_request_token"] = response.Token;
                throw new AuthorizationRequiredException()
                {
                    AuthorizationUri = GetService.BuildAuthorizationUrl(response.Token, callback)
                };
            } 
        }

        public static void RevokeAccess(HttpContext context)
        {
            context.Session["MySpace_request_token"] = null;
            context.Session["MySpace_access_token"] = null;
        }
    }
}
