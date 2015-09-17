<%@ Page language="c#" MasterPageFile="Site.Master" Codebehind="InviteFriend.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.InviteFriend" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
		<table class="wrap-sized">
		    <tr>
		    <td>
		        <table  style="width:350px;">
		            <tr>
		                <td><%= Lang.Trans("Friend's Name") %>&nbsp;</td>
		                <td><input class="textbox" type=text ID="txtFriendsName" Runat="server"/></td>
		            </tr>
		            <tr>
			            <td><%= Lang.Trans("Friend's Email Address") %>&nbsp;</td>
			            <td><input class="textbox" type=text id="txtFriendsEmail" runat="server"/></td>
			        </tr>			
		        </table>
		    </td>
		    </tr>
			<tr>
			<td><%= Lang.Trans("Add a personal note to this message:") %></td>
			</tr>
			<tr>
			<td>
			    <div class="fckeditor">
			        <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
			    </div>
			</td>
			</tr>
			<tr>
			<td>
			    <div class="srvmsg"><asp:label id="lblError" CssClass="error" runat="server"></asp:label></div>
			    <div class="buttons"><asp:button id="btnSubmit" tabIndex="1" runat="server" CssClass="SubmitBtn"></asp:button></div>
			</td>
			</tr>
		</table>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>
