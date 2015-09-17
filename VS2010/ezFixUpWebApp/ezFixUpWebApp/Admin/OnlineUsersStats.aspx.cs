using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Caching;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class OnlineUsersStats : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseOnlineUsersStats;

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            btnShowStatistics.Text = Lang.TransA("Show Statistics");
            btnGetCSV.Text = Lang.TransA("Download as CSV");

            ddType.Items.Add(new ListItem(Lang.TransA("Hourly"), ((int) HourlyStats.eType.Hourly).ToString()));
            ddType.Items.Add(new ListItem(Lang.TransA("Daily"), ((int) HourlyStats.eType.Daily).ToString()));
            ddType.Items.Add(new ListItem(Lang.TransA("Weekly"), ((int) HourlyStats.eType.Weekly).ToString()));
            ddType.Items.Add(new ListItem(Lang.TransA("Monthly"), ((int) HourlyStats.eType.Monthly).ToString()));

            ddType.SelectedValue = ((int) HourlyStats.eType.Daily).ToString();

            DateTime to = DateTime.Now.Date;
            DateTime from = to - TimeSpan.FromDays(30);
            txtFrom.Text = from.ToShortDateString();
            txtTo.Text = to.ToShortDateString();

            pnlOnlineUsersChart.Visible = false;
        }

        private void showStatistics(DateTime from, DateTime to)
        {
            int max = 10;

            if (getSelectedTypeInterval() == HourlyStats.eType.Monthly)
            {
                if (from == to)
                {
                    from = new DateTime(from.Year, from.Month, 1);
                    to = new DateTime(to.Year, to.Month, 1).AddMonths(1);
                }
                else
                {
                    from = new DateTime(from.Year, from.Month, 1);
                    to = new DateTime(to.Year, to.Month, 1);
                }
            }
            else if (from == to)
            {
                to = to.AddDays(1);
            }

            ChartOnlineUsers.Series["Default"].ToolTip = String.Format("#VALX\n#VALY {0}",
                                                                            "Online users".TranslateA());
            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();

            DataTable dtStats = new DataTable("OnlineUsersStats");

            dtStats.Columns.Add("Time");
            dtStats.Columns.Add("OnlineUsers");

            Hashtable htHourlyStats = HourlyStats.FetchHourlyStatsForOnlineUsers(from, to, getSelectedTypeInterval());

            if (htHourlyStats.Count > 0)
            {
                if (getSelectedTypeInterval() != HourlyStats.eType.Weekly)
                {
                    while (from.Date < to.Date)
                    {
                        string time;
                        int newUsers = 0;

                        if (htHourlyStats.ContainsKey(from))
                        {
                            newUsers = (int) htHourlyStats[from];
                        }

                        if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
                            time = from.ToString("g");
                        else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
                            time = from.ToShortDateString();
                        else time = from.ToString("MMM yyyy");

                        dtStats.Rows.Add(new object[]
                                             {
                                                 time,
                                                 newUsers
                                             });

                        xValues.Add(time);
                        yValues.Add(newUsers);
                        if (max < newUsers) max = newUsers;

                        if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
                            from = from.AddHours(1);
                        else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
                            from = from.AddDays(1);
                        else
                            from = from.AddMonths(1);
                    }
                }
                else if (getSelectedTypeInterval() == HourlyStats.eType.Weekly)
                {
                    while (from.Date < to.Date)
                    {
                        int newUsers = 0;
                        bool hasKey = false;

                        // from current date to 7 days after check is the hashtable has the day
                        for (int i = 0; i < 7; i++)
                        {
                            DateTime key = from.AddDays(i);
                            if (htHourlyStats.ContainsKey(key))
                            {
                                hasKey = true;
                                newUsers = (int) htHourlyStats[key];

                                dtStats.Rows.Add(new object[]
                                                     {
                                                         from.ToShortDateString(),
                                                         newUsers
                                                     });

                                xValues.Add(from.ToShortDateString());
                                yValues.Add(newUsers);
                                if (max < newUsers) max = newUsers;
                                break;
                            }
                        }

                        if (hasKey)
                        {
                            from = from.AddDays(7);
                            continue;
                        }

                        dtStats.Rows.Add(new object[]
                                             {
                                                 from.ToShortDateString(),
                                                 newUsers
                                             });

                        xValues.Add(from.ToShortDateString());
                        yValues.Add(newUsers);
                        if (max < newUsers) max = newUsers;

                        from = from.AddDays(7);
                    }
                }

                bool isVisibleChart = false;

                foreach (int value in yValues)
                {
                    if (value != 0)
                    {
                        isVisibleChart = true;
                        break;
                    }
                }

                ChartOnlineUsers.Series["Default"].Points.DataBindXY(xValues, yValues);

                pnlOnlineUsersChart.Visible = isVisibleChart;
                dgOnlineUsers.Visible = true;
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no online users for the specified period."),
                                Misc.MessageType.Error);
                pnlOnlineUsersChart.Visible = false;
                dgOnlineUsers.Visible = false;
            }

            dgOnlineUsers.DataSource = dtStats;
            dgOnlineUsers.DataBind();
            btnGetCSV.Visible = dgOnlineUsers.Visible;
        }

        private void setDataGridHeaderAndChartLabels()
        {
            if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
            {
                dgOnlineUsers.Columns[0].HeaderText = Lang.TransA("Hour");
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
            {
                dgOnlineUsers.Columns[0].HeaderText = Lang.TransA("Day");
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Weekly)
            {
                dgOnlineUsers.Columns[0].HeaderText = Lang.TransA("Week");
            }
            else
            {
                dgOnlineUsers.Columns[0].HeaderText = Lang.TransA("Month");
            }

            dgOnlineUsers.Columns[1].HeaderText = Lang.TransA("Online Users");
            ChartOnlineUsers.Titles["Title1"].Text = Lang.TransA("Online Users");
        }

        private bool isValidInterval(string fromDateTime, string toDateTime)
        {
            bool result = true;
            DateTime from;
            DateTime to;

            try
            {
                from = DateTime.Parse(fromDateTime);
            }
            catch (FormatException)
            {
                MessageBox.Show(Lang.TransA("Please enter valid from date!"), Misc.MessageType.Error);
                return false;
            }

            try
            {
                to = DateTime.Parse(toDateTime);
            }
            catch (FormatException)
            {
                MessageBox.Show(Lang.TransA("Please enter valid to date!"), Misc.MessageType.Error);
                return false;
            }

            if (from > to)
            {
                MessageBox.Show(Lang.TransA("The specified interval is not valid!"), Misc.MessageType.Error);
                result = false;
            }

            if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
            {
                if (to - from > TimeSpan.FromDays(7))
                {
                    MessageBox.Show(Lang.TransA("Selected range should be maximum 7 days!"), Misc.MessageType.Error);
                    result = false;
                }
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
            {
                if (to - from > TimeSpan.FromDays(90))
                {
                    MessageBox.Show(Lang.TransA("Selected range should be maximum 90 days!"), Misc.MessageType.Error);
                    result = false;
                }
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Weekly)
            {
                if (to - from > TimeSpan.FromDays(180))
                {
                    MessageBox.Show(Lang.TransA("Selected range should be maximum 180 days!"), Misc.MessageType.Error);
                    result = false;
                }
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Monthly)
            {
                if (to - from > TimeSpan.FromDays(365))
                {
                    MessageBox.Show(Lang.TransA("Selected range should be maximum 365 days!"), Misc.MessageType.Error);
                    result = false;
                }
            }

            return result;
        }

        private HourlyStats.eType getSelectedTypeInterval()
        {
            if (ddType.SelectedValue == ((int) HourlyStats.eType.Hourly).ToString())
            {
                return HourlyStats.eType.Hourly;
            }
            else if (ddType.SelectedValue == ((int) HourlyStats.eType.Daily).ToString())
            {
                return HourlyStats.eType.Daily;
            }
            else if (ddType.SelectedValue == ((int) HourlyStats.eType.Weekly).ToString())
            {
                return HourlyStats.eType.Weekly;
            }
            else
            {
                return HourlyStats.eType.Monthly;
            }
        }

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime to = DateTime.Now.Date;
            DateTime from;

            if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
            {
                from = to - TimeSpan.FromDays(1);
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
            {
                from = to - TimeSpan.FromDays(30);
            }
            else if (getSelectedTypeInterval() == HourlyStats.eType.Weekly)
            {
                from = to - TimeSpan.FromDays(168);
            }
            else // monthly
            {
                from = to - TimeSpan.FromDays(365);
            }

            txtFrom.Text = from.ToShortDateString();
            txtTo.Text = to.ToShortDateString();

            pnlOnlineUsersChart.Visible = false;
            dgOnlineUsers.Visible = false;
            btnGetCSV.Visible = false;
        }

        protected void btnShowStatistics_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();

            if (isValidInterval(from, to))
            {
                setDataGridHeaderAndChartLabels();
                showStatistics(DateTime.Parse(from), DateTime.Parse(to));
            }
            else
            {
                pnlOnlineUsersChart.Visible = false;
                dgOnlineUsers.Visible = false;
                btnGetCSV.Visible = false;
            }
        }

        protected void dgOnlineUsers_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
        }

        protected void btnGetCSV_Click(object sender, EventArgs e)
        {
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();
            DataTable dtStats = new DataTable();

            if (isValidInterval(from, to))
            {
                dtStats = getDataTableResults(DateTime.Parse(from), DateTime.Parse(to));
            }

            if (dtStats.Rows.Count != 0)
            {
                Response.Clear();
                Response.ContentType = "application/text";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = Encoding.UTF8.EncodingName;
                Response.AppendHeader("content-disposition", "attachment; filename=results.csv");
                Response.Write(Parsers.ParseCSV(dtStats));
                Response.End();
            }
        }

        private DataTable getDataTableResults(DateTime from, DateTime to)
        {
            if (getSelectedTypeInterval() == HourlyStats.eType.Monthly)
            {
                if (from == to)
                {
                    from = new DateTime(from.Year, from.Month, 1);
                    to = new DateTime(to.Year, to.Month, 1).AddMonths(1);
                }
                else
                {
                    from = new DateTime(from.Year, from.Month, 1);
                    to = new DateTime(to.Year, to.Month, 1);
                }
            }
            else if (from == to)
            {
                to = to.AddDays(1);
            }

            DataTable dtStats = new DataTable("OnlineUsersStats");

            dtStats.Columns.Add("Time");
            dtStats.Columns.Add("OnlineUsers");

            Hashtable htHourlyStats = HourlyStats.FetchHourlyStatsForOnlineUsers(from, to, getSelectedTypeInterval());

            if (htHourlyStats.Count > 0)
            {
                if (getSelectedTypeInterval() != HourlyStats.eType.Weekly)
                {
                    while (from.Date < to.Date)
                    {
                        string time;
                        int newUsers = 0;

                        if (htHourlyStats.ContainsKey(from))
                        {
                            newUsers = (int) htHourlyStats[from];
                        }

                        if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
                            time = from.ToString("g");
                        else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
                            time = from.ToShortDateString();
                        else time = from.ToString("Y");

                        dtStats.Rows.Add(new object[]
                                             {
                                                 time,
                                                 newUsers
                                             });

                        if (getSelectedTypeInterval() == HourlyStats.eType.Hourly)
                            from = from.AddHours(1);
                        else if (getSelectedTypeInterval() == HourlyStats.eType.Daily)
                            from = from.AddDays(1);
                        else from = from.AddMonths(1);
                    }
                }
                else if (getSelectedTypeInterval() == HourlyStats.eType.Weekly)
                {
                    while (from.Date < to.Date)
                    {
                        int newUsers = 0;
                        bool hasKey = false;
                        DateTime key = from.Date;

                        // from current date to 7 days after check is the hashtable has the day
                        for (int i = 0; i < 7; i++)
                        {
                            key = from.AddDays(i);
                            if (htHourlyStats.ContainsKey(key))
                            {
                                hasKey = true;
                                newUsers = (int) htHourlyStats[key];

                                dtStats.Rows.Add(new object[]
                                                     {
                                                         from.ToShortDateString(),
                                                         newUsers
                                                     });

                                break;
                            }
                        }

                        if (hasKey)
                        {
                            from = from.AddDays(7);
                            continue;
                        }

                        dtStats.Rows.Add(new object[]
                                             {
                                                 from.ToShortDateString(),
                                                 newUsers
                                             });

                        from = from.AddDays(7);
                    }
                }
            }

            return dtStats;
        }
    }
}