<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupMembers.ascx.cs" Inherits="ezFixUp.Components.Groups.GroupMembers" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Search/SearchResults.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
<asp:MultiView ID="mvViewGroupMembers" runat="server">

    <asp:View ID="viewGroupMembers" runat="server">
        <div id="pnlGroupMembersSearch" runat="server" class="UserFilter">
                <span id="pnlGender" runat="server">
                    <%= Lang.Trans("Gender") %>&nbsp;<asp:DropDownList CssClass="dropdownlist" ID="ddFilterByGender" runat="server"></asp:DropDownList>&nbsp;
                </span>
                <span id="pnlAge" runat="server">
                    <%= Lang.Trans("Age") %>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="textbox" Size="2" MaxLength="2"></asp:TextBox>
                    <%= Lang.Trans("to") %>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="textbox" Size="2" MaxLength="2"></asp:TextBox>
                </span>
                <%= Lang.Trans("Type") %>&nbsp;<asp:DropDownList CssClass="dropdownlist" ID="ddGroupMemberType" runat="server"></asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click"></asp:Button>
            </div>
        <uc1:SearchResults ID="SearchResults1" ShowSlogan="false" ShowCity="false" ShowIcons="false" ShowDistance="false" runat="server" />    
    </asp:View>
    <asp:View ID="viewDeleteOpitions" runat="server">
        <%= Lang.Trans("What do you want to do?") %><div class="separator6"></div>
        <asp:RadioButtonList ID="rbList" runat="server" />
        <div class="separator"></div>
        <asp:CheckBox ID="cbKickMember" CssClass="lineh" runat="server" />
        <br />
        <asp:CheckBox ID="cbBanMember" CssClass="lineh"  runat="server" />
        <asp:DropDownList ID="ddBanPeriod" CssClass="dropdownlist" runat="server"></asp:DropDownList>
        <div class="separator"></div>
        <div class="buttons">
	        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
	        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
        </div>
    </asp:View>

</asp:MultiView>

<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>

