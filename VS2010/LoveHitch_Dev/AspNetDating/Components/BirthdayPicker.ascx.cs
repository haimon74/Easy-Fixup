using System;
using System.Globalization;


namespace AspNetDating.Components
{
    public partial class BirthdayPicker : System.Web.UI.UserControl
    {
        public string Text { get { return this.MyDatePicker1.Text; } }
        public string CurrentText { set { this.MyDatePicker1.CurrentText = value; this.MyDatePicker1.Text = value; } }
        public DateTime SelectedDate
        {
            get
            {
                IFormatProvider culture = new CultureInfo("he-IL", true);
                string dateFormat = "dd/MM/yyyy";
                DateTime resultDate;
                if (!DateTime.TryParseExact(this.Text, dateFormat, culture, DateTimeStyles.None, out resultDate))
                    resultDate =  this.MyDatePicker1.MinDate;
                return resultDate;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}