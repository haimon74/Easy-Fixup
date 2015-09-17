using System;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class ReportAbuse : System.Web.UI.UserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }
        public event EventHandler SendClickEvent;
        protected virtual void OnSendClick(EventArgs e)
        {
            if (SendClickEvent != null)
            {
                SendClickEvent(this, e);
            }
        }
        public event EventHandler CancelClickEvent;
        protected virtual void OnCancelClick(EventArgs e)
        {
            if (CancelClickEvent != null)
            {
                CancelClickEvent(this, e);
            }
        }        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSend.Text = Lang.Trans("Report");
                btnCancel.Text = Lang.Trans("Cancel");                
                litText.Text = Lang.Trans("Please tell us why you are reporting this user");
            }
        }
        
        public int? TargetID
        {
            get
            {
                return ViewState["TargetID"] as int?;
            }
            set{ ViewState["TargetID"] = value;}
        }

        public string Text
        {
            set {litText.Text = value; }
        }
        
        public string ReportedUser
        {
            get
            {
                return ViewState["ReportedUser"] as string;
            }
            set { ViewState["ReportedUser"] = value; }
        }
        
        public AbuseReport.ReportType ReportType
        {
            get
            {
                return (AbuseReport.ReportType) ViewState["ReportType"];
            }
            set 
            { 
                ViewState["ReportType"] = value;
                string key = "ReportAbuse" + value.ToString();
                switch (value)
                {
                    case AbuseReport.ReportType.Profile:
                        cvProfile.Key = key;
                        mvContentViews.SetActiveView(vProfile);
                        break;
                    case AbuseReport.ReportType.Photo:
                        cvPhoto.Key = key;
                        mvContentViews.SetActiveView(vPhoto);
                        break;
                    case AbuseReport.ReportType.Message:
                        cvMessage.Key = key;
                        mvContentViews.SetActiveView(vMessage);
                        break;
                }                
            }            
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Visible = false;
            OnCancelClick(new EventArgs());
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {            
            Visible = false;
            if (txtReport.Text.Length > 0)
            {
                if (CurrentUserSession != null)
                {
                    AbuseReport report = new AbuseReport(CurrentUserSession.Username, ReportedUser, ReportType);

                    report.Report = txtReport.Text;
                    report.TargetID = TargetID;
                    report.DateReported = DateTime.Now;
                    report.Save();

                    if (ReportType == AbuseReport.ReportType.Photo)
                    {
                        int reports = AbuseReport.Search(null, ReportedUser, AbuseReport.ReportType.Photo, TargetID.Value, false, null, null,
                                           String.Empty, false).Length;

                        if (reports > Config.CommunityModeratedSystem.MaxPhotoAbuseReportsToDeletePhoto)
                        {
                            Photo.Delete(TargetID.Value);
                        }
                        else if (reports > Config.CommunityModeratedSystem.MaxPhotoAbuseReportsForManualApproval)
                        {
                            Photo photo = null;

                            try
                            {
                                photo = Photo.Fetch(TargetID.Value);
                            }
                            catch (NotFoundException)
                            {
                                return;
                            }

                            photo.ManualApproval = true;
                            photo.Save(false);
                        }    
                    }

                    OnSendClick(new EventArgs());
                    
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("Your report has been sent successufuly!");
                    Response.Redirect("ShowStatus.aspx");
                }
            }
            else
            {
                Visible = true;
                lblError.Text = Lang.Trans("Please fill in your report in the text field!");
                lblError.Visible = true;
                return;
            }
        }        
    }
}