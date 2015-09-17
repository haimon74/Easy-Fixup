using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Data;
using WebPartZone=System.Web.UI.WebControls.WebParts.WebPartZone;

namespace ezFixUp.Classes
{
    public class CustomWebDisplayNameAttribute : WebDisplayNameAttribute
    {
        public CustomWebDisplayNameAttribute(string displayName) : base(displayName)
        {
        }

        public override string DisplayName
        {
            get { return Lang.Trans(base.DisplayName); }
        }
    }

    public class GenderPropertyAttribute : Attribute
    {
        
    }

    public class AgePropertyAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EditableAttribute : Attribute
    {
        private readonly bool editable;

        // Constructors 
        public EditableAttribute(bool editable)
        {
            this.editable = editable;
        }

        public EditableAttribute()
        {
            editable = true;
        }

        public bool Editable
        {
            get { return editable; }
        }
    }

    public class WebPartUserControl : UserControl, IWebPart, IWebEditable, IWebActionable
    {
        private WebPartVerbCollection verbs;

        public bool IsEditable
        {
            get
            {
                return false;
                var atts = (EditableAttribute[]) GetType().GetCustomAttributes(typeof (EditableAttribute), true);

                if (atts.Length > 0)
                    return atts[0].Editable;
                return false;
            }
        }

        #region IWebActionable Members

        public WebPartVerbCollection Verbs
        {
            get
            {
                if (Config.Misc.LockHomePageLayout)
                    return null;

                var verbsList = new List<WebPartVerb>();

                if (verbs != null)
                    return verbs;

                var moveUpVerb = new WebPartVerb
                    ("moveUpVerb", new WebPartEventHandler(OnCustomMoveUpVerbClick))
                                     {
                                         Text = "Move Up".Translate(),
                                         Description = "Move Up".Translate(),
                                         Visible = true,
                                         Enabled = true,
                                         ImageUrl = "~/images/moveup.png"
                                     };
                verbsList.Add(moveUpVerb);

                var moveDownVerb = new WebPartVerb
                    ("moveDownVerb", new WebPartEventHandler(OnCustomMoveDownVerbClick))
                                    {
                                        Text = "Move Down".Translate(),
                                        Description = "Move Down".Translate(),
                                        Visible = true,
                                        Enabled = true,
                                        ImageUrl = "~/images/movedown.png"
                                    };
                verbsList.Add(moveDownVerb);

                var minimizeVerb = new WebPartVerb
                    ("customMinimizeVerb", new WebPartEventHandler(OnCustomMinimizeVerbClick))
                                       {
                                           Text = Lang.Trans("Minimize"),
                                           Description = Lang.Trans("Minimize"),
                                           Visible = true,
                                           Enabled = true
                                       };
                verbsList.Add(minimizeVerb);

                var restoreVerb = new WebPartVerb
                    ("customRestoreVerb", new WebPartEventHandler(OnCustomRestoreVerbClick))
                                      {
                                          Text = Lang.Trans("Restore"),
                                          Description = Lang.Trans("Restore"),
                                          Visible = true,
                                          Enabled = true
                                      };
                verbsList.Add(restoreVerb);

                if (IsEditable)
                {
                    var editVerb = new WebPartVerb
                        ("customEditVerb", new WebPartEventHandler(OnCustomEditVerbClick))
                                       {
                                           Text = Lang.Trans("Edit"),
                                           Description = Lang.Trans("Edit"),
                                           Visible = true,
                                           Enabled = true
                                       };
                    verbsList.Add(editVerb);
                }

                var deleteVerb = new WebPartVerb
                    ("customDeleteVerb", new WebPartEventHandler(OnCustomDeleteVerbClick))
                                     {
                                         Text = Lang.Trans("Close"),
                                         Description = Lang.Trans("Close"),
                                         Visible = true,
                                         Enabled = true
                                     };
                verbsList.Add(deleteVerb);

                verbs = new WebPartVerbCollection(verbsList);

                return verbs;
            }
        }

        #endregion

        #region IWebEditable Members

        public EditorPartCollection CreateEditorParts()
        {
            var editorPart = new CustomEditorPart
                                 {
                                     ID = (ID + "CustomPropertyEditor"),
                                     ChromeType = PartChromeType.None
                                 };
            return new EditorPartCollection(new EditorPart[] {editorPart});
        }

        public object WebBrowsableObject
        {
            get { return this; }
        }

        #endregion

        #region IWebPart Members

        public string CatalogIconImageUrl
        {
            get { return ViewState["CatalogIconImageUrl"] as string ?? String.Empty; }
            set { ViewState["CatalogIconImageUrl"] = value; }
        }

        public string Description
        {
            get { return ViewState["Description"] as string ?? String.Empty; }
            set { ViewState["Description"] = value; }
        }

        public string Subtitle
        {
            get { return String.Empty; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayName("Title")]
        public string Title
        {
            get { return ViewState["Title"] as string ?? String.Empty; }
            set { ViewState["Title"] = value; }
        }

        public string TitleIconImageUrl
        {
            get { return ViewState["TitleIconImageUrl"] as string ?? String.Empty; }
            set { ViewState["TitleIconImageUrl"] = value; }
        }

        public string TitleUrl
        {
            get { return ViewState["TitleUrl"] as string ?? String.Empty; }
            set { ViewState["TitleUrl"] = value; }
        }

        #endregion

        private static void OnCustomMoveUpVerbClick(object sender, WebPartEventArgs e)
        {
            if (e.WebPart.ZoneIndex > 0)
            {
                var webPartManager = WebPartManager.GetCurrentWebPartManager(e.WebPart.Page);
                if (webPartManager == null) return;
                webPartManager.MoveWebPart(e.WebPart, e.WebPart.Zone, e.WebPart.ZoneIndex - 1);
            }
        }

        private static void OnCustomMoveDownVerbClick(object sender, WebPartEventArgs e)
        {
            if (e.WebPart.ZoneIndex + 1 < e.WebPart.Zone.WebParts.Count)
            {
                var webPartManager = WebPartManager.GetCurrentWebPartManager(e.WebPart.Page);
                if (webPartManager == null) return;
                webPartManager.MoveWebPart(e.WebPart, e.WebPart.Zone, e.WebPart.ZoneIndex + 1);
            }
        }

        private void OnCustomEditVerbClick(object sender,
                                           WebPartEventArgs e)
        {
            var manager = (CustomWebPartManager.CustomWebPartManager) 
                WebPartManager.GetCurrentWebPartManager(Page);

            if (manager == null) return;

            if (manager.DisplayMode != WebPartManager.EditDisplayMode)
            {
                manager.DisplayMode = WebPartManager.EditDisplayMode;
                manager.EditedWebPartID = e.WebPart.ID;
            }
            else
            {
                WebPart editedWebPart = null;
                foreach (WebPart part in manager.WebParts)
                {
                    if (part.ID == manager.EditedWebPartID)
                    {
                        editedWebPart = part;
                        break;
                    }
                }

                if (editedWebPart != null && editedWebPart != e.WebPart)
                {
                    manager.EndWebPartEditing();
                    manager.ReOpenClosedWebParts();
                    manager.DisplayMode = WebPartManager.EditDisplayMode;
                    manager.EditedWebPartID = e.WebPart.ID;
                }
                else return;
            }

            manager.BeginWebPartEditing(e.WebPart);

            SortedList<int, WebPart> sortedWebParts = new SortedList<int, WebPart>();
            foreach (WebPart part in e.WebPart.Zone.WebParts)
                sortedWebParts.Add(part.ZoneIndex, part);
            List<string> ids = new List<string>();
            foreach (var v in sortedWebParts.Values)
                ids.Add(v.ID);

            manager.SortedWebPartIDs = ids;

            foreach (WebPart part in e.WebPart.Zone.WebParts)
            {
                if (part != e.WebPart)
                {
                    ((CustomWebPartZone.CustomWebPartZone) e.WebPart.Zone).CloseWebPart(part);
                }
            }
        }

        private void OnCustomDeleteVerbClick(object sender,
                                             WebPartEventArgs e)
        {
            WebPartManager manager = WebPartManager.GetCurrentWebPartManager(Page);

            if (manager == null) return;

            if (manager.DisplayMode != WebPartManager.EditDisplayMode)
                manager.DisplayMode = WebPartManager.EditDisplayMode;

            manager.DeleteWebPart(e.WebPart);

            manager.DisplayMode = WebPartManager.BrowseDisplayMode;
        }

        private static void OnCustomMinimizeVerbClick(object sender, WebPartEventArgs e)
        {
            e.WebPart.ChromeState = PartChromeState.Minimized;
        }

        private static void OnCustomRestoreVerbClick(object sender, WebPartEventArgs e)
        {
            e.WebPart.ChromeState = PartChromeState.Normal;
        }
    }

    public class CustomWebPartChrome : WebPartChrome
    {
        public CustomWebPartChrome(WebPartZoneBase zone, WebPartManager manager)
            : base(zone, manager)
        {
        }
        protected override void RenderPartContents(HtmlTextWriter writer,WebPart part)
        {

            if (part == this.WebPartManager.SelectedWebPart)
                HttpContext.Current.Response.Write("<span>Not rendered</span>");
            else //if (this.Zone.GetType() == typeof(MyZone))
                part.RenderControl(writer);
        }
        protected override Style CreateWebPartChromeStyle(WebPart webPart, PartChromeType chromeType)
        {
            var style = new Style();

            string className = null;

            if (webPart.Controls.Count > 0)
            {
                string rawName = webPart.Controls[0].GetType().Name;
                string[] parts = rawName.Split('_');
                if (parts.Length >= 2)
                {
                    className = parts[parts.Length - 2];
                }
            }
            style.CssClass = String.Format(String.IsNullOrEmpty(className) ? "{0} {0}_{1}" : "{0} {0}_{1} {0}_{2}",
                                           webPart.Zone.ID, webPart.ZoneIndex, className);
            return style;
        }

        protected override WebPartVerbCollection FilterWebPartVerbs(WebPartVerbCollection verbs, WebPart webPart)
        {
            WebPartVerbCollection filteredVerbs = base.FilterWebPartVerbs(verbs, webPart);
            var finalVerbs = new List<WebPartVerb>();

            foreach (WebPartVerb verb in filteredVerbs)
            {
                //get images from the original verbs and set them on our new custom ones
                if (verb.ID == "customEditVerb")
                    verb.ImageUrl = Zone.EditVerb.ImageUrl;
                else if (verb.ID == "customDeleteVerb")
                    verb.ImageUrl = Zone.DeleteVerb.ImageUrl;
                else if (verb.ID == "customMinimizeVerb")
                    verb.ImageUrl = Zone.MinimizeVerb.ImageUrl;
                else if (verb.ID == "customRestoreVerb")
                    verb.ImageUrl = Zone.RestoreVerb.ImageUrl;

                if (!(verb.ID == "customRestoreVerb" && webPart.ChromeState == PartChromeState.Normal) &&
                    !(verb.ID == "customMinimizeVerb" && webPart.ChromeState == PartChromeState.Minimized))
                    finalVerbs.Add(verb);
            }

            return new WebPartVerbCollection(finalVerbs);
        }
    }

    public class CustomEditorPart : EditorPart
    {
        private Control[] controls;
        private PropertyInfo[] properties;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            properties = GetBrowsableProperties(((GenericWebPart) WebPartToEdit).ChildControl);

            controls = CreateControlsByProperyInfo(properties);
            PopulateControls(((GenericWebPart) WebPartToEdit).ChildControl, properties, controls);
            var table = new Table {CssClass = "EditorZoneTable"};
            foreach (Control control in controls)
            {
                string correspondingPropertyName = control.ID.Remove(0, ID.Length);

                PropertyInfo property = Array.Find(properties, p => p.Name == correspondingPropertyName);

                if (property != null)
                {
                    var row = new TableRow();

                    var label = new Label();

                    Attribute att = Attribute.GetCustomAttribute(property, typeof (WebDisplayNameAttribute));
                    label.Text = att != null ? ((WebDisplayNameAttribute) att).DisplayName : property.Name;

                    var cell = new TableCell {CssClass = "editorzonelabel"};
                    cell.Controls.Add(label);
                    row.Cells.Add(cell);

                    cell = new TableCell {CssClass = "editorzoneinput"};
                    cell.Controls.Add(control);
                    row.Cells.Add(cell);

                    table.Rows.Add(row);
                }

                //
            }

            Controls.Add(table);
            ChildControlsCreated = true;
        }

        public override bool ApplyChanges()
        {
            EnsureChildControls();

            try
            {
                PopulateProperties(((GenericWebPart) WebPartToEdit).ChildControl, properties, controls);
            }
            catch (FormatException ex)
            {
                Zone.ErrorText = ex.Message;
                return false;
            }

            return true;
        }

        public override void SyncChanges()
        {
            EnsureChildControls();


            PopulateControls(((GenericWebPart) WebPartToEdit).ChildControl, properties, controls);
        }

        private static PropertyInfo[] GetBrowsableProperties(Control part)
        {
            PropertyInfo[] propertyInfos = part.GetType().GetProperties();

            var browsableProperties = new List<PropertyInfo>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                WebBrowsableAttribute att = (WebBrowsableAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(WebBrowsableAttribute));
                Attribute genderPropertyAtt = Attribute.GetCustomAttribute(propertyInfo, typeof(GenderPropertyAttribute));
                Attribute agePropertyAtt = Attribute.GetCustomAttribute(propertyInfo, typeof(AgePropertyAttribute));
                if (att != null && att.Browsable && !(Config.Users.DisableGenderInformation && genderPropertyAtt != null)
                    && !(Config.Users.DisableAgeInformation && agePropertyAtt != null))
                {
                    browsableProperties.Add(propertyInfo);
                }
            }

            return browsableProperties.ToArray();
        }

        private Control[] CreateControlsByProperyInfo(IEnumerable<PropertyInfo> props)
        {
            var ctrls = new List<Control>();
            foreach (PropertyInfo info in props)
            {
                Control control;
                if (info.PropertyType.IsEnum)
                {
                    control = new DropDownList();
                }
                else if (info.PropertyType == typeof (bool))
                {
                    control = new CheckBox();
                }
                else
                {
                    control = new TextBox();
                }

                control.ID = ID + info.Name;
                ctrls.Add(control);
            }

            return ctrls.ToArray();
        }

        private void PopulateControls(Control part, PropertyInfo[] props, IEnumerable<Control> ctrls)
        {
            foreach (Control control in ctrls)
            {
                Control control1 = control;
                PropertyInfo property = Array.Find(props, p => ID + p.Name == control1.ID);

                if (property != null)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        var ddControl = (DropDownList) control;
                        ddControl.Items.Clear();

                        Array values = Enum.GetValues(property.PropertyType);

                        for (int i = 0; i < values.Length; ++i)
                        {
                            ddControl.Items.Add(new ListItem(Lang.Trans(values.GetValue(i).ToString()),
                                                             values.GetValue(i).ToString()));
                        }
                        ddControl.SelectedValue = property.GetValue(part, null).ToString();
                    }
                    else if (property.PropertyType == typeof (bool))
                    {
                        var cbControl = (CheckBox) control;
                        var value = (bool) property.GetValue(part, null);
                        cbControl.Checked = value;
                    }
                    else
                    {
                        var txtControl = (TextBox) control;
                        object value = property.GetValue(part, null);
                        txtControl.Text = value.ToString();
                    }
                }
            }
        }

        private void PopulateProperties(Control part, IEnumerable<PropertyInfo> props, Control[] ctrls)
        {
            foreach (PropertyInfo property in props)
            {
                PropertyInfo property1 = property;
                Control control = Array.Find(ctrls, c => c.ID == ID + property1.Name);

                if (control != null)
                {
                    if (control is DropDownList)
                    {
                        var ddControl = (DropDownList) control;

                        property.SetValue(part, Enum.Parse(property.PropertyType, ddControl.SelectedValue), null);
                    }
                    else if (control is CheckBox)
                    {
                        var cbControl = (CheckBox) control;
                        property.SetValue(part, cbControl.Checked, null);
                    }
                    else if (control is TextBox)
                    {
                        var txtControl = (TextBox) control;
                        if (property.PropertyType == typeof (String))
                            property.SetValue(part, txtControl.Text, null);
                        else if (property.PropertyType == typeof (Int32))
                        {
                            int value;
                            if (Int32.TryParse(txtControl.Text, out value))
                                property.SetValue(part, value, null);
                            else
                                throw new FormatException(String.Format(Lang.Trans("{0} is not an integer value"),
                                                                        txtControl.Text));
                        }
                        else
                            throw new NotImplementedException(String.Format("Type {0} is not implemented",
                                                                            property.PropertyType));
                    }
                }
            }
        }
    }

    public delegate bool BoolDelegate();

    public enum WebPartZone
    {
        HomePageLeftZone,
        HomePageRightZone
    }

    public class WebPartInfo
    {
        public BoolDelegate RequirementsMet;
        public string Name { get; set; }
        public string Description { get; set; }

        public string ThumbnailURL
        {
            get
            {
                int slashIndex = ControlPath.LastIndexOf('/');
                int dotIndex = ControlPath.LastIndexOf('.');
                int length = dotIndex - slashIndex;
                string thumbnailName = ControlPath.Substring(slashIndex, length);

                return
                    ((Page) HttpContext.Current.CurrentHandler).ResolveClientUrl(
                        String.Format("~/Images/{0}.png", thumbnailName));
            }
        }

        public string ControlPath { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVisibleByDefault { get; set; }
        public WebPartZone Zone { get; set; }
    }
}

namespace ezFixUp.CustomWebPartManager
{
    public class CustomWebPartManager : WebPartManager
    {
        public string EditedWebPartID
        {
            get { return ViewState["EditedWebPartID"] as string; }
            set { ViewState["EditedWebPartID"] = value; }
        }

        public void SetDirty()
        {
            SetPersonalizationDirty();
        }

        public List<string> SortedWebPartIDs
        {
            get { return HttpContext.Current.Session["SortedWebPartIDs"] as List<string>; }
            set { HttpContext.Current.Session["SortedWebPartIDs"] = value; }
        }

        public void ReOpenClosedWebParts()
        {
            bool partReopened = false;

            var webparts = new List<WebPart>();
            foreach (WebPart webpart in WebParts)
            {
                webparts.Add(webpart);
            }

            //webparts.Sort((w1,w2) => w1.ZoneIndex - w2.ZoneIndex);

            if (SortedWebPartIDs != null)
            {
                for (int i = SortedWebPartIDs.Count - 1; i >= 0; --i)
                {
                    WebPart part = webparts.Find(p => p.ID == SortedWebPartIDs[i]);
                    if (part != null && part.IsClosed)
                    {
                        AddWebPart(part, part.Zone, part.ZoneIndex);
                        partReopened = true;
                    }
                }

                SortedWebPartIDs = null;
            }
            else
            {
                for (int i = webparts.Count - 1; i >= 0; --i)
                {
                    if (webparts[i].IsClosed)
                    {
                        AddWebPart(webparts[i], webparts[i].Zone, webparts[i].ZoneIndex);
                        partReopened = true;
                    }
                }
            }

            if (partReopened)
                SetPersonalizationDirty();
        }

        public void AddWebPartUserControl(WebPartInfo info, WebPartZone zone, int zoneIndex)
        {
            int titleLength = (info.Zone == Data.WebPartZone.HomePageLeftZone) ? 13 : 25;
            var uc = (WebPartUserControl) zone.Page.LoadControl(info.ControlPath);
            uc.DataBind();
            uc.ID = info.Name;
            GenericWebPart part = CreateWebPart(uc);
            part.Title = info.Name.Translate();
            if (part.Title.Length > titleLength) part.Title = part.Title.Substring(0, titleLength) + "..";
            part.AllowClose = true;
            part.AllowEdit = true;
            part.AllowHide = true;
            part.AllowMinimize = true;
            part.AllowZoneChange = true;
            AddWebPart(part, zone, zoneIndex);
            part.DataBind();
        }

        protected override WebPartPersonalization CreatePersonalization()
        {
            return new CustomWebPartPersonalization(this);
        }
    }

    public class CustomWebPartPersonalization : WebPartPersonalization
    {
        public CustomWebPartPersonalization(WebPartManager owner) : base(owner)
        {
        }

        protected override IDictionary UserCapabilities
        {
            get
            {
                //return base.UserCapabilities;
                return PersonalizationAdministration.Provider.DetermineUserCapabilities(WebPartManager);
            }
        }
    }
}

namespace ezFixUp.CustomWebPartZone
{
    public class CustomWebPartZone : WebPartZone
    {
        protected override bool HasHeader
        {
            get { return false; // return base.HasHeader;
            }
        }

        protected override bool HasFooter
        {
            get { return false; // base.HasFooter;
            }
        }

        public new void CloseWebPart(WebPart part)
        {
            base.CloseWebPart(part);
        }

        protected override WebPartChrome CreateWebPartChrome()
        {
            WebPartChrome theChrome = new CustomWebPartChrome(this,
                                                              WebPartManager);

            return theChrome;
        }

        //Prevents rendering white spaces in edit mode (must be revised when drag&drop is added in next versions)
        protected override void RenderDropCue(HtmlTextWriter writer)
        {
            //base.RenderDropCue(writer);
        }
    }
}

namespace ezFixUp.CustomEditorZone
{
    public class CustomEditorZone : EditorZone
    {
        protected override void OnSelectedWebPartChanged(object sender, WebPartEventArgs e)
        {
            base.OnSelectedWebPartChanged(sender, e);

            if (e.WebPart == null && WebPartManager.DisplayMode == WebPartManager.EditDisplayMode)
            {
                ((CustomWebPartManager.CustomWebPartManager) WebPartManager).ReOpenClosedWebParts();
                WebPartManager.DisplayMode = WebPartManager.BrowseDisplayMode;
            }
        }
    }
}
