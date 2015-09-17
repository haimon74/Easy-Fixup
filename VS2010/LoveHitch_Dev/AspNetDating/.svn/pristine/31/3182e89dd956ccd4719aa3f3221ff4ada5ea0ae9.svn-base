/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using AspNetDating.Classes.LoveHitchParallel;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class AbuseReport
    {
        public enum ReportType
        {
            //Do not change the order of the values below, it could lead to data loss!!!
            //The maintenance stored procedure presumes that the ReportedType.Photo == 1
            Profile,
            Photo,
            Message
        }
        
        #region fields

        private int? id;
        private string reportedBy;
        private string reportedUser;
        private ReportType type;
        private string report;
        private int? targetId;
        private bool reviewed;
        private DateTime dateReported = DateTime.Now;

        #endregion

        #region Constructors

        private AbuseReport()
        {}

        public AbuseReport(string reportedBy, string reportedUser, ReportType type)
        {
            this.reportedBy = reportedBy;
            this.reportedUser = reportedUser;
            this.type = type;
        }
        
        #endregion

        #region Properties

        public int ID
        {
            get 
            {
                if (id.HasValue)
                    return id.Value;
                else throw new Exception("ID is not set");
            }
        }
        
        public string ReportedBy
        {
            get { return reportedBy; }
            set { reportedBy = value; }
        }

        public string ReportedUser
        {
            get { return reportedUser; }
            set { reportedUser = value; }
        }

        public ReportType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Report
        {
            get { return report; }
            set { report = value; }
        }
        
        public int? TargetID
        {
            get { return targetId; }
            set { targetId = value;}
        }
        
        public bool Reviewed
        {
            get { return reviewed; }
            set { reviewed = value; }
        }

        public DateTime DateReported
        {
            get { return dateReported; }
            set { dateReported = value; }
        }        
        #endregion

        #region Methods

        public static int[] Search(string reportedBy, string reportedUser, ReportType? type, int? reportedId,
                                   bool? reviewed, DateTime? dateReportedFrom,
                                   DateTime? dateReportedTo, string sortColumn, bool sortAsc
            )
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "SearchAbuseReports",
                                            reportedBy,
                                            reportedUser,
                                            type,
                                            reportedId,
                                            reviewed,
                                            dateReportedFrom,
                                            dateReportedTo,
                                            sortColumn);

                List<int> lReportIds = new List<int>();

                while (reader.Read())
                {
                    //AbuseReport report = new AbuseReport();

                    int id = (int)reader["ID"];
                    //report.id = (int)reader["ID"];
                    //report.reportedBy = (string)reader["ReportedBy"];
                    //report.reportedUser = (string)reader["ReportedUser"];
                    //report.type = (ReportType)(int)reader["Type"];
                    //report.report = (string)reader["Report"];
                    //report.reviewed = (bool)reader["Reviewed"];
                    //report.targetId = reader["TargetId"] as int?;
                    //report.dateReported = (DateTime)reader["DateReported"];
                    
                    lReportIds.Add(id/*report*/);
                }

                if (!sortAsc) lReportIds.Reverse();

                return lReportIds.ToArray();
            }
        }

        /// <summary>
        /// Fetches an abuse report by specified id from DB. If the it doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static AbuseReport Fetch(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "LoadAbuseReport", id);

                AbuseReport report = null;

                if (reader.Read())
                {
                    report = new AbuseReport();

                    report.id = (int)reader["ID"];
                    report.reportedBy = (string)reader["ReportedBy"];
                    report.reportedUser = (string)reader["ReportedUser"];
                    report.type = (ReportType)(int)reader["Type"];
                    report.report = (string)reader["Report"];
                    report.reviewed = (bool)reader["Reviewed"];
                    report.targetId = reader["TargetId"] as int?;
                    report.dateReported = (DateTime)reader["DateReported"];
                }

                return report;
            }
        }

        /// <summary>
        /// Saves this instance in DB.
        /// If id for this instance is NULL it inserts new record in DB otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveAbuseReport", id,
                                                        reportedBy, reportedUser, type, report, reviewed,targetId, dateReported);
                
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        #endregion
    }

    [Serializable]
    public class AbuseReportSearchResults : SearchResults<int, AbuseReport>
    {
        public int[] AbuseReports
        {
            get
            {
                if (Results == null)
                    return new int[0];
                return Results;
            }
            set { Results = value; }
        }

        public new int GetTotalPages(int reportsPerPage)
        {
            return base.GetTotalPages(reportsPerPage);
        }

        protected override AbuseReport LoadResult(int id)
        {
            return AbuseReport.Fetch(id);
        }
        protected override AbuseReport[] LoadResults(int[] ids)
        {
            List<int> keysList = new List<int>();// ids.ToList();
            foreach (var id in ids)
            {
                keysList.Add(id);
            }
            List<AbuseReport> resultList = new List<AbuseReport>();
            if (keysList.Count > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<int, AbuseReport>(
                                                    keysList, resultList,
                                                    (new LoadAbuseReportsResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="reportsPerPage">reportsPerPage</param>
        /// <returns>Array of photos</returns>
        public new AbuseReport[] GetPage(int Page, int reportsPerPage)
        {
            return base.GetPage(Page, reportsPerPage);
        }
    }    
}
