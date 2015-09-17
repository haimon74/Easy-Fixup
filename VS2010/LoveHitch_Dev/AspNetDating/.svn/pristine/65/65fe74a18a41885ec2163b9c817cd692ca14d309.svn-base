using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class EcardType
    {
        #region fields

        private int? id = null;
        private int? categoryId = null;
        private string name = String.Empty;
        private byte[] content = null;
        private string phrase = String.Empty;
        private eType type;
        private bool active = false;
        private eSortColumn sortColumn;

        public EcardType()
        {
        }

        public enum eType
        {
            Image = 1,
            Flash = 2,
            Wink = 4,
            Gift = 8
        }

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Name
        }

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public int? CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

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

        public string Phrase
        {
            get{return phrase;}
            set{phrase = value;}
        }

        public byte[] Content
        {
            get { return content ?? ((id > 0) ? (content = LoadContent(ID)) : null) ; }
            set { content = value; }
        }

        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        public static EcardType[] FetchWinks()
        {
            return Fetch(null, null, null, eType.Wink, null, eSortColumn.None);
        }

        public static EcardType[] FetchWinksForCategory(int? categoryId)
        {
            return Fetch(null, categoryId, null, eType.Wink, null, eSortColumn.None);
        }

        public static EcardType[] FetchWinks(bool active, int? categoryId)
        {
            return Fetch(null, categoryId, null, eType.Wink, active, eSortColumn.Name);
        }

        public static EcardType[] FetchGifts()
        {
            return Fetch(null, null, null, eType.Gift, null, eSortColumn.None);
        }

        public static EcardType[] FetchGiftsForCategory(int? categoryId)
        {
            return Fetch(null, categoryId, null, eType.Gift, null, eSortColumn.None);
        }

        public static EcardType[] FetchGifts(bool active, int? categoryId)
        {
            return Fetch(null, categoryId, null, eType.Gift, active, eSortColumn.Name);
        }

        public static EcardType[] FetchEcards()
        {
            EcardType[] flashes = Fetch(null, null, null, eType.Flash, null, eSortColumn.Name);
            EcardType[] images = Fetch(null, null, null, eType.Image, null, eSortColumn.Name);
            return (EcardType[])flashes.Add(images);
        }
        public static EcardType[] FetchEcardsForCategory(int? categoryId)
        {
            EcardType[] flashes = Fetch(null, categoryId, null, eType.Flash, null, eSortColumn.Name);
            EcardType[] images = Fetch(null, categoryId, null, eType.Image, null, eSortColumn.Name);
            return (EcardType[])flashes.Add(images);
        }
        public static EcardType[] FetchEcards(bool active, int? categoryId)
        {
            EcardType[] flashes = Fetch(null, categoryId, null, eType.Flash, active, eSortColumn.Name);
            EcardType[] images = Fetch(null, categoryId, null, eType.Image, active, eSortColumn.Name);
            return (EcardType[])flashes.Add(images);

        }

        //public static EcardType[] Fetch()
        //{
        //    return Fetch(null, null, null, null, eSortColumn.None);
        //}

        //public static EcardType[] Fetch(bool active)
        //{
        //    return Fetch(null, null, null, active, eSortColumn.Name);
        //}

        public static EcardType Fetch(int id)
        {
            EcardType[] ecards = Fetch(id, null, null, null, null, eSortColumn.None);

            if (ecards.Length > 0)
            {
                return ecards[0];
            }

            return null;
        }

        private static EcardType[] Fetch(int? id, int? categoryId, string name, eType? type, bool? active, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchEcardTypes",
                                                               id,
                                                               categoryId,
                                                               name,
                                                               (int?)type,
                                                               active,
                                                               sortColumn);
                var lEcardType = new List<EcardType>();

                while (reader.Read())
                {
                    try
                    {
                        var ecardType = new EcardType
                                            {
                                                id = (int) reader["ID"],
                                                categoryId = (int) reader["CategoryID"],
                                                name = (string) reader["Name"],
                                                phrase = (string) reader["Phrase"],
                                                type = (eType) reader["Type"],
                                                active = (bool) reader["Active"]
                                            };
                        lEcardType.Add(ecardType);
                    }
                    catch { }
                }
                return lEcardType.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveEcardType",
                                                        id,
                                                        categoryId,
                                                        name,
                                                        phrase,
                                                        Content,
                                                        type,
                                                        active);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteEcardType", id);
            }
        }

        public static byte[] LoadContent(int ecardTypeID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchEcardTypeContent", ecardTypeID);

                if (reader.Read())
                {
                    byte[] buffer = (byte[])reader["Content"];

                    return buffer;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void SaveContent(int ecardTypeID, byte[] content)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SaveEcardTypeContent", ecardTypeID, content);
            }
        }

        #endregion
    }
}
