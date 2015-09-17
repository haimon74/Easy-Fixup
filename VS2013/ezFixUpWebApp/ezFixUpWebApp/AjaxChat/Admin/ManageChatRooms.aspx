<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="ManageChatRooms.aspx.cs" Inherits="Estream.AjaxChat.Admin.ManageChatRooms" Title="Manage Chat Rooms" %>
<%@ Import namespace="Estream.AjaxChat.Classes"%>
<%@ MasterType TypeName="Estream.AjaxChat.Admin.AdminMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphIcon" runat="server">
    <% 
        Master.CrumpsText = Lang.Trans("Manage Chat Rooms");
        Master.HeaderText = Lang.Trans("Manage Chat Rooms");
        Master.HeaderDescription = Lang.Trans("In this section you can create new chat rooms or modify existing ones...");
    %><img src="Images/i_managechatrooms.jpg" alt="manage chat rooms" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
<asp:MultiView ID="mvChatRooms" ActiveViewIndex="0" runat="server">
<asp:View ID="vChatRooms" runat="server">
    <asp:GridView ID="gvChatRooms" AllowSorting="False" AllowPaging="False" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
       AutoGenerateColumns="False"  OnRowCommand="gvChatRooms_RowCommand" OnRowDataBound="gvChatRooms_RowDataBound" GridLines="None"  CssClass="btable1">
		<AlternatingRowStyle  CssClass="btable2"></AlternatingRowStyle>       
        <Columns>
          <asp:BoundField DataField="ID" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" HeaderText="ID" />
          <asp:BoundField DataField="Name" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" HeaderText="Name" />
          <asp:BoundField DataField="MaxUsers" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" HeaderText="MaxUsers" />
          <asp:BoundField DataField="Visible" HeaderStyle-CssClass="table_header2" ItemStyle-CssClass="table_cell3" HeaderText="Visible" />
          <asp:TemplateField>
            <HeaderStyle CssClass="table_header2"></HeaderStyle>
        	<ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" Text="Edit" CommandName="EditChatRoom" CommandArgument='<%# Eval("ID") %>' runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkDelete" Text="Delete" CommandName="DeleteChatRoom" CommandArgument='<%# Eval("ID") %>' runat="server"></asp:LinkButton>
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
        <PagerSettings  />
    </asp:GridView>
    <div class="add-buttons">
	    <asp:Button ID="btnAddNewChatRoom" runat="server" OnClick="btnAddNewChatRoom_Click"/>
    </div>
</asp:View>
<asp:View ID="vChatRoomDetails" runat="server">
    <table cellpadding="0" cellspacing="0"  id="stable">
    	<tr><td colspan="2" class="table_header"><%= Lang.Trans("Manage Chat Room")%></td></tr>
        <tr><td class="table_cell" width="20%"><%= Lang.Trans("Name") %></td><td class="table_cell"><asp:TextBox CssClass="textbox" ID="txtName" runat="server"></asp:TextBox></td></tr>
        <tr><td class="table_cell"><%= Lang.Trans("Topic") %></td><td class="table_cell"><asp:TextBox ID="txtTopic" CssClass="textbox" runat="server"></asp:TextBox></td></tr>
        <tr><td class="table_cell"><%= Lang.Trans("Password") %></td><td class="table_cell"><asp:TextBox ID="txtPassword" CssClass="textbox" runat="server"></asp:TextBox></td></tr>        
        <tr><td class="table_cell"><%= Lang.Trans("MaxUsers") %></td><td class="table_cell"><asp:TextBox ID="txtMaxUsers" CssClass="textbox" runat="server"></asp:TextBox></td></tr>  
        <tr><td class="table_cell"><%= Lang.Trans("Visible") %></td><td class="table_cell"><asp:CheckBox ID="chkVisible" Checked="True" runat="server" /></td></tr>
        <tr><td colspan="2"><asp:HiddenField ID="hidChatRoomId" runat="server" />
        	<div class="separator06"></div>
        	<div class="add-buttons"><asp:Button id="btnSaveChatRoom" runat="server" OnClick="SaveChatRoom_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" /></div>
      	    <div class="separator06"></div>
        	</td></tr>
    </table>
    <div class="separator10"></div>
    <div id="pnlAddAdmins" runat="server">
      	<div class="separator10"></div>
    	<table cellpadding="0" cellspacing="0"  id="stable">
    	<tr><td colspan="2" class="table_header"><%= Lang.Trans("Add Administrator")%></td></tr>
			<tr>
				<td class="table_cell">
					<%=Lang.Trans("Username") %>&nbsp;<asp:TextBox ID="txtAdmin" runat="server"></asp:TextBox>
				</td>
				<td class="table_cell">
					<%=Lang.Trans("Password") %>&nbsp;<asp:TextBox ID="txtAdminPassword" runat="server"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<div class="separator06"></div>
					<asp:Button ID="btnAddAdmin" runat="server" OnClick="btnAddAdmin_Click" />
					<div class="separator06"></div>
				</td>
			</tr>
		</table>  	        
        <div class="separator10"></div>
      	<div class="separator10"></div>
	        <asp:DataList CellPadding="0" CellSpacing="0" 
	        CssClass="filter" GridLines="None"
	        ID="dlAdmins" runat="server" OnItemCommand="dlAdmins_ItemCommand">
            <HeaderStyle CssClass="table_header"></HeaderStyle>
            <ItemStyle CssClass="table_cell"></ItemStyle>
            	<HeaderTemplate> 
					<%# Lang.Trans("Manage Chat Room Administrators") %>
            	</HeaderTemplate>
	            <ItemTemplate>
	                <table cellpadding="0" cellspacing="0" width="100%">
	                	<tr>
	                		<td width="40%"><%# Eval("Username") %></td>
	                		<td>
	                			<asp:LinkButton ID="lnkDelete" Text='<%# Lang.Trans("Delete") %>' CommandName="Delete" CommandArgument='<%# Eval("UserId") %>' runat="server"></asp:LinkButton>
	                		</td>
	                	</tr>
	                </table>	                
	            </ItemTemplate>
	        </asp:DataList>
      	<div class="separator10"></div>
    </div>   
</asp:View>
</asp:MultiView>
</asp:Content>
