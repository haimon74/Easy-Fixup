using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;


namespace ezFixUp.Classes
{
    public class ScheduledAnnouncement
    {
        #region fields

        private int? id;
        private string name;
        private string subject;
        private string body;
        private Classes.User.eGender? gender;
        private bool? paidMember;
        private bool? hasPhotos;
        private bool? hasProfile;
        private int? languageID;
        private string country;
        private string region;
        private eType type;
        private DateTime? date;
        private int? days;

        public enum eType
        {
            SpecificDate,
            DaysAfterInactivity,
            DaysAfterRegistration,
            DaysBeforeMembershipEnd,
            Weekly,
            Monthly
        }

        #endregion

        #region Properties

        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public Classes.User.eGender? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public bool? PaidMember
        {
            get { return paidMember; }
            set { paidMember = value; }
        }

        public bool? HasPhotos
        {
            get { return hasPhotos; }
            set { hasPhotos = value; }
        }

        public bool? HasProfile
        {
            get { return hasProfile; }
            set { hasProfile = value; }
        }

        public int? LanguageId
        {
            get { return languageID; }
            set { languageID = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

        public int? Days
        {
            get { return days; }
            set { days = value; }
        }

        #endregion

        #region Methods

        public static ScheduledAnnouncement[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null, null, null);
        }

        public static ScheduledAnnouncement Fetch(int id)
        {
            ScheduledAnnouncement[] scheduledAnnouncements = Fetch(id, null, null, null, null, null, null, null, null);

            if (scheduledAnnouncements.Length > 0)
            {
                return scheduledAnnouncements[0];
            }

            return null;
        }

        private static ScheduledAnnouncement[] Fetch(int? id, Classes.User.eGender? gender, bool? paidMember, bool? hasPhotos,
                                                        bool? hasProfile, int? languageID, string country, string region,
                                                        eType? type)
        {
            //using (var conn = Config.DB.Open())
            {
                List<ScheduledAnnouncement> lScheduledAnnouncements = new List<ScheduledAnnouncement>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchScheduledAnnouncement",
                                                                    id, gender, paidMember, hasPhotos,
                                                                    hasProfile, languageID, country, region, type))
                {

                    
                    while (reader.Read())
                    {
                        ScheduledAnnouncement scheduledAnnouncement = new ScheduledAnnouncement();

                        scheduledAnnouncement.id = (int) reader["ID"];
                        scheduledAnnouncement.name = (string) reader["Name"];
                        scheduledAnnouncement.subject = (string) reader["Subject"];
                        scheduledAnnouncement.body = (string) reader["Body"];
                        scheduledAnnouncement.gender = reader["Gender"] != DBNull.Value
                                                           ? (User.eGender?) ((int) reader["Gender"])
                                                           : null;
                        scheduledAnnouncement.paidMember = reader["PaidMember"] != DBNull.Value
                                                               ? (bool?) reader["PaidMember"]
                                                               : null;
                        scheduledAnnouncement.hasPhotos = reader["HasPhotos"] != DBNull.Value
                                                              ? (bool?) reader["HasPhotos"]
                                                              : null;
                        scheduledAnnouncement.hasProfile = reader["HasProfile"] != DBNull.Value
                                                               ? (bool?) reader["HasProfile"]
                                                               : null;
                        scheduledAnnouncement.languageID = reader["LanguageID"] != DBNull.Value
                                                               ? (int?) reader["LanguageID"]
                                                               : null;
                        scheduledAnnouncement.country = reader["Country"] != DBNull.Value
                                                            ? (string) reader["Country"]
                                                            : null;
                        scheduledAnnouncement.region = reader["Region"] != DBNull.Value
                                                           ? (string) reader["Region"]
                                                           : null;
                        scheduledAnnouncement.type = (eType) reader["Type"];
                        scheduledAnnouncement.date = reader["Date"] != DBNull.Value
                                                         ? (DateTime?) reader["Date"]
                                                         : null;
                        scheduledAnnouncement.days = reader["Days"] != DBNull.Value
                                                         ? (int?) reader["Days"]
                                                         : null;

                        lScheduledAnnouncements.Add(scheduledAnnouncement);
                    }
                    reader.Close();
                }
                return lScheduledAnnouncements.ToArray();
            }
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveScheduledAnnouncement",
                                                        id, name, subject, body, gender, paidMember, hasPhotos, hasProfile,
                                                        languageID, country, region, type, date, days);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteScheduledAnnouncement", id);
            }
        }

        #endregion
    }
}
