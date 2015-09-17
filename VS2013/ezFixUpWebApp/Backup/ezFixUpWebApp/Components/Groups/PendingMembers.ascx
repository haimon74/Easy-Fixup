<%@ Import namespace="ezFixUp"%>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingMembers.ascx.cs" Inherits="ezFixUp.Components.Groups.PendingMembers" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
<div class="srvmsg"><asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label></div>
<asp:DataList ID="dlPendingMembers" runat="server"  Width="100%" OnItemCommand="dlPendingMembers_ItemCommand" OnItemDataBound="dlPendingMembers_ItemDataBound">
    <ItemTemplate>
    	<table cellpadding="0" cellspacing="0" class="searchresultstable">
    		<tr>
    			<td valign="top" align="center" width="100">
					<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'>
						<%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 90, 90, "photoframe", false, true) %></a></td>
				<td valign="top" class="pleft5">
					<span class="label"><asp:Label ID="lblUsernameText" runat="server"></asp:Label>:</span>
					<span class="value"><asp:Label ID="lblUsername" runat="server"></asp:Label><br /></span>
					<span class="label"><asp:Label ID="lblAgeText" runat="server"></asp:Label>:</span>
					<span class="value"><asp:Label ID="lblAge" runat="server"></asp:Label><br /></span>
					<span class="label"><asp:Label ID="lblGenderText" runat="server"></asp:Label>:</span>
					<span class="value"><asp:Label ID="lblGender" runat="server"></asp:Label><br /></span>
					<span class="label" id="pnlAnswer" runat="server"><asp:Label ID="lblAnswerText" runat="server"></asp:Label>:</span>
					<span class="value"><asp:Label ID="lblAnswer" runat="server"></asp:Label><br /></span>
					<ul class="itemlinks">
						<li>
							<asp:LinkButton ID="lnkApprove" runat="server" Text='<%# Lang.Trans("Approve") %>' CommandName="Approve" CommandArgument='<%# Eval("Username") %>' />
						</li>
						<li>
							<asp:LinkButton ID="lnkReject" runat="server" Text='<%# Lang.Trans("Reject") %>' CommandName="Reject" CommandArgument='<%# Eval("Username") %>' />
						</li>
					</ul>
				</td>
			</tr>
		</table>
	</ItemTemplate>
</asp:DataList>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>