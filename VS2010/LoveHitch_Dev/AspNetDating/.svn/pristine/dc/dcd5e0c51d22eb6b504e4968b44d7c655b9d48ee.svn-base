using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class EcardCategory
    {
        #region fields

        protected int? _id = null;
        protected int? _parentID = null;
        protected string _title;
        protected bool _isActive;
        protected eSortColumn _sortColumn;
        protected eEcardCategoryType? _ecardCategoryType;
        protected string SaveProcedureNameFormat = "Save{0}Category";
        protected string SaveProcedureName;
        
        public static string FetchProcedureName = "FetchEcardCategories";
        public static string DeleteProcedureName = "DeleteEcardCategory";

        public enum eSortColumn
        {
            None,
            Title
        }
        public enum eEcardCategoryType
        {
            Wink,
            Gift,
            Ecard
        }
        #endregion

        #region Constructors

        protected string GetSaveStoreProceduresName()
        {
            return String.Format(SaveProcedureNameFormat, EcardCategoryType);
        }
        public EcardCategory()
        {
        }
        public EcardCategory(int parentID)
        {
            this._parentID = parentID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                if (_id.HasValue)
                {
                    return _id.Value;
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        public int? ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public eSortColumn SortColumn
        {
            get { return _sortColumn; }
            set { _sortColumn = value; }
        }

        public eEcardCategoryType EcardCategoryType
        {
            get { return _ecardCategoryType ?? eEcardCategoryType.Ecard; }
            set { _ecardCategoryType = value; }
        }

        #endregion

        #region Methods

        public static EcardCategory[] Fetch()
        {
            return Fetch(null, null, null, eSortColumn.None);
        }
        public static EcardCategory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }
        public static EcardCategory[] FetchCategories(eSortColumn sortColumn)
        {
            EcardCategory[] categories = Fetch(sortColumn);
            return categories.Where(category => category._parentID == null)
                             .Select(category => category).ToArray();
        }
        public static EcardCategory[] FetchSubcategories(int categoryID, eSortColumn sortColumn)
        {
            return Fetch(null, categoryID, null, sortColumn);
        }
        public static EcardCategory Fetch(int id)
        {
            EcardCategory[] categories = Fetch(id, null, null, eSortColumn.None);
            return (categories.Length > 0) ? categories[0] : null;
        }
        private static EcardCategory[] Fetch(int? id, int? parentID, string title, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, FetchProcedureName, id, parentID, title, sortColumn);
                var categories = new List<EcardCategory>();

                while (reader.Read())
                {
                    var category = new EcardCategory
                                       {
                                           _id = (int) reader["ID"],
                                           //_parentID = reader["ParentID"] != DBNull.Value 
                                           //             ? (int?) reader["ParentID"] : null,
                                           _title = (string) reader["Title"]
                                       };
                    categories.Add(category);
                }
                return categories.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, GetSaveStoreProceduresName(), _id, _parentID, _isActive, _title);

                if (_id == null)
                {
                    _id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            Delete(id, DeleteProcedureName);
        }
        public static void Delete(int id, string procedureName)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, procedureName, id);
            }
        }

        #endregion
    }

    public class WinkCategory : EcardCategory
    {
        public static string FetchProcedureName = "FetchWinkCategories";
        public static string DeleteProcedureName = "DeleteWinkCategory";
        
        public WinkCategory()
        {
            this.EcardCategoryType = eEcardCategoryType.Wink;
        }
        public WinkCategory(int parentID)
        {
            this.EcardCategoryType = eEcardCategoryType.Wink;
            ParentID = parentID;
        }
        new public static void Delete(int id)
        {
            EcardCategory.Delete(id, DeleteProcedureName);
        }
        public static WinkCategory Fetch(int id)
        {
            WinkCategory[] categories = Fetch(id, null, null, eSortColumn.None);
            return (categories.Length > 0) ? categories[0] : null;
        }
        public static WinkCategory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }
        public static WinkCategory[] FetchCategoriesByLaguageId(int languageId, eSortColumn sortColumn)
        {
            WinkCategory[] categories = Fetch(sortColumn);
            return categories.Where(category => category._parentID == languageId)
                             .Select(category => category).ToArray();
        }
        private static WinkCategory[] Fetch(int? id, int? parentID, string title, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, FetchProcedureName, id, parentID, title, sortColumn);
                var categories = new List<WinkCategory>();

                while (reader.Read())
                {
                    var category = new WinkCategory
                    {
                        _id = (int)reader["ID"],
                        _parentID = (int?)reader["LanguageID"],
                        _title = (string)reader["Title"]
                    };
                    categories.Add(category);
                }
                return categories.ToArray();
            }
        }
    }
    public class GiftCategory : EcardCategory
    {
        public static string FetchProcedureName = "FetchGiftCategories";
        public static string DeleteProcedureName = "DeleteGiftCategory";
        
        public GiftCategory()
        {
            this.EcardCategoryType = eEcardCategoryType.Gift;
        }
        public GiftCategory(int parentID)
        {
            this.EcardCategoryType = eEcardCategoryType.Gift;
            ParentID = parentID;
        }
        new public static void Delete(int id)
        {
            EcardCategory.Delete(id, DeleteProcedureName);
        }
        public static GiftCategory Fetch(int id)
        {
            GiftCategory[] categories = Fetch(id, null, null, eSortColumn.None);
            return (categories.Length > 0) ? categories[0] : null;
        }
        public static GiftCategory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }
        public static GiftCategory[] FetchCategories(eSortColumn sortColumn)
        {
            GiftCategory[] categories = Fetch(sortColumn);
            return categories.Where(category => category._parentID == null /*&& category.IsActive*/)
                             .Select(category => category).ToArray();
        }
        private static GiftCategory[] Fetch(int? id, int? parentID, string title, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, FetchProcedureName, id, parentID, title, sortColumn);
                var categories = new List<GiftCategory>();

                while (reader.Read())
                {
                    var category = new GiftCategory
                    {
                        _id = (int)reader["ID"],
                        //_parentID = reader["ParentID"] != DBNull.Value 
                        //             ? (int?) reader["ParentID"] : null,
                        _title = (string)reader["Title"]
                    };
                    categories.Add(category);
                }
                return categories.ToArray();
            }
        }
    }
}
