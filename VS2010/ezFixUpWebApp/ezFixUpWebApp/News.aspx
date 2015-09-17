<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="News.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.NewsPage" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsBox" Src="Components/NewsBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
		<uc1:NewsBox id="NewsBox1" runat="server"></uc1:NewsBox>
	</div>
	<div id="main">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div id="divNewsContent" class="newscontent" runat="server"></div>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
