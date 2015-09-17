<%@ Import namespace="AjaxChat"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGroup.ascx.cs" Inherits="ezFixUp.Components.Groups.ViewGroup" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewMembers" Src="~/Components/Groups/NewMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewTopics" Src="~/Components/Groups/NewTopics.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewPhotos" Src="~/Components/Groups/NewPhotos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewEvents" Src="~/Components/Groups/NewEvents.ascx" %>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc2" %>
<asp:MultiView ID="mvViewGroup" ActiveViewIndex="0" runat="server">
<asp:View ID="vGroupInfo" runat="server">
    <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
    <div class="srvmsg"><asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label></div>
    <uc2:HeaderLine ID="hlGroupName" runat="server" />
    <table cellpadding="0" cellspacing="0" width="100%">
	    <tr>
		    <td valign="top" width="124px">
			    <img class="photoframe" src='GroupIcon.ashx?groupID=<%= GroupID %>&width=120&height=120&diskCache=1' />
		    </td>
		    <td valign="top">
		    <div id="pnlGroupInformation" class="lineh pleft5">
	            <b><%= Lang.Trans("Categories") %>:</b>
	            <asp:Label ID="lblCategories" runat="server"></asp:Label>
	            <br />
	            <b><%= Lang.Trans("Created on") %>:</b>
	            <asp:Label ID="lblCreated" runat="server"></asp:Label>
	            <br />
	            <b><%= Lang.Trans("Type") %>:</b>
	            <asp:Label ID="lblType" runat="server"></asp:Label>
	            <br />
	            <b><%= Lang.Trans("Members") %>:</b>
	            <asp:LinkButton ID="lnkMembers" runat="server" OnClick="lnkMembers_Click"></asp:LinkButton>
	            <asp:Label ID="lblMembers" runat="server" Visible="false"></asp:Label>
	            <br />
	            <b><%= Lang.Trans("Owner") %>:</b>
	            <a id="lnkOwner" runat="server" class="user-name"></a>
	            <span id="pnlChatOnlineUsers" runat="server">
	                <br />
	                <%= String.Format(Lang.Trans("There are <b>{0}</b> users in the chat room"), ChatUsersOnline) %></span></div>
		    </td>
	    </tr>
    </table>
    <div class="note"><asp:Label ID="lblGroupDescription" runat="server"></asp:Label></div>
    <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
    <div class="clear"></div>
    <uc1:NewEvents id="NewEvents1" runat="server"></uc1:NewEvents>
    <uc1:NewTopics ID="NewTopics1" runat="server" />
    <uc1:NewMembers id="NewMembers1" runat="server" />
    <uc1:NewPhotos ID="NewPhotos1" runat="server" />
    <div class="buttons">
	    <asp:Button ID="btnJoinGroup" runat="server" OnClick="btnJoinGroup_Click"/>
    </div>
</asp:View>
<asp:View ID="vJoinGroup" runat="server">
<uc1:largeboxstart id="LargeBoxStart2" runat="server"></uc1:largeboxstart>
    <div class="srvmsg"><asp:Label ID="lblError2" runat="server" EnableViewState="False"></asp:Label></div>
    <div class="wrap-sized">
	    <div id="pnlTerms" runat="server">
	        <div class="label bold"><%= Lang.Trans("Terms & Conditions:") %></div>
	        <div class="text overflow">
		        <asp:Literal ID="ltrTerms" runat="server"></asp:Literal>
	        </div>
	        <div class="clear"></div>
	        <div class="agreement">
		        <asp:CheckBox ID="cbAgree" runat="server" />
	        </div>
	    </div>
	    <div id="pnlQuestion" runat="server">
	    <div class="label bold">
	        <asp:Label ID="lblQuestion" runat="server"></asp:Label>
	    </div>
	        <asp:TextBox ID="txtAnswer" Columns="50" CssClass="titletextbox" runat="server"></asp:TextBox>
	    </div>
    </div>
    <div class="buttons">
	    <asp:Button ID="btnJoinGroup2" runat="server" OnClick="btnJoinGroup2_Click"/>
    </div>
<uc1:largeboxend id="LargeBoxEnd2" runat="server"></uc1:largeboxend>
</asp:View>
</asp:MultiView>
