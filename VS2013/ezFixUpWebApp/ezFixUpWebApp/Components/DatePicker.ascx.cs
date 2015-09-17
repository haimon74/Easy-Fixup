using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;


namespace ezFixUp.Components
{
    public partial class DatePicker : UserControl
    {
        #region Properties

        public DateTime SelectedDate
        {
            get
            {
                return new DateTime(
                    Convert.ToInt32(dropYear.SelectedValue),
                    Convert.ToInt32(dropMonth.SelectedValue),
                    Convert.ToInt32(dropDay.SelectedValue));
            }
            set
            {
                if (dropYear.Items.Count == 0) initializeControl();
                dropYear.SelectedValue = value.Year.ToString();
                dropMonth.SelectedValue = value.Month.ToString();
                dropDay.SelectedValue = value.Day.ToString();
            }
        }

        public bool ValidDateEntered
        {
            get
            {
                int year = Convert.ToInt32(dropYear.SelectedValue);
                int month = Convert.ToInt32(dropMonth.SelectedValue);
                int day = Convert.ToInt32(dropDay.SelectedValue);
                try
                {
                    new DateTime(year, month, day);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
        }

        public string CssClass
        {
            set { divDatePicker.Attributes.Add("class", value); }
        }

        public bool DateRestriction
        {
            get { return dateRestriction; }
            set { dateRestriction = value; }
        }

        /// <summary>
        /// Resets this instance.
        /// Sets year, month and day with their undefined values.
        /// </summary>
        public void Reset()
        {
            dropYear.SelectedIndex = 0;
            dropMonth.SelectedIndex = 0;
            dropDay.SelectedIndex = 0;
        }

        private bool dateRestriction = true;

        private int minYear = 0;
        public int MinYear
        {
            get { return minYear; }
            set
            {
                dropYear.Items.Clear();
                minYear = value;
                initializeControl();
            }
        }  

        private int maxYear = 0;
        public int MaxYear
        {
            get { return maxYear; }
            set
            {
                dropYear.Items.Clear();
                maxYear = value;
                initializeControl();
            }
        }        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //initializeControl();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            initializeControl();
        }

        private void initializeControl()
        {
            if (dropDay.Items.Count == 0)
            {
                dropDay.Items.Add(new ListItem(Lang.Trans("day"), "0"));
                for (int day = 1; day <= 31; day++)
                {
                    dropDay.Items.Add(
                        new ListItem(day.ToString(), day.ToString()));
                }
            }

            if (dropMonth.Items.Count == 0)
            {
                dropMonth.Items.Add(new ListItem(Lang.Trans("month"), "0"));
                for (int month = 1; month <= 12; month++)
                {
                    dropMonth.Items.Add(
                        new ListItem(DateTimeFormatInfo.CurrentInfo.GetMonthName(month), month.ToString()));
                }
            }

            if (dropYear.Items.Count == 0)
            {
                dropYear.Items.Add(new ListItem(Lang.Trans("year"), "0"));
                int minYear = DateTime.Now.Year - Config.Users.MaxAge;
                int maxYear = DateTime.Now.Year - Config.Users.MinAge;
                if (!dateRestriction) maxYear = DateTime.Now.Year;
                if (this.minYear != 0) minYear = this.minYear;                
                if (this.maxYear != 0) maxYear = this.maxYear;
                for (int year = maxYear; year >= minYear; year--)
                {
                    dropYear.Items.Add(
                        new ListItem(year.ToString(), year.ToString()));
                }
            }
        }
    }
}