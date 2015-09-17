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
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Linq;

namespace AspNetDating.Classes
{
    /// <summary>
    /// Summary description for Reflection.
    /// </summary>
    public class Reflection
    {
        //used as parameter in GetProperties method for classes that not implement ITemplate
        private const int NO_LANGUAGE = -1;

        /// <summary>
        /// The property data
        /// </summary>
        public class PropertyData
        {
            /// <summary>
            /// The class description
            /// </summary>
            public string ClassDesc;
            /// <summary>
            /// The property name
            /// </summary>
            public string PropertyName;
            /// <summary>
            /// The property description
            /// </summary>
            public string PropertyDesc;
            /// <summary>
            /// The property hint
            /// </summary>
            public string PropertyHint;
            /// <summary>
            /// The ID
            /// </summary>
            public string ID;
            /// <summary>
            /// The value
            /// </summary>
            public object Value;
            /// <summary>
            /// The control type
            /// </summary>
            public Type ControlType;
            /// <summary>
            /// The control property
            /// </summary>
            public string ControlProperty;
            /// <summary>
            /// Contains a list of properties to be applied
            /// </summary>
            public Hashtable PropertiesToApply;
        }

        public class OptionData
        {
            public string ID;
            public string Name;
            public string Description;
            public object Value;

        }

        public enum OptionAvailability
        {
            NotAllowed,
            Allowed,
            AllowedWithCredits
        }

        /// <summary>
        /// This class defines the "Description" attribute
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
        public class DescriptionAttribute : Attribute
        {
            private string description;

            /// <summary>
            /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
            /// </summary>
            /// <param name="description">The description.</param>
            public DescriptionAttribute(string description)
            {
                this.description = description;
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            public string Description
            {
                get { return description; }
            }
        }

        /// <summary>
        /// This class defines the "Hint" attribute
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class HintAttribute : Attribute
        {
            private string hint;

            // Constructor  
            /// <summary>
            /// Initializes a new instance of the <see cref="HintAttribute"/> class.
            /// </summary>
            /// <param name="hint">The hint.</param>
            public HintAttribute(string hint)
            {
                this.hint = hint;
            }

            /// <summary>
            /// Gets the hint.
            /// </summary>
            /// <value>The hint.</value>
            public string Hint
            {
                get { return hint; }
            }
        }

        /// <summary>
        /// This class defines the "Control" attribute
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class ControlAttribute : Attribute
        {
            private string propertyName;
            private Type controlType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ControlAttribute"/> class.
            /// </summary>
            /// <param name="controlType">Type of the control.</param>
            /// <param name="propertyName">Name of the property.</param>
            public ControlAttribute(Type controlType, string propertyName)
            {
                this.controlType = controlType;
                this.propertyName = propertyName;
            }

            /// <summary>
            /// Gets the name of the property.
            /// </summary>
            /// <value>The name of the property.</value>
            public string PropertyName
            {
                get { return propertyName; }
            }

            public Type ControlType
            {
                get { return controlType; }
            }
        }

        /// <summary>
        /// This class handles defines the "Property" attribute
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
        public class PropertyAttribute : Attribute
        {
            private readonly string propertyName;
            private readonly object _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="_value">The _value.</param>
            public PropertyAttribute(string propertyName, object _value)
            {
                this.propertyName = propertyName;
                this._value = _value;
            }

            /// <summary>
            /// Gets the name of the property.
            /// </summary>
            /// <value>The name of the property.</value>
            public string PropertyName
            {
                get { return propertyName; }
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <value>The value.</value>
            public object Value
            {
                get { return _value; }
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class AllowCreditsAttribute : Attribute
        {
        }

        /// <summary>
        /// Strings to enum.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        public static object StringToEnum(Type t, string Value)
        {
            foreach (FieldInfo fi in t.GetFields())
                if (fi.Name == Value)
                    return fi.GetValue(null);

            throw new Exception(String.Format(
                                    "Can't convert {0} to {1}", Value,
                                    t.ToString()));
        }

        /// <summary>
        /// Gets the enum elements description.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static string[] GetEnumElementsDescription(Enum e)
        {
            List<string> lEnumElementsDescription = new List<string>();

            bool first = true;

            foreach (FieldInfo fieldInfo in e.GetType().GetFields())
            {
                // we don't need the first one because it is not an enum element
                if (first)
                {
                    first = false;
                    continue;
                }

                lEnumElementsDescription.Add(GetDescriptionAttribute(fieldInfo));
            }

            return lEnumElementsDescription.ToArray();
        }

        private static string GetDescriptionAttribute(MemberInfo memberInfo)
        {
            Attribute att = Attribute.GetCustomAttribute(memberInfo, typeof (DescriptionAttribute), false);
            if (att != null)
                return Lang.TransA(((DescriptionAttribute) att).Description);
            else return null;
        }

        private static bool GetAllowCreditsAttribute(MemberInfo memberInfo)
        {
            Attribute att = Attribute.GetCustomAttribute(memberInfo, typeof (AllowCreditsAttribute), false);
            return att != null;
        }

        private static Hashtable GetPropertiesToApply(MemberInfo memberInfo)
        {
            Attribute[] attributes =
                Attribute.GetCustomAttributes(memberInfo, typeof (PropertyAttribute));

            Hashtable hashtable = new Hashtable();
            foreach (Attribute attribute in attributes)
            {
                string propertyType = ((PropertyAttribute) attribute).PropertyName;
                object _value = ((PropertyAttribute) attribute).Value;

                hashtable.Add(propertyType, _value);
            }

            return hashtable;
        }

        private static string GetHintAttribute(MemberInfo memberInfo)
        {
            Attribute att = Attribute.GetCustomAttribute(memberInfo, typeof (HintAttribute), false);
            if (att != null)
                return Lang.TransA(((HintAttribute) att).Hint);
            else return String.Empty;
        }

        private static void GetControlInfo(MemberInfo memberInfo, out Type type, out string property)
        {
            type = null;
            property = null;

            Attribute att = Attribute.GetCustomAttribute(memberInfo, typeof (ControlAttribute));
            if (att != null)
            {
                type = ((ControlAttribute) att).ControlType;
                property = ((ControlAttribute) att).PropertyName;
            }
        }

        /// <summary>
        /// Saves the properties data.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="items">The items.</param>
        /// <param name="languageId">The language id.</param>
        /// <returns></returns>
        public static object SavePropertiesData(Type classType, PropertyData[] items, int languageId)
        {
            PropertyInfo[] propertyInfoArray = classType.GetProperties(
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

            object obj = null;
            if (classType.IsClass)
            {
                if (languageId == NO_LANGUAGE)
                    obj = Activator.CreateInstance(classType);
                else
                {
                    try
                    {
                        obj = Activator.CreateInstance(classType, new object[] {languageId});
                    }
                    catch (MissingMethodException)
                    {
                        obj = Activator.CreateInstance(classType);
                    }
                }
            }

            foreach (PropertyInfo propertyInfo in propertyInfoArray)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    string propertyDesc = GetDescriptionAttribute(propertyInfo);
                    if (propertyDesc == null)
                        continue;

                    string id = classType.FullName + propertyInfo.Name;

                    foreach (PropertyData item in items)
                    {
                        if (id == item.ID)
                        {
                            propertyInfo.SetValue(obj, item.Value, null);
                            break;
                        }
                    }
                }
            }

            Type[] nestedTypes = classType.GetNestedTypes(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (Type nestedType in nestedTypes)
            {
                SavePropertiesData(nestedType, items, languageId);
            }

            return obj;
        }

        /// <summary>
        /// Gets the properties data.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="languageId">The language id.</param>
        /// <returns></returns>
        public static PropertyData[] GetPropertiesData(Type classType, int languageId)
        {
            return GetPropertiesData(classType, null, languageId);
        }

        /// <summary>
        /// Gets the properties data from object.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="languageId">The language id.</param>
        /// <returns></returns>
        public static PropertyData[] GetPropertiesDataFromObject(object objectInstance, int languageId)
        {
            return GetPropertiesData(objectInstance.GetType(), objectInstance, languageId);
        }

        private static PropertyData[] GetPropertiesData(Type classType, object objectInstance, int languageId)
        {
            List<PropertyData> lSettingsItems = new List<PropertyData>();

            PropertyInfo[] propertyInfoArray = classType.GetProperties(
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

            if (!classType.IsAbstract)
            {
                object obj = null;
                if (objectInstance == null)
                {
                    if (classType.IsClass)
                    {
                        if (languageId == NO_LANGUAGE)
                            obj = Activator.CreateInstance(classType);
                        else
                        {
                            try
                            {
                                obj = Activator.CreateInstance(classType, new object[] { languageId });
                            }
                            catch (MissingMethodException)
                            {
                                obj = Activator.CreateInstance(classType);
                            }
                        }
                    }
                }
                else obj = objectInstance;

                foreach (PropertyInfo propertyInfo in propertyInfoArray)
                {
                    string propertyDesc = GetDescriptionAttribute(propertyInfo);
                    if (propertyDesc == null)
                        continue;

                    string propertyHint = GetHintAttribute(propertyInfo);

                    string classDesc = GetDescriptionAttribute(classType);
                    if (classDesc == null)
                        throw new NoAttributeFoundException(
                            "The parent class should also have description attribute!");

                    Hashtable propertiesToApply = GetPropertiesToApply(propertyInfo);

                    Type controlType;
                    string controlProperty;

                    GetControlInfo(propertyInfo, out controlType, out controlProperty);

                    PropertyData item = new PropertyData();
                    item.ClassDesc = classDesc;
                    item.PropertyName = propertyInfo.Name;
                    item.PropertyDesc = propertyDesc;
                    item.PropertyHint = propertyHint;
                    item.Value = propertyInfo.GetValue(obj, null);
                    item.ID = classType.FullName + propertyInfo.Name;
                    item.ControlType = controlType;
                    item.ControlProperty = controlProperty;
                    item.PropertiesToApply = propertiesToApply;
                    lSettingsItems.Add(item);
                }

                Type[] nestedTypes = classType.GetNestedTypes(
                    BindingFlags.Public | BindingFlags.Instance);

                foreach (Type nestedType in nestedTypes)
                {
                    lSettingsItems.AddRange(GetPropertiesData(nestedType, languageId));
                }
            }
            return lSettingsItems.ToArray();
        }

        /// <summary>
        /// Generates the settings table.
        /// </summary>
        /// <param name="phSettings">The ph settings.</param>
        /// <param name="SettingsClassType">Type of the settings class.</param>
        public static void GenerateSettingsTable(PlaceHolder phSettings, Type SettingsClassType)
        {
            GenerateSettingsTable(phSettings, SettingsClassType, null);
        }

        /// <summary>
        /// Generates the settings table from object.
        /// </summary>
        /// <param name="phSettings">The ph settings.</param>
        /// <param name="objectInstance">The object instance.</param>
        public static void GenerateSettingsTableFromObject(PlaceHolder phSettings, object objectInstance)
        {
            GenerateSettingsTable(phSettings, objectInstance.GetType(), objectInstance);
        }

        private static void GenerateSettingsTable(PlaceHolder phSettings, Type SettingsClassType, object objectInstance)
        {
            PropertyData[] items = objectInstance == null?
                GetPropertiesData(SettingsClassType, NO_LANGUAGE) : GetPropertiesDataFromObject(objectInstance, NO_LANGUAGE);

            Table tblSettings = new Table();
            tblSettings.CellPadding = 0;
            tblSettings.CellSpacing = 0;
            tblSettings.CssClass = "stable";
            PropertyData lastItem = null;

            int itemNum = 0;
            foreach (PropertyData item in items)
            {
                if (lastItem == null || lastItem.ClassDesc != item.ClassDesc)
                {
                    if (lastItem != null)
                    {
                        TableRow emptyRow = new TableRow();
                        emptyRow.CssClass = "empty_row";
                        emptyRow.Cells.Add(new TableCell());
                        tblSettings.Rows.Add(emptyRow);
                    }
                    
                    TableRow headerRow = new TableRow();
                    TableCell header = new TableCell();
                    ApplyHeaderProperties(header);
                    header.Text = item.ClassDesc;
                    headerRow.Cells.Add(header);
                    tblSettings.Rows.Add(headerRow);
                }

                TableRow itemRow = new TableRow();
                if (itemNum%2 != 0)
                    itemRow.CssClass = "even_row";
                TableCell descCell = new TableCell();
                descCell.CssClass = "slabel";
                descCell.Text = item.PropertyDesc;
                itemRow.Cells.Add(descCell);

                TableCell valueCell = new TableCell();
                valueCell.CssClass = "svalue";

                TableCell hintCell = null;
                if (item.PropertyHint != String.Empty)
                {
                    hintCell = new TableCell();
                    hintCell.CssClass = "shint";
                    HtmlAnchor anchor = new HtmlAnchor();
                    anchor.HRef = "#";
                    anchor.Attributes.Add("class", "hintanchor");
                    string onMouseOverValue = String.Format("showhint('{0}', this, event, '150px')", item.PropertyHint);
                    anchor.Attributes.Add("onMouseover", onMouseOverValue);
                    anchor.InnerHtml = "<img src='Images/help_icon.gif' border=0 />";
                    hintCell.Controls.Add(anchor);
                }

                if (item.Value is Enum)
                {
                    string[] fields = GetEnumElementsDescription((Enum) item.Value);
                    Array values = Enum.GetValues(item.Value.GetType());
                    DropDownList dropEnums = new DropDownList();
                    dropEnums.ID = item.ID;
                    //dropEnums.Font.Name = "Arial";
                    //dropEnums.Font.Size = 9;

                    for (int i = 0; i < fields.Length; ++i)
                    {
                        dropEnums.Items.Add(new ListItem(fields[i], values.GetValue(i).ToString()));
                    }

                    dropEnums.SelectedValue = item.Value.ToString();
                    valueCell.Controls.Add(dropEnums);
                }
                else if (item.Value is Boolean)
                {
                    CheckBox chkValue = new CheckBox();
                    chkValue.ID = item.ID;
                    chkValue.Checked = (bool) item.Value;
                    valueCell.Controls.Add(chkValue);
                }
                else if (item.Value is String || item.Value is Int32 ||
                         item.Value is Double || item.Value is Decimal)
                {
                    TextBox txtValue = new TextBox();
                    txtValue.ID = item.ID;
                    txtValue.Text = item.Value.ToString();
                    //txtValue.Font.Name = "Arial";
                    //txtValue.Font.Size = 9;
                    valueCell.Controls.Add(txtValue);
                }
                else if (item.Value is string[])
                {
                    TextBox txtValues = new TextBox();
                    txtValues.ID = item.ID;
                    txtValues.Text = StringArrayToCommaDelimitedString((string[]) item.Value);
                    //txtValues.Font.Name = "Arial";
                    //txtValues.Font.Size = 9;
                    txtValues.TextMode = TextBoxMode.MultiLine;
                    txtValues.Columns = 40;
                    txtValues.Rows = 4;
                    valueCell.Controls.Add(txtValues);
                }
                else if (item.Value is Hashtable)
                {
                    TextBox txtValues = new TextBox();
                    txtValues.ID = item.ID;
                    txtValues.Text = HashtableToString((Hashtable) item.Value);
                    //txtValues.Font.Name = "Arial";
                    //txtValues.Font.Size = 9;
                    txtValues.TextMode = TextBoxMode.MultiLine;
                    txtValues.Columns = 80;
                    txtValues.Rows = 8;
                    valueCell.Controls.Add(txtValues);
                }
                else if (item.Value is Color)
                {
                    TextBox txtValue = new TextBox();
                    txtValue.ID = item.ID;
                    txtValue.Text = ColorToString((Color) item.Value, true);
                    //txtValue.Font.Name = "Arial";
                    //txtValue.Font.Size = 9;
                    valueCell.Controls.Add(txtValue);
                }
                else throw new Exception("Setting type not implemented!");

                itemRow.Cells.Add(valueCell);

                if (hintCell != null)
                    itemRow.Cells.Add(hintCell);

                tblSettings.Rows.Add(itemRow);

                lastItem = item;
                ++itemNum;
            }

            phSettings.Controls.Add(tblSettings);
        }

        /// <summary>
        /// Saves the table settings.
        /// </summary>
        /// <param name="phSettings">The ph settings.</param>
        /// <param name="SettingsClassType">Type of the settings class.</param>
        /// <returns></returns>
        public static object SaveTableSettings(PlaceHolder phSettings, Type SettingsClassType)
        {
            PropertyData[] items =
                GetPropertiesData(SettingsClassType, NO_LANGUAGE);

            foreach (PropertyData item in items)
            {
                Control control = FindControl(phSettings, item.ID);

                if (control == null)
                    throw new Exception("No Control with such ID!");

                if (item.Value is Enum)
                {
                    string val = ((DropDownList) control).SelectedItem.Value;
                    item.Value = StringToEnum(item.Value.GetType(), val);
                }
                else if (item.Value is Boolean)
                {
                    item.Value = ((CheckBox) control).Checked;
                }
                else if (item.Value is String || item.Value is Int32 ||
                         item.Value is Double || item.Value is Decimal)
                {
                    TextBox txtValue = (TextBox) control;

                    if (item.Value is String)
                        item.Value = txtValue.Text;
                    else if (item.Value is Int32)
                    {
                        try
                        {
                            item.Value = Convert.ToInt32(txtValue.Text.Trim());
                        }
                        catch (FormatException) {
                            item.Value = 0; }
                    }
                    else if (item.Value is Double)
                    {
                        try
                        {
                            item.Value = Convert.ToDouble(txtValue.Text.Trim());
                        }
                        catch (FormatException) {
                            item.Value = 0.0; }

                    }
                    else if (item.Value is Decimal)
                    {
                        try
                        {
                            item.Value = Convert.ToDecimal(txtValue.Text.Trim());
                        }
                        catch (FormatException)
                        {
                            item.Value = 0.0M;
                        }

                    }
                }
                else if (item.Value is string[])
                {
                    TextBox txtValues = (TextBox) control;

                    item.Value = txtValues.Text.Split(',');
                }
                else if (item.Value is Hashtable)
                {
                    TextBox txtValues = (TextBox) control;

                    item.Value = StringToHashtable(txtValues.Text);
                }
                else if (item.Value is Color)
                {
                    TextBox txtValue = (TextBox) control;
                    item.Value = StringToColor(txtValue.Text);
                }
                else throw new Exception("Setting type not implemented!");
            }

            return SavePropertiesData(SettingsClassType, items, NO_LANGUAGE);
        }

        public static void UpdateBillingPlanOptionsFromUI(PlaceHolder phPlans, BillingPlanOptions[] planOptions)
        {
            Type billingPlanOptionsType = typeof(BillingPlanOptions);

            PropertyInfo[] propertyInfoArray = billingPlanOptionsType.GetProperties();

            foreach (var options in planOptions)
            {
                options.ContainsOptionWithEnabledCredits = false;
            }

            foreach (var propertyInfo in propertyInfoArray)
            {
                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(BillingPlanOption<>))
                {
                    List<object> previousOptions = new List<object>();

                    foreach (var options in planOptions)
                    {
                        var option = propertyInfo.GetValue(options, null);
                        string optionID = (string)option.GetType().GetField("ID").GetValue(option);

                        object optionValue = option.GetType().GetField("Value").GetValue(option);
                        Control control = FindControl(phPlans, optionID);

                        if (control == null)
                            throw new Exception("No Control with such ID!");

                        if (optionValue is Enum)
                        {
                            string val = ((DropDownList)control).SelectedItem.Value;
                            optionValue = StringToEnum(optionValue.GetType(), val);
                        }
                        else if (optionValue is Boolean)
                        {
                            OptionAvailability availability = (OptionAvailability) Int32.Parse(((DropDownList)control).SelectedValue);

                            optionValue = false;
                            option.GetType().GetField("EnableCreditsPayment").SetValue(option, false);

                            switch (availability)
                            {
                                case OptionAvailability.Allowed:
                                    optionValue = true;
                                    break;
                                case OptionAvailability.NotAllowed:
                                    optionValue = false;
                                    break;
                                case OptionAvailability.AllowedWithCredits:
                                    option.GetType().GetField("EnableCreditsPayment").SetValue(option, true);
                                    options.ContainsOptionWithEnabledCredits = true;
                                    break;
                            }

                            //optionValue = ((CheckBox)control).Checked;
                        }
                        else if (optionValue is String || optionValue is Int32 ||
                                 optionValue is Double || optionValue is Decimal)
                        {
                            TextBox txtValue = (TextBox)control;

                            if (optionValue is String)
                                optionValue = txtValue.Text;
                            else if (optionValue is Int32)
                            {
                                try
                                {
                                    optionValue = Convert.ToInt32(txtValue.Text.Trim());
                                }
                                catch (FormatException)
                                {
                                    optionValue = 0;
                                }
                            }
                            else if (optionValue is Double)
                            {
                                try
                                {
                                    optionValue = Convert.ToDouble(txtValue.Text.Trim());
                                }
                                catch (FormatException)
                                {
                                    optionValue = 0.0;
                                }

                            }
                            else if (optionValue is Decimal)
                            {
                                try
                                {
                                    optionValue = Convert.ToDecimal(txtValue.Text.Trim());
                                }
                                catch (FormatException)
                                {
                                    optionValue = 0.0M;
                                }

                            }
                        }
                        else if (optionValue is string[])
                        {
                            TextBox txtValues = (TextBox)control;

                            optionValue = txtValues.Text.Split(',');
                        }
                        else if (optionValue is Hashtable)
                        {
                            TextBox txtValues = (TextBox)control;

                            optionValue = StringToHashtable(txtValues.Text);
                        }
                        else if (optionValue is Color)
                        {
                            TextBox txtValue = (TextBox)control;
                            optionValue = StringToColor(txtValue.Text);
                        }
                        else throw new Exception("Setting type not implemented!");

                        option.GetType().GetField("Value").SetValue(option, optionValue);

                        bool allowCredits = (bool)option.GetType().GetField("AllowCredits").GetValue(option);

                        if (allowCredits && !(optionValue is Boolean))
                        {
                            CheckBox cbEnableCredits = FindControl(phPlans, optionID + "EnableCredits") as CheckBox;
                            if (cbEnableCredits != null)
                            {
                                option.GetType().GetField("EnableCreditsPayment").SetValue(option, cbEnableCredits.Checked);
                                if (cbEnableCredits.Checked)
                                    options.ContainsOptionWithEnabledCredits = true;
                            }
                        }

                        if (allowCredits)
                        {
                            TextBox txtCredits = FindControl(phPlans, optionID + "Credits") as TextBox;
                            if (txtCredits != null)
                            {
                                int credits;

                                if (Int32.TryParse(txtCredits.Text.Trim(), out credits))
                                {
                                    option.GetType().GetField("Credits").SetValue(option, credits);
                                }
                            } 
                        }

                        if (optionValue is Boolean)
                        {
                            if (!((bool)optionValue))
                            {
                                previousOptions.Add(option);
                                option.GetType().GetField("UpgradableToNextPlan").SetValue(option, false);
                            }
                            else
                            {
                                foreach (var previousOption in previousOptions)
                                {
                                    previousOption.GetType().GetField("UpgradableToNextPlan").SetValue(previousOption, true);
                                }
                                previousOptions.Clear();
                            }
                        }
                        else if (optionValue is Int32)
                        {
                            previousOptions.Add(option);
                            option.GetType().GetField("UpgradableToNextPlan").SetValue(option, false);
                            bool enableCredits = (bool) option.GetType().GetField("EnableCreditsPayment").GetValue(option);

                            foreach (var previousOption in previousOptions)
                            {
                                if (((BillingPlanOption<int>)previousOption).Value < (int)optionValue || enableCredits)
                                    previousOption.GetType().GetField("UpgradableToNextPlan").SetValue(previousOption, true);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Strings the array to comma delimited string.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static string StringArrayToCommaDelimitedString(string[] items)
        {
            string CommaDelimitedValues = "";
            foreach (string item in items)
                CommaDelimitedValues = CommaDelimitedValues + item + ",";
            return CommaDelimitedValues.Remove(CommaDelimitedValues.LastIndexOf(','), 1);
        }

        /// <summary>
        /// Hashtables to string.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        public static string HashtableToString(Hashtable hash)
        {
            StringBuilder sb = new StringBuilder(null);
            foreach (string key in hash.Keys)
                sb.AppendFormat("<title>{0}</title>\n<body>{1}</body>\n\n", key.ToString(), hash[key].ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Strings to hashtable.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        /// <returns></returns>
        public static Hashtable StringToHashtable(string pairs)
        {
            Hashtable hash = new Hashtable();
            try
            {
                XmlDocument doc = new XmlDocument();
                string xml = String.Format("<root>{0}</root>", pairs);

                doc.LoadXml(xml);
                XmlNodeList titles = doc.GetElementsByTagName("title");
                XmlNodeList bodys = doc.GetElementsByTagName("body");
                if (titles.Count != bodys.Count)
                    throw new FormatException();
                for (int i = 0; i < titles.Count; ++i)
                {
                    hash.Add(titles[i].InnerText, bodys[i].InnerText);
                }
            }
            catch (XmlException)
            {
                throw new FormatException();
            }

            return hash;
        }

        /// <summary>
        /// Converts RGBA string to Color structure
        /// </summary>
        /// <param name="hexValue">string that represents color in a hex RGB format i.e #FF0000(Red)</param>
        /// <returns></returns>
        public static Color StringToColor(string hexValue)
        {
            hexValue = hexValue.Replace("#", "");

            if (hexValue == String.Empty)
                return Color.Empty;

            if (hexValue.Length == 6)
            {
                Color color =
                    Color.FromArgb(
                        Byte.Parse(hexValue.Substring(0, 2), NumberStyles.HexNumber),
                        Byte.Parse(hexValue.Substring(2, 2), NumberStyles.HexNumber),
                        Byte.Parse(hexValue.Substring(4, 2), NumberStyles.HexNumber)
                        );

                return color;
            }
            else throw new FormatException();
        }

        /// <summary>
        /// Colors to string.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="addHexSign">if set to <c>true</c> [add hex sign].</param>
        /// <returns></returns>
        public static string ColorToString(Color color, bool addHexSign)
        {
            if (color == Color.Empty)
                return String.Empty;
            else
            {
                string strColor = ColorTranslator.ToHtml(color);
                if (!addHexSign)
                    return strColor.Remove(0, 1);
                else return strColor;
            }
        }

        private static void ApplyHeaderProperties(TableCell header)
        {
            header.CssClass = "table_header2";
            header.ColumnSpan = 3;
        }

        private static Control FindControl(Control control, string id)
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

        public static void GetBillingPlanOptionsData(BillingPlanOptions billingPlanOptions, int planID)
        {
            Type billingPlanOptionsType = billingPlanOptions.GetType();

            PropertyInfo[] propertyInfoArray = billingPlanOptionsType.GetProperties();

            foreach (var propertyInfo in propertyInfoArray)
            {
                if (propertyInfo.PropertyType.IsGenericType && 
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(BillingPlanOption<>))
                {
                    object billingPlanOption = propertyInfo.GetValue(billingPlanOptions, null);

                    string description = GetDescriptionAttribute(propertyInfo);
                    billingPlanOption.GetType().GetField("PropertyDescription").SetValue(billingPlanOption, description);
                    billingPlanOption.GetType().GetField("ID").SetValue(billingPlanOption, billingPlanOptionsType.FullName + propertyInfo.Name + planID);
                    billingPlanOption.GetType().GetField("PlanID").SetValue(billingPlanOption, planID);

                    bool allowCredits = GetAllowCreditsAttribute(propertyInfo);
                    billingPlanOption.GetType().GetField("AllowCredits").SetValue(billingPlanOption, allowCredits);
                }
            }

            //return optionsData.ToArray();
        }

        public static void GenerateBillingPlanOptionsTable(PlaceHolder phPlans, BillingPlanOptions[] planOptions,Action executeAfterDelete, bool readOnly)
        {
            //PropertyData[] items = objectInstance == null ?
            //    GetPropertiesData(SettingsClassType, NO_LANGUAGE) : GetPropertiesDataFromObject(objectInstance, NO_LANGUAGE);

            Table tblPlans = new Table();
            tblPlans.CellPadding = 0;
            tblPlans.CellSpacing = 0;
            tblPlans.CssClass = "stable";
            //PropertyData lastItem = null;
            int itemNum = 0;

            Type billingPlanOptionsType = typeof(BillingPlanOptions);

            PropertyInfo[] propertyInfoArray = billingPlanOptionsType.GetProperties();

            foreach (var propertyInfo in propertyInfoArray)
            {
                if (propertyInfo.PropertyType.IsGenericType &&
                    propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(BillingPlanOption<>))
                {
                    #region Plan names
                    if (itemNum == 0)
                    {
                        #region PlanInfo
                        TableRow planNameRow = new TableRow();
                        TableCell planNameCell = new TableCell();
                        planNameRow.Cells.Add(planNameCell);
                        planNameCell.Text = Lang.TransA("Plans");
                        planNameCell.CssClass = "table_header";

                        TableRow amountRow = new TableRow();
                        TableCell amountCell = new TableCell();
                        amountRow.Cells.Add(amountCell);
                        amountCell.Text = Lang.TransA("<b>Amount</b>");
                        amountCell.CssClass = "slabel";

                        TableRow cycleRow = new TableRow();
                        TableCell cycleCell = new TableCell();
                        cycleRow.Cells.Add(cycleCell);
                        cycleCell.Text = Lang.TransA("<b>Cycle</b>");
                        cycleCell.CssClass = "slabel";

                        TableRow cycleUnitRow = new TableRow();
                        TableCell cycleUnitCell = new TableCell();
                        cycleUnitRow.Cells.Add(cycleUnitCell);
                        cycleUnitCell.Text = Lang.TransA("<b>CycleUnit</b>");
                        cycleUnitCell.CssClass = "slabel";

                        foreach (var options in planOptions)
                        {
                            planNameCell = new TableCell();
                            planNameCell.CssClass = "table_header";
                            amountCell = new TableCell();
                            cycleCell = new TableCell();
                            cycleUnitCell = new TableCell();

                            var option = propertyInfo.GetValue(options, null);
                            int planID = (int)option.GetType().GetField("PlanID").GetValue(option);
                            planNameRow.Cells.Add(planNameCell);
                            amountRow.Cells.Add(amountCell);
                            cycleRow.Cells.Add(cycleCell);
                            cycleUnitRow.Cells.Add(cycleUnitCell);

                            if (planID == -1)
                            {
                                planNameCell.Text = Lang.TransA("Non-Paying Members");
                                amountCell.Text = String.Empty;
                                cycleCell.Text = String.Empty;
                                cycleUnitCell.Text = String.Empty;
                            }
                            else
                            {
                                TextBox txtPlanName = new TextBox();
                                txtPlanName.ID = "PlanName" + planID;

                                TextBox txtAmount = new TextBox();
                                txtAmount.ID = "Amount" + planID;

                                TextBox txtCycle = new TextBox();
                                txtCycle.ID = "Cycle" + planID;

                                DropDownList ddCycleUnits = new DropDownList();
                                ddCycleUnits.ID = "CycleUnits" + planID;
                                ddCycleUnits.Items.Add(new ListItem(Lang.TransA(CycleUnits.Days.ToString()), ((int)CycleUnits.Days).ToString()));
                                ddCycleUnits.Items.Add(new ListItem(Lang.TransA(CycleUnits.Weeks.ToString()), ((int)CycleUnits.Weeks).ToString()));
                                ddCycleUnits.Items.Add(new ListItem(Lang.TransA(CycleUnits.Months.ToString()), ((int)CycleUnits.Months).ToString()));
                                ddCycleUnits.Items.Add(new ListItem(Lang.TransA(CycleUnits.Years.ToString()), ((int)CycleUnits.Years).ToString()));


                                var plan = BillingPlan.Fetch(planID);
                                txtPlanName.Text = plan.Title;
                                txtAmount.Text = plan.Amount.ToString();
                                txtCycle.Text = plan.Cycle.ToString();
                                ddCycleUnits.SelectedValue = ((int)plan.CycleUnit).ToString();

                                planNameCell.Controls.Add(txtPlanName);
                                amountCell.Controls.Add(txtAmount);
                                cycleCell.Controls.Add(txtCycle);
                                cycleUnitCell.Controls.Add(ddCycleUnits);
                            }
                        }

                        tblPlans.Rows.Add(planNameRow);
                        tblPlans.Rows.Add(amountRow);
                        tblPlans.Rows.Add(cycleRow);
                        tblPlans.Rows.Add(cycleUnitRow);
                        #endregion
                    }
                    //header.Text = item.ClassDesc;
                    //headerRow.Cells.Add(header);
                    //tblSettings.Rows.Add(headerRow);
                    #endregion

                    TableRow itemRow = new TableRow();
                    if (itemNum % 2 != 0)
                        itemRow.CssClass = "even_row";

                    var firstOption = propertyInfo.GetValue(planOptions[0], null);

                    TableCell descCell = new TableCell();
                    descCell.CssClass = "slabel";
                    descCell.Text = firstOption.GetType().GetField("PropertyDescription").GetValue(firstOption).ToString();
                    itemRow.Cells.Add(descCell);

                    foreach (var options in planOptions)
                    {
                        var option = propertyInfo.GetValue(options, null);
                        object optionValue = option.GetType().GetField("Value").GetValue(option);
                        string optionID = (string) option.GetType().GetField("ID").GetValue(option);

                        bool allowCredits = (bool)option.GetType().GetField("AllowCredits").GetValue(option);
                        bool enableCreditsPayment = (bool)option.GetType().GetField("EnableCreditsPayment").GetValue(option);

                        TableCell valueCell = new TableCell();
                        valueCell.CssClass = "svalue";

                        if (optionValue is Enum)
                        {
                            string[] fields = GetEnumElementsDescription((Enum)optionValue);
                            Array values = Enum.GetValues(optionValue.GetType());
                            DropDownList dropEnums = new DropDownList();
                            dropEnums.ID = optionID;
                            //dropEnums.Font.Name = "Arial";
                            //dropEnums.Font.Size = 9;

                            for (int i = 0; i < fields.Length; ++i)
                            {
                                dropEnums.Items.Add(new ListItem(fields[i], values.GetValue(i).ToString()));
                            }

                            dropEnums.SelectedValue = optionValue.ToString();
                            valueCell.Controls.Add(dropEnums);
                        }
                        else if (optionValue is Boolean)
                        {
                            DropDownList ddValue = new DropDownList();
                            ddValue.ID = optionID;

                            ddValue.Items.Add(new ListItem("Not Allowed".TranslateA(), ((int)OptionAvailability.NotAllowed).ToString()));
                            ddValue.Items.Add(new ListItem("Allowed".TranslateA(), ((int)OptionAvailability.Allowed).ToString()));

                            if (allowCredits)
                            {
                                ddValue.Items.Add(new ListItem("Allowed with credits".TranslateA(), ((int)OptionAvailability.AllowedWithCredits).ToString()));
                            }

                            if (enableCreditsPayment)
                            {
                                ddValue.SelectedValue = ((int)OptionAvailability.AllowedWithCredits).ToString();
                            }
                            else if ((bool)optionValue)
                                ddValue.SelectedValue = ((int)OptionAvailability.Allowed).ToString();
                            else ddValue.SelectedValue = ((int)OptionAvailability.NotAllowed).ToString();

                            valueCell.Controls.Add(ddValue);

                            //CheckBox chkValue = new CheckBox();
                            //chkValue.ID = optionID;
                            //chkValue.Checked = (bool)optionValue;
                            //valueCell.Controls.Add(chkValue);
                        }
                        else if (optionValue is String || optionValue is Int32 ||
                                 optionValue is Double || optionValue is Decimal)
                        {
                            TextBox txtValue = new TextBox();
                            txtValue.Columns = 4;
                            txtValue.ID = optionID;
                            txtValue.Text = optionValue.ToString();
                            //txtValue.Font.Name = "Arial";
                            //txtValue.Font.Size = 9;
                            valueCell.Controls.Add(txtValue);
                        }
                        else if (optionValue is string[])
                        {
                            TextBox txtValues = new TextBox();
                            txtValues.ID = optionID;
                            txtValues.Text = StringArrayToCommaDelimitedString((string[])optionValue);
                            //txtValues.Font.Name = "Arial";
                            //txtValues.Font.Size = 9;
                            txtValues.TextMode = TextBoxMode.MultiLine;
                            txtValues.Columns = 40;
                            txtValues.Rows = 4;
                            valueCell.Controls.Add(txtValues);
                        }
                        else if (optionValue is Hashtable)
                        {
                            TextBox txtValues = new TextBox();
                            txtValues.ID = optionID;
                            txtValues.Text = HashtableToString((Hashtable)optionValue);
                            //txtValues.Font.Name = "Arial";
                            //txtValues.Font.Size = 9;
                            txtValues.TextMode = TextBoxMode.MultiLine;
                            txtValues.Columns = 80;
                            txtValues.Rows = 8;
                            valueCell.Controls.Add(txtValues);
                        }
                        else if (optionValue is Color)
                        {
                            TextBox txtValue = new TextBox();
                            txtValue.ID = optionID;
                            txtValue.Text = ColorToString((Color)optionValue, true);
                            //txtValue.Font.Name = "Arial";
                            //txtValue.Font.Size = 9;
                            valueCell.Controls.Add(txtValue);
                        }
                        else throw new Exception("Setting type not implemented!");

                        if (allowCredits && !(optionValue is Boolean))
                        {
                            CheckBox cbEnableCredits = new CheckBox();
                            cbEnableCredits.ID = optionID + "EnableCredits";
                            cbEnableCredits.Checked = enableCreditsPayment;
                            cbEnableCredits.Text = "Use credits".TranslateA();
                            valueCell.Controls.Add(cbEnableCredits);
                        }

                        if (allowCredits)
                        {
                            TextBox txtCredits = new TextBox();
                            int credits = (int)option.GetType().GetField("Credits").GetValue(option);
                            txtCredits.Text = credits.ToString();
                            txtCredits.ID = optionID + "Credits";
                            txtCredits.Columns = 4;
                            valueCell.Controls.Add(txtCredits);


                            if (optionValue is Boolean)
                            {
                                var ddValue = valueCell.Controls[0] as DropDownList;
                                ddValue.Attributes.Add("onchange",
                                    "$get('" + ddValue.ClientID + "').value == '" + (int)OptionAvailability.AllowedWithCredits + "' ? $get('" + txtCredits.ClientID + "').style.display = '' : $get('" + txtCredits.ClientID + "').style.display = 'none'");
                                
                                if (!enableCreditsPayment)
                                    txtCredits.Style[HtmlTextWriterStyle.Display] = "none";
                            }
                        }

                        itemRow.Cells.Add(valueCell);
                    }

                    tblPlans.Rows.Add(itemRow);

                    if (itemNum == propertyInfoArray.Length - 2)
                    {
                        TableRow footerRow = new TableRow();

                        TableCell footerCell = new TableCell();
                        footerRow.Cells.Add(footerCell);
                        //footerCell.Text = Lang.TransA("Plans");

                        foreach (var options in planOptions)
                        {
                            footerCell = new TableCell();
                            var option = propertyInfo.GetValue(options, null);
                            int planID = (int)option.GetType().GetField("PlanID").GetValue(option);
                            footerRow.Cells.Add(footerCell);

                            if (planID == -1)
                            {
                                //headerCell.Text = Lang.TransA("Non-Paying Members");
                            }
                            else
                            {
                                Button button = new Button();
                                button.Enabled = !readOnly;
                                button.ID = "btnDelete" + planID;
                                button.Text = "Delete".TranslateA();
                                button.CommandName = "Delete";
                                button.CommandArgument = planID.ToString();
                                button.Command += new CommandEventHandler((s, e) => {
                                    if (e.CommandName == "Delete")
                                    {
                                        if (readOnly)
                                            return;

                                        BillingPlan.Delete(Int32.Parse((string)e.CommandArgument));
                                        executeAfterDelete();
                                    }
                                });
                                //TextBox txtValue = new TextBox();
                                //txtValue.ID = "PlanName" + planID;

                                //var plan = BillingPlan.Fetch(planID);
                                //txtValue.Text = plan.Title;
                                footerCell.Controls.Add(button);
                            }
                        }

                        tblPlans.Rows.Add(footerRow);
                    }

                    //lastItem = item;
                    ++itemNum;
                }
            }

            phPlans.Controls.Add(tblPlans);
        }

        public static void UpdateBillingPlansFromUI(PlaceHolder phBillingPlans, BillingPlan[] billingplans)
        {
            foreach (BillingPlan plan in billingplans)
            {
                TextBox txtPlanName = FindControl(phBillingPlans, "PlanName" + plan.ID) as TextBox;
                TextBox txtAmount = FindControl(phBillingPlans, "Amount" + plan.ID) as TextBox;
                TextBox txtCycle = FindControl(phBillingPlans, "Cycle" + plan.ID) as TextBox;
                DropDownList ddCycleUnits = FindControl(phBillingPlans, "CycleUnits" + plan.ID) as DropDownList;

                //if (txtPlanName == null)
                //    throw new Exception(String.Format("No Control with ID {0}!", "PlanName" + plan.ID));

                if (txtPlanName.Text.Trim() == "")
                {
                    throw new ArgumentException(Lang.TransA("The plan name field cannot be empty!"));
                }

                try
                {
                    Decimal amount = Convert.ToDecimal(txtAmount.Text);
                    if (amount < 0)
                    {
                        throw new ArgumentException(Lang.TransA("The amount of money can't be negative!"));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Lang.TransA("Given amount of money is invalid"));
                }

                try
                {
                    int cycle = Convert.ToInt32(txtCycle.Text);
                    if (cycle < 1)
                    {
                        throw new ArgumentException(Lang.TransA("The billing cycle length must be positive number!"));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Lang.TransA("Given billing cycle length is invalid"));
                }


                plan.Title = txtPlanName.Text;
                plan.Amount = Convert.ToSingle(txtAmount.Text);
                plan.Cycle = Convert.ToInt32(txtCycle.Text);
                plan.CycleUnit = (CycleUnits) Convert.ToInt32(ddCycleUnits.SelectedItem.Value);
            }
        }
    }
}