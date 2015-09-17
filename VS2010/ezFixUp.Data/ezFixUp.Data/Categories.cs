/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    [Serializable]
    public class Category
    {
        #region fields

        private int? id = null;
        private string name;
        private int order;
        private bool usersCanCreateGroups;

        /// <summary>
        /// Specifies the action which will be perform.
        /// </summary>
        private enum eAction
        {
            Assign = 1,
            Remove = 2
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

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public bool UsersCanCreateGroups
        {
            get { return usersCanCreateGroups; }
            set { usersCanCreateGroups = value; }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Fetches all categories from DB.
        /// If there are no categories in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static Category[] Fetch()
        {
            return Fetch(null, null, null);
        }

        /// <summary>
        /// Fetches category by the specified id from DB.
        /// If the category doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>If the category doesn't exist returns NULL.</returns>
        public static Category Fetch(int id)
        {
            Category[] categories = Fetch(id, null, null);

            if (categories.Length > 0)
            {
                return categories[0];    
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches categories by specified id, name or order.
        /// It returns an empty array if there are no categories in DB by specified arguments.
        /// If these arguments are null it returns all categories from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="order">The order.</param>
        /// <returns>An array with categories or an empty array if no categories are found in DB.</returns>
        private static Category[] Fetch(int? id, string name, int? order)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchCategories", id, name, order);

                List<Category> categories = new List<Category>();
                
                while (reader.Read())
                {
                    Category category = new Category();

                    category.id = (int) reader["ID"];
                    category.name = (string) reader["Name"];
                    category.order = (int) reader["Order"];
                    category.usersCanCreateGroups = (bool)reader["UsersCanCreateGroups"];

                    categories.Add(category);
                }

                return categories.ToArray();
            }
        }

        /// <summary>
        /// Fetches all categories from DB by group ID.
        /// If there are no categories in DB for specified group ID it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns>Category array or an emptry array if there are no categories in DB for specified group ID.</returns>
        public static Category[] FetchCategoriesByGroup(int groupID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchCategoriesByGroup", groupID);

                List<Category> categories = new List<Category>();

                while (reader.Read())
                {
                    Category category = new Category();

                    category.id = (int)reader["ID"];
                    category.name = (string)reader["Name"];
                    category.order = (int)reader["Order"];
                    category.usersCanCreateGroups = (bool)reader["UsersCanCreateGroups"];

                    categories.Add(category);
                }

                return categories.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance. If the ID of this instance is NULL it inserts new record in DB
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveCategory", id, name, usersCanCreateGroups);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes category by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteCategory", id);
            }
        }

        /// <summary>
        /// Sets the groups for this instance.
        /// </summary>
        /// <param name="groupIDs">The group I ds.</param>
        public void SetGroups(int[] groupIDs)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteCategoryFromGroups", id);

                foreach (int groupID in groupIDs)
                {
                    SqlHelper.GetDB().ExecuteNonQuery( "AssignRemoveGroupToFromCategory", id, groupID, eAction.Assign);
                }
            }
        }

        public static void ChangeOrder(int id, eDirections direction)
        {
            string direction_ = "";
            switch (direction)
            {
                case eDirections.Up:
                    direction_ = "up";
                    break;
                case eDirections.Down:
                    direction_ = "down";
                    break;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "ChangeCategoryOrder", id, direction_);
            }

//            if (HttpContext.Current != null)
//            {
//                string cacheKey = "Category_Fetch";
//                if (HttpContext.Current.Cache[cacheKey] != null)
//                    HttpContext.Current.Cache.Remove(cacheKey);
//                if (id > 0)
//                {
//                    cacheKey = String.Format("Category_Fetch_{0}", id);
//                    if (HttpContext.Current.Cache[cacheKey] != null)
//                        HttpContext.Current.Cache.Remove(cacheKey);
//                }
//            }
        }

        #endregion
    }
}
