using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class GiftType
    {
                #region fields

        private int? id = null;
        private int? categoryId = null;
        private int? price = null;
        private string name = String.Empty;
        private byte[] content = null;
        private string phrase = String.Empty;
        private bool active = false;
        private eType giftType;
        private eSortColumn sortColumn;

        public GiftType()
        {
        }

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Name,
            Price
        }

        public enum eType
        {
            Image = 1,
            Flash = 2
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
        public int? Price
        {
            get { return price; }
            set { price = value; }
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

        public static GiftType[] Fetch()
        {
            return Fetch(null, null, null, null, null, eSortColumn.None);
        }

        public static GiftType[] FetchGiftsForCategory(int? categoryId)
        {
            return Fetch(null, categoryId, null, null, null, eSortColumn.None);
        }

        public static GiftType[] Fetch(bool active, int? categoryId)
        {
            return Fetch(null, categoryId, null, null, active, eSortColumn.Name);
        }

        public static GiftType[] Fetch(bool active, int? categoryId, eSortColumn sortColumn)
        {
            return Fetch(null, categoryId, null, null, active, sortColumn);
        }

        public static GiftType Fetch(int id)
        {
            GiftType[] gifts = Fetch(id, null, null, null, null, eSortColumn.None);

            if (gifts.Length > 0)
            {
                return gifts[0];
            }

            return null;
        }

        private static GiftType[] Fetch(int? id, int? categoryId, int? price, string name, bool? active, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchGiftTypes",
                                                               id,
                                                               categoryId,
                                                               price,
                                                               name,
                                                               (int)eType.Image,
                                                               active,
                                                               sortColumn);
                var lstGiftType = new List<GiftType>();

                while (reader.Read())
                {
                    try
                    {
                        var giftType = new GiftType
                                            {
                                                id = (int) reader["ID"],
                                                categoryId = (int) reader["CategoryID"],
                                                price = (int) reader["Price"],
                                                name = (string) reader["Name"],
                                                phrase = (string) reader["Phrase"],
                                                active = (bool) reader["Active"],
                                                giftType=(eType)reader["ItsType"]
                                            };
                        lstGiftType.Add(giftType);
                    }
                    catch { }
                }
                return lstGiftType.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveGiftType",
                                                        id,
                                                        CategoryId,
                                                        Name,
                                                        Phrase,
                                                        Content,
                                                        (int)eType.Image,
                                                        Price,
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
                SqlHelper.ExecuteNonQuery(conn, "DeleteGiftType", id);
            }
        }

        public static byte[] LoadContent(int giftTypeID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchGiftTypeContent", giftTypeID);

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

        public static void SaveContent(int giftTypeID, byte[] content)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SaveGiftTypeContent", giftTypeID, content);
            }
        }

        #endregion
    }
}
