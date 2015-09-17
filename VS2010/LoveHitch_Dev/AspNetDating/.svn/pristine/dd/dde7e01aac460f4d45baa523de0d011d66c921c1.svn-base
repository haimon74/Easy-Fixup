using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class CreditsPackage
    {
        #region fields

        private int? id = null;
        private string name;
        private int quantity = 0;
        private decimal price = 0;
        private eSortColumn sortColumn;

        /// <summary>
        /// Specify on which column affiliates will be sorted.
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// No sort.
            /// </summary>
            None,
            Price
        }

        #endregion

        #region Constructors

        public CreditsPackage()
        {}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
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
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all credits packages from DB.
        /// If there are no credits packages in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static CreditsPackage[] Fetch()
        {
            return Fetch(null, null, null, null, eSortColumn.None);
        }

        public static CreditsPackage[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches credits packages by specified id from DB.
        /// If the credits packages doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static CreditsPackage Fetch(int id)
        {
            CreditsPackage[] creditsPackages = Fetch(id, null, null, null, eSortColumn.None);

            if (creditsPackages.Length > 0)
            {
                return creditsPackages[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches credits packages by specified arguments.
        /// It returns an empty array if there are no credits packages in DB by specified arguments.
        /// If these arguments are null it returns all credits packages from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static CreditsPackage[] Fetch(int? id, string name, int? quantity, decimal? price, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchCreditsPackages", id, name, quantity, price, sortColumn);

                List <CreditsPackage> lCreditsPackage = new List<CreditsPackage>();

                while (reader.Read())
                {
                    CreditsPackage creditsPackage = new CreditsPackage();

                    creditsPackage.id = (int) reader["ID"];
                    creditsPackage.name = (string) reader["Name"];
                    creditsPackage.quantity = (int) reader["Quantity"];
                    creditsPackage.price = (decimal) reader["Price"];

                    lCreditsPackage.Add(creditsPackage);
                }

                return lCreditsPackage.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the field id is null it inserts new record in DB,
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveCreditsPackage", id, name, quantity, price);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes credits packages from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteCreditsPackage", id);
            }
        }

        #endregion
    }
}
