<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="ManageBadWords.aspx.cs" Inherits="Estream.AjaxChat.Admin.ManageBadWords" Title="Manage Bad Words" %>
<%@ Import Namespace="Estream.AjaxChat.Classes" %>
<%@ MasterType TypeName="Estream.AjaxChat.Admin.AdminMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphIcon" runat="server">
    <% 
        Master.CrumpsText = Lang.Trans("Manage Bad Words");
        Master.HeaderText = Lang.Trans("Manage Bad Words");
        Master.HeaderDescription = Lang.Trans("In this section you can enter bad words or modify existing ones...");
    %><img src="Images/i_managebadwords.jpg" alt="manage bad words" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
	<table cellpadding="0" cellspacing="0" id="stable">
		<tr>
			<td>
			    <asp:TextBox ID="txtBadWords" runat="server" TextMode="multiLine" CssClass="multilinetextbox"></asp:TextBox>
			</td>
		</tr>
	</table>
 	<div class="info">* Every word have to be on new line</div>
    <div class="separator10"></div>
    <div class="add-buttons" style="width:508px">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
    </div>
    <div class="separator06"></div>
</asp:Content>
