using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class AdsCategory
    {
        #region fields

        private int? id = null;
        private int? parentID = null;
        private string title;
        private eSortColumn sortColumn;

        public enum eSortColumn
        {
            None,
            Title
        }

        #endregion

        #region Constructors

        public AdsCategory(){}

        public AdsCategory(int parentID)
        {
            this.parentID = parentID;
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

        public int? ParentID
        {
            get { return parentID; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        public static AdsCategory[] Fetch()
        {
            return Fetch(null, null, null, eSortColumn.None);
        }

        public static AdsCategory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }

        public static AdsCategory[] FetchCategories(eSortColumn sortColumn)
        {
            List<AdsCategory> lAdsCategories = new List<AdsCategory>();
            AdsCategory[] categories = Fetch(sortColumn);

            foreach (var category in categories)
            {
                if (category.parentID != null) continue;
                lAdsCategories.Add(category);
            }

            return lAdsCategories.ToArray();
        }

        public static AdsCategory[] FetchSubcategories(int categoryID, eSortColumn sortColumn)
        {
            return Fetch(null, categoryID, null, sortColumn);
        }

        public static AdsCategory Fetch(int id)
        {
            AdsCategory[] categories = Fetch(id, null, null, eSortColumn.None);

            if (categories.Length > 0)
            {
                return categories[0];
            }
            else
            {
                return null;
            }
        }

        private static AdsCategory[] Fetch(int? id, int? parentID, string title, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchAdsCategories", id, parentID, title, sortColumn);

                List<AdsCategory> categories = new List<AdsCategory>();

                while (reader.Read())
                {
                    AdsCategory category = new AdsCategory();

                    category.id = (int)reader["ID"];
                    category.parentID = reader["ParentID"] != DBNull.Value ? (int?) reader["ParentID"] : null;
                    category.title = (string)reader["Title"];

                    categories.Add(category);
                }

                return categories.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveAdsCategory", id, parentID, title);

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
                SqlHelper.ExecuteNonQuery(conn, "DeleteAdsCategory", id);
            }
        }

        #endregion
    }
}
