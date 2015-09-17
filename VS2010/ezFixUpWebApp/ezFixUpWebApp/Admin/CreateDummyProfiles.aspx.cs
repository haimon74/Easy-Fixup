using System;
using System.IO;
using System.Text;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    /// <summary>
    /// Summary description for BrowseUsers.
    /// </summary>
    public partial class CreateDummyProfiles : AdminPageBase
    {
        protected MessageBox MessageBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userAccounts;
            base.OnInit(e);
        }

        #endregion

        private void LoadStrings()
        {
            btnGenerateMembers.Text = Lang.TransA("Generate Members");
        }
        private void PerformSearch()
        {
            BasicSearch search = new BasicSearch();
            search.deleted_isSet = false;
            search.active_isSet = false;
            search.hasAnswer_isSet = false;
            search.visible_isSet = false;
            search.FaceControlApproved = null;

            //if (txtUsername.Text.Trim().Length > 0)
            //    search.Username = txtUsername.Text.Trim();
            //if (txtName.Text.Trim().Length > 0)
            //    search.Name = txtName.Text.Trim();
            //if (ddGender.SelectedIndex > 0)
            //    search.Gender = (User.eGender) Convert.ToInt32(ddGender.SelectedValue);

            try
            {
                search.MinAge = Convert.ToInt32(txtAgeFrom.Text.Trim());
                search.MaxAge = Convert.ToInt32(txtAgeTo.Text.Trim());
            }
            catch
            {
            }

            if (txtCountry.Text.Trim().Length > 0)
                search.Country = txtCountry.Text.Trim();
            if (txtRegion.Text.Trim().Length > 0)
                search.State = txtRegion.Text.Trim();
            if (txtCity.Text.Trim().Length > 0)
                search.City = txtCity.Text.Trim();
        }

        //protected void btnGetCSV_Click(object sender, EventArgs e)
        //{
        //    if (Results != null)
        //    {
        //        Response.Clear();
        //        Response.ContentType = "application/text";
        //        Response.ContentEncoding = Encoding.UTF8;
        //        Response.Charset = Encoding.UTF8.EncodingName;
        //        Response.AppendHeader("content-disposition", "attachment; filename=results.csv");
        //        Response.Write(Parsers.ParseCSV(FetchResultsDataTable(Results.Get(), true)));
        //        Response.End();
        //    }
        //}
        protected void btnGenerateMembersCsvClick(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/images/fakeprofiles/fakeprofiles.csv");
        
            if (!File.Exists(path)) return;

            string[] lines = File.ReadAllLines(path,Encoding.UTF8);
            int month = 0;
            int day = 0;
            foreach (var line in lines)
            {
                string[] fields = line.Split(',');
                var age = int.Parse(fields[4]);
                DateTime birthday = new DateTime(
                    DateTime.Now.Year - age, 
                    12-(++month%12), 29-(++day%29));
                User user = new User
                {
                    Active = true,
                    Country = fields[1],
                    State = fields[2],
                    City = fields[3],
                    Credits = 1000,
                    Birthdate = birthday,
                    Paid = true,
                    LanguageId = 6,
                    ProfileVisible = true
                };

                createUser(user,
                    fields[0].Replace(" ", ""),
                    fields[9] == "רווק" ? "Single" : "Married",
                    fields[5] == "גבר" ? Classes.User.eGender.Male : Classes.User.eGender.Female, 
                    fields[6] == "גבר" ? Classes.User.eGender.Male : Classes.User.eGender.Female, 
                    (int)(DateTime.Now.Ticks % (255 * 255)));
                    
            }
        }
        protected void btnGenerateMembers_Click(object sender, EventArgs e)
        {
            int fromAge, toAge;
            if (!int.TryParse(this.txtAgeFrom.Text,out fromAge))
                fromAge = Config.Users.MinAge;
            if (!int.TryParse(this.txtAgeTo.Text,out toAge))
                toAge = Config.Users.MaxAge;

            int month = 0;
            int day = 0;
            int repeatMax = int.Parse(txtRepeatingTimes.Text);

            for (int age = fromAge; age <= toAge; age++)
            {
                DateTime birthday = new DateTime(DateTime.Now.Year-age,month+1,day+1);
                for (int i = 0; i < repeatMax; i++)
                {
                    User user = new User
                                    {
                                        Active = true,
                                        Country = txtCountry.Text,
                                        State = txtRegion.Text,
                                        City = txtRegion.Text,
                                        Credits = 1000,
                                        Birthdate = birthday,
                                        Paid = (age % 2) == 0,
                                        LanguageId = 6,
                                        ProfileVisible=true
                                    };
                    string ext = (user.Paid ? "p" : "np")+i;
                    createUser(user, "Male" + age + ext, "Single", Classes.User.eGender.Male, Classes.User.eGender.Female, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Man" + age + ext, "Single", Classes.User.eGender.Male, Classes.User.eGender.Female, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Divorced" + age + ext, "Divorce", Classes.User.eGender.Male, Classes.User.eGender.Female, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Female" + age + ext, "Single", Classes.User.eGender.Female, Classes.User.eGender.Male, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Woman" + age + ext, "Single", Classes.User.eGender.Female, Classes.User.eGender.Male, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Divorcee" + age + ext, "Divorce", Classes.User.eGender.Female, Classes.User.eGender.Male, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Gay" + age + ext, "Single", Classes.User.eGender.Male, Classes.User.eGender.Male, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "Lesbi" + age + ext, "Single", Classes.User.eGender.Female, Classes.User.eGender.Female, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "MarriedM" + age + ext, "Married", Classes.User.eGender.Male, Classes.User.eGender.Matchmaker, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "MarriedF" + age + ext, "Married", Classes.User.eGender.Female, Classes.User.eGender.Matchmaker, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "MarriedG" + age + ext, "Married", Classes.User.eGender.Male, Classes.User.eGender.Matchmaker, (int)(DateTime.Now.Ticks % (255 * 255)));
                    createUser(user, "MarriedL" + age + ext, "Married", Classes.User.eGender.Female, Classes.User.eGender.Matchmaker, (int)(DateTime.Now.Ticks % (255 * 255)));
                }
                month = (month+1) % 12;
                day = (day+1) % 28;
            }
        }
        private void createUser(User user, string username, string status, 
                        User.eGender gender, User.eGender interestedIn, int ipIdx)
        {
            if (!Classes.User.IsUsernameTaken(username))
            {
                user.Username = username;
                user.Password = username;
                user.Name = String.Format("{0} {1}{2}", status, gender, user.Age);
                user.Gender = gender;
                user.InterestedIn = interestedIn;
                user.StatusText = status;
                user.Email = String.Format("{0}@ezFixUp.com", username);
                Classes.User.Create(user, String.Format("192.168.{0}.{1}", ipIdx/255, ipIdx%255));
            }
            
            var random = new Random((int)(DateTime.Now.Ticks%int.MaxValue));
            ProfileTopic[] topics = ProfileTopic.Fetch();
            foreach (ProfileTopic profileTopic in topics)
            {
                ProfileQuestion[] questions = profileTopic.FetchQuestions();
                foreach (ProfileQuestion profileQuestion in questions)
                {
                    try
                    {
                        var existAnswer = ProfileAnswer.Fetch(username, profileQuestion.Id);
                        if (existAnswer != null) continue;
                    }
                    catch
                    {
                        ProfileChoice[] choices = profileQuestion.FetchChoices();
                        if (choices!=null&&choices.Length > 0)
                        {
                            var answer = new ProfileAnswer(username, profileQuestion.Id)
                                             {
                                                 Value = choices[random.Next(choices.Length - 1)].Value
                                             };
                            answer.Save();
                        }
                    }
                }
            }
        }
    }
}