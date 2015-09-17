<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ContentPage.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ContentPage" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div class="WideBox">
	<div class="WideBoxTop">&nbsp;</div>
    <div class="WideBoxContent">
		<div id="divContent" runat="server"></div>
    </div>
</div>
</asp:Content>