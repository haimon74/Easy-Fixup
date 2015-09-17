using System;
using System.Linq;
using ezFixUp.Model;

namespace ezFixUp.Classes
{
    public class Relationship
    {
        private int id;
        private string fromUsername = null;
        private string toUsername = null;
        private eRelationshipStatus type;
        private eRelationshipStatus? pendingType = null;
        private bool accepted = false;
        private DateTime timestamp = DateTime.Now;

        public enum eRelationshipStatus
        {
            Single,
            InRelationship,
            Engaged,
            Married,
            ItIsComplicated,
            InAnOpenRelationship
        }

        private Relationship(){}

        public Relationship(string fromUsername, string toUsername)
        {
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
        }

        public int ID
        {
            get { return id; }
        }

        public string FromUsername
        {
            get { return fromUsername; }
            set { fromUsername = value; }
        }

        public string ToUsername
        {
            get
            {
                return toUsername;
            }
            set 
            {
                toUsername = value;
            }
        }

        public eRelationshipStatus Type
        {
            get { return type; }
            set { type = value; }
        }

        public eRelationshipStatus? PendingType
        {
            get { return pendingType; }
            set { pendingType = value; }
        }

        public bool Accepted
        {
            get { return accepted; }
            set { accepted = value; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public static Relationship Fetch(string fromUsername, string toUsername)
        {
            Relationship[] relationships = Fetch(fromUsername, toUsername, null, null);
            return relationships.Length > 0 ? relationships[0] : null;
        }

        public static string[] FetchRequests(string toUsername)
        {
            using (var db = new ezFixUpDataContext())
            {
                var result = from r in db.Relationships
                             where (r.r_username == toUsername && (!r.r_accepted || r.r_pendingtype != null))
                             select r.u_username;
                return result.ToArray();
            }
        }

        private static Relationship[] Fetch(string fromUsername, string toUsername, eRelationshipStatus? type, bool? accepted)
        {
            using (var db = new ezFixUpDataContext())
            {
                var relationships = (from r in db.Relationships
                                    where (fromUsername == null || r.u_username == fromUsername)
                                            && (toUsername == null || r.r_username == toUsername)
                                            && (!type.HasValue || (eRelationshipStatus?) r.r_type == type)
                                            && (!accepted.HasValue || r.r_accepted == accepted)
                                    select new Relationship
                                               {
                                                   id = r.r_id,
                                                   fromUsername = r.u_username,
                                                   toUsername = r.r_username,
                                                   accepted = r.r_accepted,
                                                   timestamp = r.r_timestamp,
                                                   type = (eRelationshipStatus) r.r_type,
                                                   pendingType = (eRelationshipStatus) r.r_pendingtype
                                               });
                return relationships.ToArray();
            }
        }

        public void Save()
        {
            using (var db = new ezFixUpDataContext())
            {
                var relationship = new Model.Relationship()
                                        {
                                            r_id = id,
                                            u_username = fromUsername,
                                            r_username = toUsername,
                                            r_type = (int) type,
                                            r_pendingtype = pendingType.HasValue ? (int?) pendingType : null,
                                            r_accepted = accepted,
                                            r_timestamp = timestamp

                                        };
                if (id == 0)
                {
                    db.Relationships.InsertOnSubmit(relationship);
                }
                else
                {
                    db.Relationships.Attach(relationship, true);
                }

                db.SubmitChanges();

                if (id == 0) id = relationship.r_id;
            }
        }

        public static void Delete(string fromUsername, string toUsername, bool? accepted)
        {
            Delete(null, fromUsername, toUsername, accepted);
        }

        private static void Delete(int? id, string fromUsername, string toUsername, bool? accepted)
        {
            using (var db = new ezFixUpDataContext())
            {
                var relationship =
                    db.Relationships.Where(
                        r =>
                        (!id.HasValue || r.r_id == id)
                            && (fromUsername == null || r.u_username == fromUsername)
                            && (toUsername == null || r.r_username == toUsername)
                            && (!accepted.HasValue || r.r_accepted == accepted)).ToArray();
                db.Relationships.DeleteAllOnSubmit(relationship);
                db.SubmitChanges();
            }
        }

        public static string GetRelationshipStatusString(eRelationshipStatus type)
        {
            string result = String.Empty;
            switch (type)
            {
                case eRelationshipStatus.Engaged:
                    result = "Engaged".Translate();
                    break;
                case eRelationshipStatus.InAnOpenRelationship:
                    result = "In an open relationship".Translate();
                    break;
                case eRelationshipStatus.InRelationship:
                    result = "In a relationship".Translate();
                    break;
                case eRelationshipStatus.ItIsComplicated:
                    result = "It's complicated".Translate();
                    break;
                case eRelationshipStatus.Married:
                    result = "Married".Translate();
                    break;
                case eRelationshipStatus.Single:
                    result = "Single".Translate();
                    break;
            }

            return result;
        }
    }
}
