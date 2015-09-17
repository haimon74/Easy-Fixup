using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditCoupons : AdminPageBase
    {
        private string CouponCode
        {
            get { return ViewState["CouponCode"] as string; }
            set { ViewState["CouponCode"] = value; }
        }

        private DataTable DataSource
        {
            get { return ViewState["EditCouponsDataSource"] as DataTable; }
            set { ViewState["EditCouponsDataSource"] = value; }
        }

        private delegate string Parser(string s);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                }

                LoadStrings();
                PopulateDataGrid();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editCoupons;
            base.OnInit(e);
        }

        private void LoadStrings()
        {
            btnAdd.Text = Lang.TransA("Add new Coupon");
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            var types = CouponType.FetchAll();
            foreach (CouponType couponType in types)
            {
                ddlCouponType.Items.Add(new ListItem(couponType.Name, ((int)couponType.TypeId).ToString()));
            }

            #region Set Grid's Headers
            dgCoupons.Columns[0].HeaderText = Lang.TransA("Coupon");
            dgCoupons.Columns[1].HeaderText = Lang.TransA("Active");
            dgCoupons.Columns[2].HeaderText = Lang.TransA("Order");
            dgCoupons.Columns[3].HeaderText = Lang.TransA("Commands");
            #endregion
        }

        private void PopulateDataGrid()
        {
            DataTable dtCoupons = new DataTable("Coupons");

            dtCoupons.Columns.Add("CouponID");
            dtCoupons.Columns.Add("CouponCode");
            dtCoupons.Columns.Add("CouponOwner");
            dtCoupons.Columns.Add("StartDate", typeof(DateTime));
            dtCoupons.Columns.Add("DueDate", typeof(DateTime));
            dtCoupons.Columns.Add("Used", typeof(int));
            dtCoupons.Columns.Add("MaxUse", typeof(int));
            dtCoupons.Columns.Add("OnlyNewUser", typeof(bool));
            dtCoupons.Columns.Add("CouponType");

            Coupon[] coupons = Coupon.FetchAll();
            var types = CouponType.FetchAll();

            foreach (Coupon coupon in coupons)
            {
                dtCoupons.Rows.Add(new object[]
                                         {
                                             coupon.Id,
                                             coupon.Code,
                                             coupon.Username,
                                             coupon.StartDate,
                                             coupon.DueDate,
                                             coupon.Used,
                                             coupon.MaxUse,
                                             coupon.OnlyForNewUser,
                                             types.Where(t=> (int)t.TypeId == coupon.CouponTypeId).Select(t=>t.Name).FirstOrDefault()
                                         });
            }

            DataSource = dtCoupons;

            dgCoupons.DataSource = dtCoupons;
            dgCoupons.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            txtCouponName.Text = String.Empty;
            txtUsername.Text = String.Empty;
            txtMaxUse.Text = "10000";
            cbRegistrationOnly.Checked = true;
            txtUsername.Text = String.Empty;
            txtStartDate.Text = DateTime.Today.ToShortDateString();
            txtDueDate.Text = DateTime.Today.AddMonths(1).ToShortDateString();
            pnlCoupons.Visible = false;
            pnlCoupon.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlCoupons.Visible = true;
            pnlCoupon.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (CouponCode.IsNullOrEmpty())
            {
                Coupon coupon = Coupon.Create();
                coupon.Username = txtUsername.Text.Trim();
                int max;
                coupon.MaxUse = int.TryParse(txtMaxUse.Text.Trim(), out max) ? max : 10000;
                coupon.Username = txtUsername.Text.Trim();
                DateTime date;
                coupon.StartDate = DateTime.TryParse(txtStartDate.Text.Trim(), out date) ? date : DateTime.Today;
                coupon.DueDate = DateTime.TryParse(txtDueDate.Text.Trim(), out date) ? date : DateTime.Today.AddMonths(1);
                coupon.CouponTypeId = int.Parse(ddlCouponType.SelectedValue);
                coupon.OnlyForNewUser = cbRegistrationOnly.Checked;
                coupon.Save();
            }
            else
            {
                Coupon coupon = Coupon.Fetch(CouponCode);

                if (txtUsername.Text.Trim().IsNotNullOrEmpty())
                    coupon.Username = txtUsername.Text.Trim();

                int max;
                if (int.TryParse(txtMaxUse.Text.Trim(), out max))
                    coupon.MaxUse = max;

                if (txtUsername.Text.Trim().IsNotNullOrEmpty())
                    coupon.Username = txtUsername.Text.Trim();

                DateTime date;
                if (DateTime.TryParse(txtStartDate.Text.Trim(), out date))
                    coupon.StartDate = date;
                if (DateTime.TryParse(txtDueDate.Text.Trim(), out date))
                    coupon.DueDate = date;

                coupon.OnlyForNewUser = cbRegistrationOnly.Checked;

                coupon.CouponTypeId = int.Parse(ddlCouponType.SelectedValue);

                coupon.Save();

                CouponCode = null;
            }

            PopulateDataGrid();
            pnlCoupon.Visible = false;
            pnlCoupons.Visible = true;
        }

        protected void dgCoupons_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string couponCode = e.CommandArgument.ToString();

            if (e.CommandName == "EditCoupon")
            {
                Coupon coupon = Coupon.Fetch(couponCode);
                pnlCoupon.Visible = true;
                pnlCoupons.Visible = false;
                txtCouponName.Text = coupon.Code;
                txtUsername.Text = coupon.Username;
                txtMaxUse.Text = coupon.MaxUse.ToString();
                cbRegistrationOnly.Checked = coupon.OnlyForNewUser;
                txtStartDate.Text = coupon.StartDate.ToShortDateString();
                txtDueDate.Text = coupon.DueDate.ToShortDateString();
                ddlCouponType.SelectedValue = coupon.CouponTypeId.ToString();
                CouponCode = coupon.Code;
            }
            else if (e.CommandName == "DeleteCoupon")
            {
                if (!HasWriteAccess)
                    return;

                Coupon.Delete(couponCode);
                PopulateDataGrid();
            }
            else
            {
                return;
            }
        }

        protected void dgCoupons_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            var lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

            if (HasWriteAccess) return;

            if (lnkDelete != null)
                lnkDelete.Enabled = false;
        }

        protected void dgCoupons_ItemCreated(object sender, DataGridItemEventArgs e)
        {
        }

        //private Parser parse = delegate(string text)
        //                           {
        //                               return String.Join(",", text.Split(','));
        //                           };
    }
}
