using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditBanners : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editBanners;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
//                loadBanners();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void loadStrings()  
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            btnSave.Text = "Save".TranslateA();
            btnDelete.Text = "Delete".TranslateA();

            ddPosition.Items.Add(new ListItem("", "-1"));

            foreach (string item in Enum.GetNames(typeof (BannerCode.ePosition)))
            {
                int value = (int) Enum.Parse(typeof (BannerCode.ePosition), item);
                ddPosition.Items.Add(new ListItem(item, value.ToString()));
            }

            ddVisibleFor.Items.Add(new ListItem("Any".TranslateA(), "-1"));
            ddVisibleFor.Items.Add(new ListItem("Logged in users".TranslateA(),
                                                ((int) BannerCode.eVisibleFor.LoggedInUsers).ToString()));
            ddVisibleFor.Items.Add(new ListItem("Non logged in visitors".TranslateA(),
                                                ((int) BannerCode.eVisibleFor.NonLoggedInVisitors).ToString()));

            ddGender.Items.Add(new ListItem("Any".TranslateA(), "-1"));
            ddGender.Items.Add(new ListItem("Male".TranslateA(), ((int) Classes.User.eGender.Male).ToString()));
            ddGender.Items.Add(new ListItem("Female".TranslateA(), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                ddGender.Items.Add(new ListItem("Couple".TranslateA(), ((int)Classes.User.eGender.Couple).ToString()));
            }

            ddPaid.Items.Add(new ListItem("Any".TranslateA(), "-1"));
            ddPaid.Items.Add(new ListItem("Yes".TranslateA()));
            ddPaid.Items.Add(new ListItem("No".TranslateA()));

            ddFromAge.Items.Add(new ListItem("Any".TranslateA(), "-1"));
            ddToAge.Items.Add(new ListItem("Any".TranslateA(), "-1"));
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++ )
            {
                ddFromAge.Items.Add(new ListItem(i.ToString()));
                ddToAge.Items.Add(new ListItem(i.ToString()));
            }

            for (int i = 1; i <= 10; i++)
            {
                ddPriority.Items.Add(new ListItem(i.ToString()));
            }
        }

        private void loadBannerCode(int position)
        {
            if (position != -1)
            {
                imgBannerCode.Src = Config.Urls.Home + "/Admin/images/bannericons/banner_" +
                                    (BannerCode.ePosition)position + ".jpg";

                populateDDTarget(position);

                loadTarget(Convert.ToInt32(ddTarget.SelectedValue));

                pnlBannerCode.Visible = true;
            }
            else pnlBannerCode.Visible = false;
        }

        private void loadTarget(int id)
        {
            if (ddTarget.SelectedIndex == 0) // default
            {
                bool hasDefaultInDB = false;
                BannerCode[] bannerCodes =
                    BannerCode.Fetch((BannerCode.ePosition)Convert.ToInt32(ddPosition.SelectedValue));
                if (bannerCodes.Length > 0)
                {
                    foreach (var bannerCode in bannerCodes)
                    {
                        if (bannerCode.Target == null)
                        {
                            txtBannerCode.Text = bannerCode.Code;
                            hasDefaultInDB = true;
                            break;
                        }
                    }
                }
                if (!hasDefaultInDB) txtBannerCode.Text = String.Empty;

                pnlTarget.Visible = false;
                btnDelete.Visible = false;
            }
            else if (id == -2)
            {
                txtName.Text = String.Empty;
                CascadingDropDownCountry.SelectedValue = " ";
                CascadingDropDownState.SelectedValue = " ";
                CascadingDropDownCity.SelectedValue = " ";
                ddGender.SelectedIndex = 0;
                ddVisibleFor.SelectedIndex = 0;
                ddPaid.SelectedIndex = 0;
                ddFromAge.SelectedIndex = 0;
                ddToAge.SelectedIndex = 0;
                ddPriority.SelectedIndex = 0;
                txtBannerCode.Text = String.Empty;

                pnlTarget.Visible = true;
                btnDelete.Visible = false;
            }
            else
            {
                BannerCode bannerCode = BannerCode.Fetch(id);

                if (bannerCode != null)
                {
                    BannerCodeTarget target = Misc.FromXml<BannerCodeTarget>(bannerCode.Target);

                    txtName.Text = target.Name;
                    CascadingDropDownCountry.SelectedValue = target.Country;
                    CascadingDropDownState.SelectedValue = target.Region;
                    CascadingDropDownCity.SelectedValue = target.City;
                    ddGender.SelectedValue = target.Gender.HasValue ? ((int) target.Gender).ToString() : "-1";
                    ddVisibleFor.SelectedValue = target.VisibleFor.HasValue
                                                     ? ((int) target.VisibleFor).ToString()
                                                     : "-1";
                    if (!target.Paid.HasValue) ddPaid.SelectedValue = "-1";
                    else ddPaid.SelectedIndex = target.Paid.Value ? 1 : 2;
                    ddFromAge.SelectedValue = target.FromAge.HasValue ? target.FromAge.ToString() : "-1";
                    ddToAge.SelectedValue = target.ToAge.HasValue ? target.ToAge.ToString() : "-1";
                    ddPriority.SelectedValue = bannerCode.Priority.ToString();
                    txtBannerCode.Text = bannerCode.Code;
                }

                pnlTarget.Visible = true;
                btnDelete.Visible = true;
            }
        }

        private void populateDDTarget(int position)
        {
            ddTarget.Items.Clear();

            BannerCode defaultBannerCode = null;
            BannerCode[] bannerCodes = BannerCode.Fetch((BannerCode.ePosition)position);
            foreach (BannerCode bannerCode in bannerCodes)
            {
                if (bannerCode.Target == null)
                {
                    defaultBannerCode = bannerCode;
                    continue;
                }
                BannerCodeTarget target = Misc.FromXml<BannerCodeTarget>(bannerCode.Target);

                ddTarget.Items.Add(new ListItem(target.Name, bannerCode.ID.ToString()));
            }

            if (defaultBannerCode == null)
            {
                ddTarget.Items.Insert(0, new ListItem("Default".TranslateA(), "-1"));
                btnDelete.Visible = false;
            }
            else
            {
                ddTarget.Items.Insert(0, new ListItem("Default".TranslateA(), defaultBannerCode.ID.ToString()));
                btnDelete.Visible = true;
            }

            ddTarget.Items.Add(new ListItem("- Add new -".TranslateA(), "-2"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess) return;

            #region validate input data

            string name = txtName.Text.Trim();
            string country = ddCountry.SelectedValue.Trim();
            string region = ddRegion.SelectedValue.Trim();
            string city = ddCity.SelectedValue.Trim();
            User.eGender? gender = null;
            if (Convert.ToInt32(ddGender.SelectedValue) != -1)
                gender = (User.eGender) Convert.ToInt32(ddGender.SelectedValue);
            BannerCode.eVisibleFor? visibleFor = null;
            if (Convert.ToInt32(ddVisibleFor.SelectedValue) != -1)
                visibleFor = (BannerCode.eVisibleFor) Convert.ToInt32(ddVisibleFor.SelectedValue);
            bool? paid;
            if (ddPaid.SelectedIndex == 0) paid = null;
            else paid = ddPaid.SelectedIndex == 1;
            int? fromAge = null;
            if (ddFromAge.SelectedIndex > 0) fromAge = Convert.ToInt32(ddFromAge.SelectedValue);
            int? toAge = null;
            if (ddToAge.SelectedIndex > 0) toAge = Convert.ToInt32(ddToAge.SelectedValue);
            int priority = Convert.ToInt32(ddPriority.SelectedValue);
            string code = txtBannerCode.Text.Trim();

            if (ddTarget.SelectedIndex > 0)
            {
                if (name.Length == 0)
                {
                    Messagebox1.Show("Please enter a name!", Misc.MessageType.Error);
                    return;
                }

                if (country.Length == 0 && region.Length == 0 && city.Length == 0
                    && gender == null && visibleFor == null && paid == null && fromAge == null && toAge == null)
                {
                    Messagebox1.Show("Please select at least one filter!", Misc.MessageType.Error);
                    return;
                }
            }

            BannerCode[] bannerCodes = BannerCode.Fetch((BannerCode.ePosition) Convert.ToInt32(ddPosition.SelectedValue));

            foreach (BannerCode bc in bannerCodes)
            {
                BannerCodeTarget target = Misc.FromXml<BannerCodeTarget>(bc.Target);

                if (target == null)
                {
                    continue;
                }

                if (target.Country == country && target.Region == region && target.City == city
                    && target.Gender == gender && target.VisibleFor == visibleFor &&
                    target.Paid == paid && target.FromAge == fromAge && target.ToAge == toAge && bc.Code == code)
                {
                    Messagebox1.Show("The specified target already exists!", Misc.MessageType.Error);
                    return;
                }
            }

            #endregion

            BannerCode bannerCode = null;

            if (ddTarget.SelectedValue == "-2" || ddTarget.SelectedValue == "-1") // add new
            {
                bannerCode = new BannerCode();
            }
            else
            {
                bannerCode = BannerCode.Fetch(Convert.ToInt32(ddTarget.SelectedValue));
            }

            if (bannerCode != null)
            {
                BannerCodeTarget target = new BannerCodeTarget();
                target.Name = name;
                target.Country = country;
                target.Region = region;
                target.City = city;
                target.Gender = gender;
                target.VisibleFor = visibleFor;
                target.Paid = paid;
                target.FromAge = fromAge;
                target.ToAge = toAge;
                bannerCode.Position = (BannerCode.ePosition)Convert.ToInt32(ddPosition.SelectedValue);
                bannerCode.Priority = priority;
                bannerCode.Code = code;
                bannerCode.Target = ddTarget.SelectedIndex > 0 ? Misc.ToXml(target) : null;

                bannerCode.Save();

                Messagebox1.Show("Banner codes have been successfully updated!", Misc.MessageType.Success);

                populateDDTarget(Convert.ToInt32(ddPosition.SelectedValue));
                ddTarget.SelectedValue = bannerCode.ID.ToString();
//                loadBannerCode(Convert.ToInt32(ddPosition.SelectedValue));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess) return;

            BannerCode.Delete(Convert.ToInt32(ddTarget.SelectedValue));
            populateDDTarget(Convert.ToInt32(ddPosition.SelectedValue));
            ddTarget.SelectedIndex = 0;
            loadTarget(Convert.ToInt32(ddTarget.SelectedValue));
        }

        protected void ddPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadBannerCode(Convert.ToInt32(ddPosition.SelectedValue));
        }

        protected void ddTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTarget(Convert.ToInt32(ddTarget.SelectedValue));
        }
    }
}
