<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadAudio.ascx.cs" Inherits="ezFixUp.Components.Profile.UploadAudio" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<uc1:HeaderLine ID="HeaderLine1" runat="server" />
<div class="note">
    <components:ContentView ID="cvNote" Key="UploadAudioNotes" runat="server"></components:ContentView>
</div>
<div class="center">
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="false"></asp:Label>
    <div id="divUploadAudio" runat="server">
        <%= Lang.Trans("Title") %>:
        <asp:TextBox ID="txtTitle" CssClass="textbox" runat="server"></asp:TextBox>
        <div class="separator"></div>
        <%= Lang.Trans("Audio file") %>:
        <asp:FileUpload ID="fileAudio" runat="Server" />
        <div class="separator"></div>
        <asp:CheckBox ID="cbPrivateAudio" runat="server" />
        <%= Lang.Trans("Set this audio as private")%><div class="separator"></div>
        <asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click"/>
    </div>
</div>
<asp:Repeater ID="rptAudioUploads" runat="server" 
        onitemcommand="rptAudioUploads_ItemCommand" 
        onitemcreated="rptAudioUploads_ItemCreated">
    <ItemTemplate>
    	<table cellpadding="5" cellspacing="0" width="100%">
    		<tr>
    			<td width="70px">
    				<img src='<%# Eval("ImageURL") %>' />
    			</td>
    			<td valign="middle">
    				<div class="label">
			        	<asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
			        </div>
			        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
			        <asp:Button ID="btnDeleteAudioUpload" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"/>
    			</td>
    		</tr>
    	</table>
    </ItemTemplate>
</asp:Repeater>
<br>
<div class="clear"></div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>