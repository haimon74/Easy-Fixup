using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls.WebParts;


namespace ezFixUp.Classes
{
    [Serializable]
    public class PersonalizationInfo
    {
        public SerializableDictionary<string, byte[]> userPersonalizationData = new SerializableDictionary<string, byte[]>();
    }

    public class CustomPersonalizationProvider : PersonalizationProvider
    {
        public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope,
                                                                     PersonalizationStateQuery query, int pageIndex,
                                                                     int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            throw new NotImplementedException();
        }

        protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName,
                                                         ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            PageBase page = webPartManager.Page as PageBase;

            if (page != null && page.CurrentUserSession != null)
            {
                userName = page.CurrentUserSession.Username;
                User user = Classes.User.Load(userName);
                PersonalizationInfo info = user.PersonalizationInfo;

                if (info != null)
                {
                    string key = String.Format("{0}_{1}_{2}", webPartManager.ID, path, userName);
                    byte[] data = null;
                    try
                    {
                        if (info.userPersonalizationData.ContainsKey(key))
                            data = info.userPersonalizationData[key];
                    }
                    catch (KeyNotFoundException)
                    {
                        //info is somehow corrupted
                        //user.ResetPersonalization();
                    }

                    if (data != null)
                    {
                        userDataBlob = data;
                    }
                }
            }
        }

        protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
        {
        }

        public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override IDictionary DetermineUserCapabilities(WebPartManager webPartManager)
        {
            //return base.DetermineUserCapabilities(webPartManager);
            ICollection supportedUserCapabilities = CreateSupportedUserCapabilities();
            if ((supportedUserCapabilities != null) && (supportedUserCapabilities.Count != 0))
            {
                IDictionary capabilities = new HybridDictionary();

                foreach (WebPartUserCapability capability in supportedUserCapabilities)
                {
                        capabilities[capability] = capability;
                }

                return capabilities;                 
            }
            else return new HybridDictionary();
        }

        public override PersonalizationScope DetermineInitialScope(WebPartManager webPartManager, PersonalizationState loadedState)
        {
            //return base.DetermineInitialScope(webPartManager, loadedState);
            return PersonalizationScope.User;
        }

        protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName,
                                                        byte[] dataBlob)
        {
            PageBase page = webPartManager.Page as PageBase;

            if (page != null && page.CurrentUserSession != null)
            {
                userName = page.CurrentUserSession.Username;
                User user = Classes.User.Load(userName);

                PersonalizationInfo info = user.PersonalizationInfo;
                string key = String.Format("{0}_{1}_{2}", webPartManager.ID, path, userName);

                if (info == null)
                {
                    info = new PersonalizationInfo();
                }
                info.userPersonalizationData[key] = dataBlob;
                user.PersonalizationInfo = info;
                page.CurrentUserSession.PersonalizationInfo = info;

                user.Update();
            }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
