using System;
using ezFixUp.Classes;

namespace ezFixUp.Components.Search
{
    public partial class AgeRangePicker : System.Web.UI.UserControl
    {
        private bool _ageFromIsSet;
        public int AgeFrom
        {
            get { return Convert.ToInt32(this.comboAgeFrom.SelectedValue); }
            set 
            { 
                this.comboAgeFrom.SelectedValue = value.ToString();
                _ageFromIsSet = true;
            }
        }

        private bool _ageToIsSet;
        public int AgeTo
        {
            get { return Convert.ToInt32(this.comboAgeTo.SelectedValue); }
            set
            {
                this.comboAgeTo.SelectedValue = value.ToString();
                _ageToIsSet = true;
            }
        }

        private int _minAge;
        private int _maxAge;
        public int MinAge
        {
            get { return _minAge = (_minAge == 0 || _minAge < Config.Users.MinAge) ? Config.Users.MinAge : _minAge; }
            set { _minAge = (value == 0 || value < Config.Users.MinAge) ? Config.Users.MinAge : value; }
        }
        public int MaxAge
        {
            get
            {
                _maxAge = (_maxAge == 0 || _maxAge > Config.Users.MaxAge) ? Config.Users.MaxAge : _maxAge;
                return (_maxAge < MinAge) ? MinAge : _maxAge;
            }
            set { _maxAge = (value == 0 || value > Config.Users.MaxAge) ? Config.Users.MaxAge : value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((comboAgeFrom.Items.Count < 2 || comboAgeTo.Items.Count < 2))
            {
                PopulateCombos(null);
            }
        }
        public void PopulateCombos(User user)
        {
            comboAgeFrom.Items.Clear();
            comboAgeTo.Items.Clear();
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                comboAgeTo.Items.Add(i.ToString());
                comboAgeFrom.Items.Add(i.ToString());
            }
            comboAgeFrom.SelectedValue = user != null && user.Age - 10 > MinAge ? (user.Age - 10).ToString() : MinAge.ToString();
            comboAgeTo.SelectedValue = user != null && user.Age + 10 < MaxAge ? (user.Age + 10).ToString() : MaxAge.ToString();    
        }
        
        protected void comboAgeToSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        protected void comboAgeFromSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}