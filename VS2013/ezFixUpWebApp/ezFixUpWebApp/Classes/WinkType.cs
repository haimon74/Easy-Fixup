using System;
using System.Collections.Generic;

namespace ezFixUp.Classes
{
    public class WinkType
    {
        #region fields

        private int? id = null;
        private int? categoryId = null;
        private string name = String.Empty;
        private string phrase = String.Empty;
        private bool active = false;
        private eSortColumn sortColumn;

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
        public WinkType()
        {
        }
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

        public static WinkType[] Fetch()
        {
            return Fetch(null, null, null, null, eSortColumn.None);
        }

        public static WinkType[] FetchWinksForCategory(int? categoryId)
        {
            return Fetch(null, categoryId, null, null, eSortColumn.None);
        }

        public static WinkType[] Fetch(bool active, int? categoryId)
        {
            return Fetch(null, categoryId, null, active, eSortColumn.Name);
        }

        public static WinkType Fetch(int id)
        {
            WinkType[] winks = Fetch(id, null, null, null, eSortColumn.None);

            if (winks.Length > 0)
            {
                return winks[0];
            }

            return null;
        }

        private static WinkType[] Fetch(int? id, int? categoryId, string name, bool? active, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = SqlHelper.GetDB().ExecuteReader( "FetchWinkTypes",
                                                               id,
                                                               categoryId,
                                                               name,
                                                               active,
                                                               sortColumn);
                var lstWinkType = new List<WinkType>();

                while (reader.Read())
                {
                    try
                    {
                        var winkType = new WinkType
                                            {
                                                id = (int) reader["ID"],
                                                categoryId = (int) reader["CategoryID"],
                                                name = (string) reader["Name"],
                                                phrase = (string) reader["Phrase"],
                                                active = (bool) reader["Active"]
                                            };
                        lstWinkType.Add(winkType);
                    }
                    catch { }
                }
                return lstWinkType.ToArray();
            }
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveWinkType",
                                                        id,
                                                        categoryId,
                                                        name,
                                                        phrase,
                                                        active);
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
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteWinkType", id);
            }
        }

        #endregion
    }
}
