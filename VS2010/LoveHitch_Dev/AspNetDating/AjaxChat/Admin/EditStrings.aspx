<%@ Page language="c#" MasterPageFile="Admin.Master" Codebehind="EditStrings.aspx.cs" AutoEventWireup="True" Inherits="Estream.AjaxChat.Admin.EditStrings" Title="Edit Strings" %>
<%@ MasterType TypeName="Estream.AjaxChat.Admin.AdminMaster" %>
<%@ Import namespace="Estream.AjaxChat.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphIcon" runat="server">
    <% 
        Master.CrumpsText = Lang.Trans("Text Management");
        Master.HeaderText = Lang.Trans("Text Management");
        Master.HeaderDescription = Lang.Trans("Use this section to edit the text used throughout the site");
    %><img src="Images/i_textmanagement.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <div id="pnlLanguage" runat="server">
        <table cellSpacing="0" cellPadding="0" class="filter">
	        <tr>
		        <td class="table_header" colSpan="2"><%= Lang.Trans("Languages") %></td>
	        </tr>
	        <tr>
		        <td class="table_cell"><%= Lang.Trans("Language") %>:</td>
		        <td class="table_cell"><asp:dropdownlist id="ddLanguage" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
				        <asp:ListItem></asp:ListItem>
			        </asp:dropdownlist></td>
	        </tr>
        </table>
		<div class="separator10"></div>
    </div>
    <asp:datagrid id="dgTranslations" Runat="server" CssClass="btable1" BorderWidth="0" cellspacing="0" cellpadding="0" AutoGenerateColumns="False" AllowPaging="False"
        Width="100%" GridLines="None">
        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
        <Columns>
	        <asp:TemplateColumn>
		        <HeaderStyle CssClass="table_header2"></HeaderStyle>
		        <ItemStyle CssClass="table_cell3" Width="48%"></ItemStyle>
		        <ItemTemplate>
			        <%# Server.HtmlEncode((string)Eval("Key")) %>
		        </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
		        <HeaderStyle CssClass="table_header2"></HeaderStyle>
		        <ItemStyle CssClass="table_cell3" Width="50%"></ItemStyle>
		        <ItemTemplate>
			        <input id="hidKey" value='<%# Eval("Key") %>' type="hidden" runat="server" />
			        <input id="hidOriginalValue" value='<%# Eval("Value") %>' type="hidden" runat="server" />
			        <asp:TextBox ID="txtValue" Width="98%" Runat="server" Text='<%# Eval("Value") %>'>
			        </asp:TextBox>
		        </ItemTemplate>
	        </asp:TemplateColumn>
        </Columns>
    </asp:datagrid>
	<div class="separator10"></div>
    <div class="add-buttons">
        <asp:Button ID="btnSave" Visible="False" Runat="server" onclick="btnSave_Click"></asp:Button>
    </div>
</asp:Content>
