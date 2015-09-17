<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadGroupPhoto.ascx.cs" Inherits="AspNetDating.Components.Groups.UploadGroupPhoto" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Search/SearchResults.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
<div class="wrap-sized">
	<div class="srvmsg"><asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label></div>
	<div class="clear"></div>
	<%= Lang.Trans("Name") %>
	<br/>
	<asp:TextBox ID="txtName" CssClass="titletextbox" runat="server"></asp:TextBox>
	<div class="separator"></div>
	<%= Lang.Trans("Description") %>
	<br/>
	<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="multilinetextbox"></asp:TextBox>
	<div class="separator"></div>
	<asp:FileUpload ID="fuGroupPhoto" runat="server" />
	<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" />
</div>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>