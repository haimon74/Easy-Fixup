<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>--%>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPopupTips.aspx.cs"
    Inherits="ezFixUp.Admin.EditPopupTips" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="MasterHead" runat="server">
    <title>
        <%= Lang.TransA("Edit Content Views") %></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <%--<ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />--%>
    <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
    <div id="layout">
        <div id="crupms">
            <img src="images/i_editpages.jpg" alt='' />
            <div id="crumpstxt">
                <%= Lang.TransA("Edit Popups Tips") %></div>
        </div>
        <div id="sidebar">
            <uc1:AdminMenu ID="Adminmenu1" runat="server"></uc1:AdminMenu>
        </div>
        <asp:UpdatePanel ID="updatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div id="main">
                    <div id="content-head">
                        <div class="ch-head">
                            <%= Lang.TransA("Edit Popups Tips")%></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to edit tips in popups") %></div>
                    </div>
                    <table cellspacing="0" cellpadding="0" class="filter">
                        <tr>
                            <td class="table_header" colspan="2">
                                <%= Lang.TransA("Languages/Tips") %>
                            </td>
                        </tr>
                        <tr id="trLanguage" runat="server">
                            <td class="table_cell">
                                <%= Lang.TransA("Select Language:") %>
                            </td>
                            <td class="table_cell">
                                <asp:DropDownList ID="ddLanguage" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddLanguage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trPageName" runat="server">
                            <td class="table_cell">
                                <%= Lang.TransA("Select Tip:") %>
                            </td>
                            <td class="table_cell">
                                <asp:DropDownList ID="ddContentKey" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddContentKey_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="separator10">
                    </div>
                    <uc1:MessageBox ID="Messagebox1" runat="server"></uc1:MessageBox>
                    <div id="divEdit" runat="server" visible="False">
                        <table cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td class="table_header2" align="center" colspan="2">
                                    <%= (ddContentKey.SelectedItem.Value == "0")
                                            ? Lang.TransA("New Tip") 
                                            : Lang.TransA("Edit Tip") 
                                    %>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Tip Key Name") %>
                                </td>
                                <td class="table_cell">
                                    <div>
                                        <asp:TextBox ID="txtKeyName" runat="server" class="textbox"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Tip Title") %>
                                </td>
                                <td class="table_cell">
                                    <div>
                                        <asp:TextBox ID="txtTitle" runat="server" class="textbox"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Time to Show:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddlTimesToShow" AutoPostBack="False" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Login Count Trigger:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddlLoginCountTrigger" AutoPostBack="False" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Days Count Trigger:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddlDaysCountTrigger" AutoPostBack="False" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Credits Below Value Trigger:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddlCreditsTrigger" AutoPostBack="False" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("No Use of Credits Days Count Trigger:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddlNoSpendingDaysTrigger" AutoPostBack="False" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Pages Names to View on:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:TextBox ID="txtPagesNames" class="textbox" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Is Matchmaker State Tip:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:CheckBox ID="cbIsMatchmaker" runat="server" Text="Check to view in matchmaker state and Uncheck to view in normal mode.">
                                    </asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell3">
                                    <%= Lang.TransA("Tip Content") %>
                                </td>
                                <td class="table_cell3">
                                    <%--<div class="">
                                        <asp:PlaceHolder ID="phEditor" runat="server" Visible="false"></asp:PlaceHolder>
                                    </div>--%>
                                    <asp:TextBox ID="advancedHtmlEditor" runat="server" TextMode="MultiLine"
                                            Columns="50" Rows="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="table_cell3" colspan="2">
                                    <div class="add-buttons">
                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"></asp:Button>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                    <div>
                                        
                                        <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender2" 
                                            TargetControlID="advancedHtmlEditor" 
                                            OnImageUploadComplete="ajaxUpload1_OnUploadComplete"
                                            runat="server" 
                                            DisplaySourceTab="True">
                                            <Toolbar>
                                                <ajaxToolkit:Undo />
                                                <ajaxToolkit:Redo />
                                                <ajaxToolkit:Bold />
                                                <ajaxToolkit:Italic />
                                                <ajaxToolkit:Underline />
                                                <ajaxToolkit:StrikeThrough />
                                                <ajaxToolkit:Subscript />
                                                <ajaxToolkit:Superscript />
                                                <ajaxToolkit:JustifyLeft />
                                                <ajaxToolkit:JustifyCenter />
                                                <ajaxToolkit:JustifyRight />
                                                <ajaxToolkit:JustifyFull />
                                                <ajaxToolkit:InsertOrderedList />
                                                <ajaxToolkit:InsertUnorderedList />
                                                <ajaxToolkit:CreateLink />
                                                <ajaxToolkit:UnLink />
                                                <ajaxToolkit:RemoveFormat />
                                                <ajaxToolkit:SelectAll />
                                                <ajaxToolkit:UnSelect />
                                                <ajaxToolkit:Delete />
                                                <ajaxToolkit:Cut />
                                                <ajaxToolkit:Copy />
                                                <ajaxToolkit:Paste />
                                                <ajaxToolkit:BackgroundColorSelector />
                                                <ajaxToolkit:ForeColorSelector />
                                                <ajaxToolkit:FontNameSelector />
                                                <ajaxToolkit:FontSizeSelector />
                                                <ajaxToolkit:Indent />
                                                <ajaxToolkit:Outdent />
                                                <ajaxToolkit:InsertHorizontalRule />
                                                <ajaxToolkit:HorizontalSeparator />
                                                <ajaxToolkit:InsertImage />
                                            </Toolbar>
                                        </ajaxToolkit:HtmlEditorExtender>
                                    </div>
                    <%--<asp:TextBox runat="server" ID="txtBox2" TextMode="MultiLine" Columns="50" Rows="10"
                    Text="Hello <b>world!</b>" /><br />
                <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="txtBox2"
                    runat="server" DisplaySourceTab="True" OnImageUploadComplete="ajaxFileUpload_OnUploadComplete">
                    <Toolbar>                        
                        <ajaxToolkit:Bold />
                        <ajaxToolkit:Italic />
                        <ajaxToolkit:Underline />
                        <ajaxToolkit:HorizontalSeparator />
                        <ajaxToolkit:JustifyLeft />
                        <ajaxToolkit:JustifyCenter />
                        <ajaxToolkit:JustifyRight />
                        <ajaxToolkit:JustifyFull />
                        <ajaxToolkit:CreateLink />
                        <ajaxToolkit:UnLink />
                        <ajaxToolkit:InsertImage />                        
                    </Toolbar>
                </ajaxToolkit:HtmlEditorExtender>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
