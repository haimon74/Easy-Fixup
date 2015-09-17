<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="BrowseUsers.aspx.cs" Inherits="Estream.AjaxChat.Admin.BrowseUsers" Title="Browse Users" %>
<%@ Import namespace="Estream.AjaxChat.Classes"%>
<%@ Import namespace="Estream.AjaxChat.Admin"%>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="../Components/DatePicker.ascx" %>
<%@ MasterType TypeName="Estream.AjaxChat.Admin.AdminMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphIcon" runat="server">
    <% 
        Master.CrumpsText = Lang.Trans("User Management");
        Master.HeaderText = Lang.Trans("User Management");
        Master.HeaderDescription = Lang.Trans("Use this section to browse, edit or delete members of your site...");
    %><img src="Images/i_browsemembers.jpg" alt="browse members" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
<asp:MultiView ID="mvChatUsers" runat="server" ActiveViewIndex="0">
    <asp:View ID="vChatUsers" runat="server">
    <table id="tblHideSearch" cellpadding="0" cellspacing="0" class="filter" runat="server" visible="false">
    	<tr>
        	<td class="table_header">
            	<%= "Filter" %></td>
            <td class="table_header">
            	[&nbsp;<a onclick="document.getElementById('tblSearch').style.display = 'block'; document.getElementById('<%= tblHideSearch.ClientID %>').style.display = 'none';"
                href="javascript: void(0);"><%= "Expand filter" %></a>&nbsp;]
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlFilterDefaultButton" DefaultButton="btnSearch" runat="server">
    <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
    	<tr>
        	<td colspan="2" class="table_header">
            	<%= "Filter" %></td>
        </tr>
        <tr>
             <td class="table_cell">
             	<%= "Username" %>:
             </td>
             <td class="table_cell">
             	<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
             </td>
        </tr>
        <tr>
         	<td class="table_cell">
            	<%= "DisplayName" %>:
            </td>
            <td class="table_cell">
            	<asp:TextBox ID="txtDisplayName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
         	<td class="table_cell">
            	<%= "Gender" %>:
            </td>
            <td class="table_cell">
            	<asp:DropDownList ID="ddGender" runat="server">
            		<asp:ListItem Value=""></asp:ListItem>
            	</asp:DropDownList>
        	</td>
		</tr>
        <tr>
         	<td class="table_cell">
            	<%= "BeginTimeFrom" %>:
            </td>
            <td class="table_cell"><uc1:DatePicker id="dpBeginTimeFrom" runat="server"></uc1:DatePicker></td>
        </tr>
        <tr>
         	<td class="table_cell">
            	<%= "BeginTimeTo" %>:
            </td>
            <td class="table_cell"><uc1:DatePicker id="dpBeginTimeTo" runat="server"></uc1:DatePicker></td>
        </tr>
        <tr>
         	<td class="table_cell">
            	<%= "LastActiveFrom" %>:
            </td>
            <td class="table_cell"><uc1:DatePicker id="dpLastActiveFrom" runat="server"></uc1:DatePicker></td>
        </tr>
        <tr>
         	<td class="table_cell">
            	<%= "LastActiveTo" %>:
            </td>
            <td class="table_cell"><uc1:DatePicker id="dpLastActiveTo" runat="server"></uc1:DatePicker></td>
        </tr>                     		
        <tr>
        	<td class="table_cell">
            	<%= "Signup IP" %>:
            </td>
            <td class="table_cell">
            	<asp:TextBox ID="txtIP" runat="server"></asp:TextBox>
            </td>
		</tr>
        <tr>
        	<td colspan="2" class="table_footer" align="center">
            	<div class="separator06"></div>
                <asp:Button ID="btnSearch" Font-Name="Verdana" runat="server" OnClick="btnSearch_Click"></asp:Button>
            </td>
		</tr>
	</table>
	</asp:Panel>
    <script type="text/javascript">
		if (document.getElementById('<%= tblHideSearch.ClientID %>'))
		    document.getElementById('tblSearch').style.display = 'none';
    </script>
	<div class="separator06"></div>
    <table cellpadding="0" cellspacing="0" width="100%">
    	<tr>
        	<td align="right" class="perpage">
            	<%= "Users per page" %>:
                <asp:DropDownList ID="ddUsersPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddUsersPerPage_SelectedIndexChanged"></asp:DropDownList>
                <div class="separator06"></div>
            </td>
		</tr>
	</table>
    <asp:GridView ID="gvUsers" AllowSorting="True" AllowPaging="false" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
       AutoGenerateColumns="False" OnSorting="gvUsers_Sorting" OnRowCommand="gvUsers_RowCommand" GridLines="None"  CssClass="btable1">
		<AlternatingRowStyle  CssClass="btable2"></AlternatingRowStyle>
        <Columns>
          <asp:BoundField DataField="Username" HeaderText="Username" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" SortExpression="Username" />
          <asp:BoundField DataField="DisplayName" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" HeaderText="DisplayName" />
          <asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" SortExpression="Gender" />
          <asp:BoundField DataField="BeginTime" HeaderText="BeginTime" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" SortExpression="BeginTime" />
          <asp:BoundField DataField="LastActive" HeaderText="LastActive" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" SortExpression="LastActive" />
          <asp:BoundField DataField="IP" HeaderText="IP" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" SortExpression="IP" />
          <asp:TemplateField>
            <HeaderStyle CssClass="table_header2"></HeaderStyle>
        	<ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" Text="Edit" CommandName="EditChatUser" CommandArgument='<%# Eval("Id") %>' runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkDelete" Text="Delete" CommandName="DeleteChatUser" CommandArgument='<%# Eval("Id") %>' runat="server"></asp:LinkButton>
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>       
    </asp:GridView>
    <asp:Panel ID="pnlPaginator" runat="server">
    <div class="separator06"></div>
    <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
    	<tr>
        	<td>
            	<asp:LinkButton ID="lnkFirst" Text="[ First ]" OnCommand="ChangePage" CommandName="First" CssClass="pagelink" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkPrev" Text="[ Prev ]" OnCommand="ChangePage" CommandName="Previous" CssClass="pagelink" runat="server"></asp:LinkButton>
                <asp:Label ID="lblPager" runat="server"></asp:Label>
                <asp:LinkButton ID="lnkNext" Text="[ Next ]" OnCommand="ChangePage" CommandName="Next" CssClass="pagelink" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkLast" Text="[ Last ]" OnCommand="ChangePage" CommandName="Last" CssClass="pagelink" runat="server"></asp:LinkButton>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </asp:View>
    <asp:View ID="vChatUserDetails" runat="server">
    <table cellpadding="0" cellspacing="0" class="filter">
    	<tr><td colspan="2" class="table_header"><%= Lang.Trans("Edit User") %></td></tr>
        <tr><td class="table_cell">Username</td><td class="table_cell"><asp:Label ID="lblUsername" runat="server"></asp:Label></td></tr>
        <tr><td class="table_cell">DisplayName</td><td class="table_cell"><asp:TextBox ID="txtEditedDisplayName" runat="server"></asp:TextBox></td></tr>
        <tr><td class="table_cell">Password</td><td class="table_cell"><asp:TextBox ID="txtEditedPassword" runat="server"></asp:TextBox></td></tr>
        <tr><td class="table_cell">Gender</td><td class="table_cell"><asp:DropDownList ID="ddEditedGender" runat="server"></asp:DropDownList></td></tr>        
        <tr><td colspan="2" class="table_footer" align="center">
            <div class="separator06"></div>
        	<asp:HiddenField ID="hidChatUserId" runat="server" /><asp:Button id="btnSaveChatUser" runat="server" OnClick="btnSaveChatUser_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
            <div class="separator06"></div>
        </td></tr>
        	
    </table>        
    </asp:View>
    </asp:MultiView>
</asp:Content>
