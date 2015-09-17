<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Codebehind="SendProfile.aspx.cs" Inherits="ezFixUp.SendProfile" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
        <uc1:smallboxstart id="SmallBoxStart2" runat="server"></uc1:smallboxstart>
        <div class="SideMenuItem">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkViewProfile" runat="server" OnClick="lnkViewProfile_Click" />
        </div>
        <uc1:smallboxend id="SmallBoxEnd2" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <uc1:largeboxstart id="LargeBoxStart" runat="server"></uc1:largeboxstart>
		<div class="wrap-sized" >
			<div class="srvmsg">
		        <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label>
	        </div>
		    <div class="clear"></div>
	        <div id="pnlSendProfile" class="settings" runat="server">
	        	<table>
	        		<tr>
	        			<td>
	        				<%= Lang.Trans("Your Name:") %>
	        			</td>
	        			<td>
	        				<asp:TextBox ID="txtSender" runat="server"></asp:TextBox>
	        			</td>
	        		</tr>
	        		<tr>
	        			<td>
	        				<%= Lang.Trans("Friend's Name:") %>
	        			</td>
	        			<td>
	        				<asp:TextBox ID="txtRecipientName" runat="server"></asp:TextBox>
	        			</td>
	        		</tr>  
	        		<tr>
	        			<td width="20%">
	        				<%= Lang.Trans("Friend's Email:") %>
	           			</td>
	        			<td>
	        				<asp:TextBox ID="txtRecipientMail" runat="server"></asp:TextBox>
	        			</td>
	        		</tr>    
	        		<tr>
	        			<td colspan="2">
	        			<div class="separator"></div>
		        			<%= Lang.Trans("Message:") %>
							<br/>
			                <div class="fckeditor">
			                	<asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
			                </div>
	        			</td>
	        		</tr>             
				</table>
	        </div>
            <div class="buttons">
            	<asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" />
            </div>
	    </div>
	    <uc1:largeboxend id="LargeBoxEnd" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
