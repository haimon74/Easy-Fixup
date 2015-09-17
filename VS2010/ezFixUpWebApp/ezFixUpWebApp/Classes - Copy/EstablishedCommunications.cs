using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    public class EstablishedCommunication
    {
        #region fields

        private string fromUsername;
        private string toUsername;
        private DateTime date = DateTime.Now;

        #endregion

        #region Constructors

        public EstablishedCommunication()
        {}

        public EstablishedCommunication(string fromUsername, string toUsername)
        {
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets from username.
        /// </summary>
        /// <value>From username.</value>
        public string FromUsername
        {
            get { return fromUsername; }
        }


        /// <summary>
        /// Gets or sets to username.
        /// </summary>
        /// <value>To username.</value>
        public string ToUsername
        {
            get { return toUsername; }
        }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Fetches all established communications from DB.
        /// If there are no established communication in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static EstablishedCommunication[] Fetch()
        {
            return Fetch(null, null, null);
        }


        /// <summary>
        /// Fetches established communication by specified from and to usernames.
        /// If the established communication doesn't exist returns NULL.
        /// </summary>
        /// <returns></returns>
        public static EstablishedCommunication Fetch(string fromUsername, string toUsername)
        {
            EstablishedCommunication[] establishedCommunications = Fetch(fromUsername, toUsername, null);

            if (establishedCommunications.Length > 0)
            {
                return establishedCommunications[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Fetches established communications by specified arguments.
        /// It returns an empty array if there are no established communication in DB by specified arguments.
        /// If these arguments are null it returns all established communication from DB.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static EstablishedCommunication[] Fetch(string fromUsername, string toUsername, DateTime? date)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader( "FetchEstablishedCommunications", fromUsername, toUsername, date);

                List <EstablishedCommunication> lEstablishedCommunications = new List<EstablishedCommunication>();

                while (reader.Read())
                {
                    EstablishedCommunication establishedCommunication = new EstablishedCommunication();

                    establishedCommunication.fromUsername= (string) reader["FromUsername"];
                    establishedCommunication.toUsername = (string) reader["ToUsername"];
                    establishedCommunication.date = (DateTime) reader["Date"];

                    lEstablishedCommunications.Add(establishedCommunication);
                }

                return lEstablishedCommunications.ToArray();
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
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveEstablishedCommunication", fromUsername, toUsername, date);
            }
        }

        /// <summary>
        /// Deletes established communication from DB by specified id.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        public static void Delete(string fromUsername, string toUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteEstablishedCommunication", fromUsername, toUsername);
            }
        }

        #endregion
    }
}
