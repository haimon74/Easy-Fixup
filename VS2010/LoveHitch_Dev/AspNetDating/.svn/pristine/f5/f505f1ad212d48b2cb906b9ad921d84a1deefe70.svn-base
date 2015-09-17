using System;
using System.IO;
using System.Text;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for BrowseUsers.
    /// </summary>
    public partial class ImportDummyProfiles : AdminPageBase
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
            btnGenerateMembers.Text = Lang.TransA("Import New Users");
        }

        protected void BtnGenerateMembersCsvClick(object sender, EventArgs e)
        {
            //var path = Server.MapPath("~/images/profiles/List.xlsx");
            var path = Config.Directories.Home + @"\images\profiles\List.csv";

            if (!File.Exists(path))
            {
                MessageBox.Show("List.cvs file doesn't exist.", Misc.MessageType.Error);
                return;
            }
            try
            {
                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                int month = 0;
                int day = 0;
                //foreach (var line in lines)
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length < 7) continue;

                    var age = int.Parse(fields[4]);
                    DateTime birthday = new DateTime(
                        DateTime.Now.Year - age,
                        12 - (++month % 12), 29 - (++day % 29));
                    User user = new User
                                    {
                                        Active = true,
                                        Country = fields[1],
                                        State = fields[2],
                                        City = fields[3],
                                        Credits = 100000,
                                        Birthdate = birthday,
                                        Paid = true,
                                        LanguageId = 6,
                                        ProfileVisible = true
                                    };
                    createUser(
                        user,
                        fields[0],
                        "Single",
                        fields[5].ToLower() == "man" ? Classes.User.eGender.Male : Classes.User.eGender.Female,
                        fields[6].ToLower() == "man" ? Classes.User.eGender.Male : Classes.User.eGender.Female,
                        (int)(DateTime.Now.Ticks % (255 * 255)),
                        age,
                        fields.Length > 7 ? fields[7] : null,
                        fields.Length > 8 ? fields[8] : null
                        );
                }
                MessageBox.Show("Operation Succeeded.", Misc.MessageType.Success);
            }
            catch (Exception ex)
            {
                var msg = String.Format("Operation didn't complete. <br/>Had exception: {0} <br/>Inner exception: {1} <br/>Stack Trace: {2}"
                                        , ex.Message
                                        , ex.InnerException != null ? ex.InnerException.Message : ""
                                        , ex.StackTrace);
                MessageBox.Show(msg, Misc.MessageType.Error);
            }
        }

        private void StoreUserPhoto(User user, string photoPath, bool isPrimary)
        {
            if (photoPath.IsNotNullOrEmpty() && File.Exists(photoPath))
            {
                try
                {
                    using (var image = System.Drawing.Image.FromFile(photoPath))
                    {
                        var photo = new Photo();
                        photo.Image = image;
                        photo.Primary = isPrimary;
                        photo.Approved = true;
                        photo.Name = String.Empty;
                        photo.Description = String.Empty;
                        photo.User = user;
                        photo.Save(true);
                        if (isPrimary)
                            Photo.SetPrimary(user.Username, photo);
                        photo.ApprovePhoto(CurrentAdminSession.Username);
                    }
                    
                    var destination = photoPath.Replace(".jpg", "v.jpg");
                    File.Copy(photoPath, destination);
                    File.Delete(photoPath);
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
        }

        private void createUser(User user, string name, string status,
                                User.eGender gender, User.eGender interestedIn,
                                int ipIdx, int age, string img1, string img2)
        {
            var username = name.Replace(" ", "").Substring(0, 4) + age;
            var random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            int day = random.Next(1, 28);
            int month = random.Next(1, 12);
            int year = DateTime.Now.Year - age;

            if (!Classes.User.IsUsernameTaken(username))
            {
                user.Username = username;
                user.Password = username;
                user.Name = name;
                user.Gender = gender;
                user.InterestedIn = interestedIn;
                user.StatusText = status;
                user.Email = String.Format("{0}@lovehitch.com", username);
                user.Birthdate = new DateTime(year, month, day);
                Classes.User.Create(user, String.Format("192.168.{0}.{1}", ipIdx / 255, ipIdx % 255));

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
                            if (choices != null && choices.Length > 0)
                            {
                                var answer = new ProfileAnswer(username, profileQuestion.Id)
                                {
                                    Value = choices[random.Next(choices.Length - 1)].Value
                                };
                                try
                                {
                                    answer.Save();
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
            }
            else
            {
                user = Classes.User.Load(username);
            }

            if (user != null && user.SignupIp.IsNotNullOrEmpty() && user.SignupIp.StartsWith("192.168."))
            {
                user.updateLastLogin(DateTime.Now.Ticks.ToString());

                var pathTemplate = Config.Directories.Home + @"\images\profiles\{0}.jpg";
                if (img1.Trim().IsNotNullOrEmpty())
                    StoreUserPhoto(user, String.Format(pathTemplate, img1), true);
                if (img2.IsNotNullOrEmpty())
                    StoreUserPhoto(user, String.Format(pathTemplate, img2), false);
            }
        }
    }
}