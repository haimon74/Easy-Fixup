/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditTemplates.
    /// </summary>
    public partial class EditTemplates : AdminPageBase
    {
        protected MessageBox MessageBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
                loadLanguages();
            }

            loadTemplate();
        }

        private void loadLanguages()
        {
            foreach (Language language in Language.FetchAll())
            {
                if (!language.Active) continue;
                ddLanguage.Items.Add(
                    new ListItem(language.Name, language.Id.ToString()));
            }

            if (ddLanguage.Items.Count <= 2)
            {
                if (ddLanguage.Items.Count == 2)
                    ddLanguage.SelectedIndex = 1;
                trLanguage.Visible = false;
                ddLanguage_SelectedIndexChanged(this, null);
            }
            else
            {
                trLanguage.Visible = true;
                trTemplateName.Visible = false;
            }
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        private void ApplyTableProperties(Table table)
        {
            /*table.BorderColor = Color.Black;
            table.BorderWidth = 1;
            table.BorderStyle = BorderStyle.Solid;*/
            table.CellSpacing = 0;
            table.CellPadding = 0;
            table.CssClass = "ttable";
        }

        private void ApplyHeaderProperties(TableCell header)
        {
            header.CssClass = "ttable_header";
            header.ColumnSpan = 2;
        }

        private void ApplyDescriptionProperties(TableCell description)
        {
            //description.BorderWidth = 1;
            description.ColumnSpan = 2;
            //description.CssClass = "font_css";
        }

        private void ApplyLableProperties(TableCell labelCell)
        {
            labelCell.Wrap = false;
            //labelCell.Width = new Unit(1, UnitType.Percentage);
            //labelCell.CssClass = "font_css";
        }

        private void LoadAllTemplates()
        {
            phTemplates.Controls.Clear();

            if (ddLanguage.SelectedIndex == 0)
            {
                btnSave.Visible = false;
                return;
            }

            btnSave.Visible = true;
            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);

            phTemplates.Controls.Add(new LiteralControl(
                                         "<div class=\"theader\">Email Templates</div>"));
            LoadTemplateTables(typeof(EmailTemplates), languageId);
            phTemplates.Controls.Add(new LiteralControl(
                                         "<div class=\"theader\">Miscellaneous Templates</div>"));
            LoadTemplateTables(typeof(MiscTemplates), languageId);
        }

        private void SaveAllTemplates()
        {
            if (ddLanguage.SelectedIndex == 0) return;
            int languageId = Convert.ToInt32(ddLanguage.SelectedValue);

            SaveTemplates(typeof(EmailTemplates), languageId);
            SaveTemplates(typeof(MiscTemplates), languageId);
            MessageBox.Show(Lang.TransA("Settings have been successfully updated!"), Misc.MessageType.Success);
        }

        private void LoadTemplateTables(Type type, int languageId)
        {
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageId);

            Reflection.PropertyData lastItem = null;
            Table tblTemplate = null;

            foreach (Reflection.PropertyData item in items)
            {
                //header
                if (lastItem == null || lastItem.ClassDesc != item.ClassDesc)
                {
                    if (lastItem != null)
                    {
                        //add previous email template
                        phTemplates.Controls.Add(tblTemplate);
                        phTemplates.Controls.Add(new LiteralControl("<div class=\"tseparator1\" />"));
                    }

                    tblTemplate = new Table();
                    ApplyTableProperties(tblTemplate);

                    TableRow headerRow = new TableRow();
                    TableCell header = new TableCell();
                    ApplyHeaderProperties(header);
                    header.Text = item.ClassDesc;
                    headerRow.Cells.Add(header);
                    tblTemplate.Rows.Add(headerRow);
                }

                //description
                if (item.PropertyName == "Description")
                {
                    TableRow descRow = new TableRow();
                    TableCell description = new TableCell();
                    ApplyDescriptionProperties(description);
                    description.Text = (string)item.Value;
                    descRow.Cells.Add(description);
                    descRow.CssClass = "tdescription";
                    tblTemplate.Rows.Add(descRow);
                }
                else
                //all other items of the templates
                {
                    TableRow itemRow = new TableRow();
                    TableCell labelCell = new TableCell();
                    ApplyLableProperties(labelCell);
                    labelCell.Text = item.PropertyDesc;
                    labelCell.CssClass = "tlabel_cell";
                    itemRow.Cells.Add(labelCell);

                    TableCell valueCell = new TableCell();

                    Control control = CreateControl(item);
                    valueCell.Controls.Add(control);

                    valueCell.CssClass = "tvalue_cell";
                    itemRow.Cells.Add(valueCell);
                    tblTemplate.Rows.Add(itemRow);
                }
                lastItem = item;
            }
            //add the last one
            phTemplates.Controls.Add(tblTemplate);
        }

        private Control CreateControl(Reflection.PropertyData item)
        {
            //Creating control from its type
            Control control;
            //if control type is specified create it
            if (item.ControlType != null)
            {
                try
                {
                    control = (Control)Activator.CreateInstance(item.ControlType, null);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException(
                        String.Format(
                            "The ControlAttribute for property named {0} must specify descedant of System.Web.UI.Control class",
                            item.PropertyName));
                }

                //applying the value of the control
                control.GetType().GetProperty(item.ControlProperty).SetValue(control, item.Value, null);
            }
            else //otherwise create TextBox
            {
                control = new TextBox();
                (control as TextBox).Text = item.Value.ToString();
            }

            Hashtable propertiesToApply = item.PropertiesToApply;
            foreach (object key in propertiesToApply.Keys)
            {
                PropertyInfo propertyInfo = control.GetType().GetProperty((string)key);
                object _value = propertiesToApply[key];

                try
                {
                    if (propertyInfo.GetValue(control, null).GetType() == typeof(Unit))
                    {
                        if (_value is String)
                            _value = new Unit((string)_value);
                        else if (_value is Int32)
                            _value = new Unit((int)_value);
                        else if (_value is Double)
                            _value = new Unit((double)_value);
                    }

                    propertyInfo.SetValue(control, _value, null);
                }
                catch
                {
                    throw new Exception(String.Format("Invalid Property/Value pair ({0} / {1}) for control of type {2}",
                                                      key.ToString(), propertiesToApply[key],
                                                      control.GetType().ToString()));
                }
            }

            control.ID = item.ID.Replace('+', '_');
            control.EnableViewState = false;
            return control;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.templates;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        private Control FindControl(Control control, string id)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl.ID == id)
                    return ctrl;

                Control ctr = FindControl(ctrl, id);
                if (ctr != null)
                    return ctr;
            }

            return null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            //            SaveAllTemplates();
            saveTemplate(Convert.ToInt32(ddLanguage.SelectedValue), ddTemplateName.SelectedValue.Split('+')[1]);
        }

        private void SaveTemplates(Type parentType, int languageId)
        {
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(parentType, languageId);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.PropertyName == "Description")
                    continue;

                Control control = FindControl(phTemplates, item.ID);

                if (control == null)
                    throw new Exception("No Control with such ID!");

                if (item.ControlType != null)
                    item.Value = control.GetType().GetProperty(item.ControlProperty).GetValue(control, null);
                else
                {
                    TextBox txtValue = (TextBox)control;
                    if (item.Value is String)
                        item.Value = txtValue.Text;
                    else if (item.Value is Int32)
                        item.Value = Convert.ToInt32(txtValue.Text);
                    else if (item.Value is Double)
                        item.Value = Convert.ToDouble(txtValue.Text);
                }
            }

            Reflection.SavePropertiesData(parentType, items, languageId);
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            phTemplate.Visible = false;
            btnSave.Visible = false;

            if (ddLanguage.SelectedItem.Value == String.Empty)
            {
                trTemplateName.Visible = false;
            }
            else
            {
                int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
                trTemplateName.Visible = true;
                populateDDTemplateName(languageID);
            }
        }

        protected void ddTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddTemplateName.SelectedValue == "-1")
            {
                phTemplate.Visible = false;
                btnSave.Visible = false;
                return;
            }
            int languageID;
            if (Int32.TryParse(ddLanguage.SelectedValue, out languageID))
            {
                phTemplate.Visible = true;
                btnSave.Visible = true;
                string id = ddTemplateName.SelectedValue;
                //                loadTemplate(languageID, id);    
                loadTemplate();
            }
        }

        private void populateDDTemplateName(int languageID)
        {
            ddTemplateName.Items.Clear();

            ddTemplateName.Items.Add(new ListItem("", "-1"));

            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(typeof(EmailTemplates), languageID);
            populateItems(items);
            items = Reflection.GetPropertiesData(typeof(MiscTemplates), languageID);
            populateItems(items);
        }

        private void populateItems(Reflection.PropertyData[] items)
        {
            foreach (Reflection.PropertyData item in items)
            {
                if (ddTemplateName.Items.FindByText(item.ClassDesc) != null) continue;
                ddTemplateName.Items.Add(new ListItem(item.ClassDesc, item.ID.Split('+')[0] + "+" + item.ClassDesc));
            }
        }

        private void loadTemplate()
        {
            phTemplate.Controls.Clear();

            if (ddLanguage.SelectedIndex == 0 || ddTemplateName.SelectedIndex == -1 || ddTemplateName.SelectedIndex == 0)
            {
                return;
            }

            loadTemplate(Convert.ToInt32(ddLanguage.SelectedValue), ddTemplateName.SelectedValue);
        }
        private void loadTemplate(int languageID, string id)
        {
            Table tblTemplate = null;
            Type type = Type.GetType(id.Split('+')[0]);
            string classDesc = id.Split('+')[1];
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageID);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.ClassDesc != classDesc) continue;

                #region create table

                if (tblTemplate == null)
                {
                    tblTemplate = new Table();
                    ApplyTableProperties(tblTemplate);

                    TableRow headerRow = new TableRow();
                    TableCell header = new TableCell();
                    ApplyHeaderProperties(header);
                    header.Text = item.ClassDesc;
                    headerRow.Cells.Add(header);
                    tblTemplate.Rows.Add(headerRow);
                }

                //description
                if (item.PropertyName == "Description")
                {
                    TableRow descRow = new TableRow();
                    TableCell description = new TableCell();
                    ApplyDescriptionProperties(description);
                    description.Text = (string)item.Value;
                    descRow.Cells.Add(description);
                    descRow.CssClass = "tdescription";
                    tblTemplate.Rows.Add(descRow);
                }
                else
                //all other items of the templates
                {
                    TableRow itemRow = new TableRow();
                    TableCell labelCell = new TableCell();
                    ApplyLableProperties(labelCell);
                    labelCell.Text = item.PropertyDesc;
                    labelCell.CssClass = "tlabel_cell";
                    itemRow.Cells.Add(labelCell);

                    TableCell valueCell = new TableCell();

                    Control control = CreateControl(item);
                    valueCell.Controls.Add(control);

                    valueCell.CssClass = "tvalue_cell";
                    itemRow.Cells.Add(valueCell);
                    tblTemplate.Rows.Add(itemRow);
                }

                #endregion
            }

            phTemplate.Controls.Add(tblTemplate);
            //upTemplate.Update();
            phTemplates.Visible = false;
        }

        private void saveTemplate(int languageID, string classDesc)
        {
            Type type = Type.GetType(ddTemplateName.SelectedValue.Split('+')[0]);
            Reflection.PropertyData[] items =
                Reflection.GetPropertiesData(type, languageID);

            foreach (Reflection.PropertyData item in items)
            {
                if (item.ClassDesc != classDesc) continue;
                if (item.PropertyName == "Description")
                    continue;

                Control control = FindControl(phTemplate, item.ID.Replace("+", "_"));

                if (control == null)
                    throw new Exception("No Control with such ID!");

                if (item.ControlType != null)
                    item.Value = control.GetType().GetProperty(item.ControlProperty).GetValue(control, null);
                else
                {
                    TextBox txtValue = (TextBox)control;
                    if (item.Value is String)
                        item.Value = txtValue.Text;
                    else if (item.Value is Int32)
                        item.Value = Convert.ToInt32(txtValue.Text);
                    else if (item.Value is Double)
                        item.Value = Convert.ToDouble(txtValue.Text);
                }
            }

            Reflection.SavePropertiesData(type, items, languageID);

            MessageBox.Show(Lang.TransA("Template have been successfully updated!"), Misc.MessageType.Success);
        }
    }
}