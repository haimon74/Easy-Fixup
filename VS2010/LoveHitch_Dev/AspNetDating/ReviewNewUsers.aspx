<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="ReviewNewUsers.aspx.cs" Inherits="AspNetDating.ReviewNewUsers"%>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem">
		<%= Lang.Trans("Moderation points") %>:<b>
		<%= CurrentUserSession.ModerationScores %></b></div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
        	<ContentTemplate>
	            <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
	            <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
	            <div id="pnlNewUser" runat="server">
	            <div class="center viewphotos">
		            <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
	            </div>
	            <br />
	            <b><asp:Label ID="lblPhotoName" runat="server" /></b>
	            <br />
	            <asp:Label ID="lblPhotoDescription" runat="server" />
	            <br />
	            <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" RepeatDirection="Horizontal"
                    CellPadding="0" CellSpacing="0" Width="100%" ShowFooter="False" 
                    ShowHeader="False" onitemcommand="dlPhotos_ItemCommand">
                    <ItemTemplate>
                        <table class="thumbs-wrap" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" valign="middle">
                                    <asp:LinkButton ID="lnkShowPhoto" CommandName="ShowPhoto" CommandArgument='<%# Eval("PhotoId") %>'
                                        runat="server">
							    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, false) %></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
	            <span class="label"><%= Lang.Trans("Username") %></span>:
                <%= CurrentNewUser != null ? CurrentNewUser.Username : String.Empty%><br>
                <span class="label"><%= Lang.Trans("Age") %></span>:
                <%= CurrentNewUser != null ? CurrentNewUser.Age : 0%><br>
                <span class="label"><%= Lang.Trans("Gender") %></span>:
                <%= CurrentNewUser.Gender %><br>
                <div id="pnlLocation" runat="server">
                    <span class="label"><%= Lang.Trans("Location") %></span>:
                    <span class="value"><%= CurrentNewUser.LocationString %></span><uc1:HeaderLine ID="hlSlogan" runat="server" /><asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder>
		            <div class="separator"></div>
		            <div class="buttons">
			            <asp:Button ID="btnApprove" runat="server" onclick="btnApprove_Click" />
			            <asp:Button ID="btnReject" runat="server" onclick="btnReject_Click" />
			            <asp:Button ID="btnPass" runat="server" onclick="btnPass_Click" />
		            </div>
	            </div>
	            </div>
            </ContentTemplate>
        </asp:UpdatePanel>        
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
