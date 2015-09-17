<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ContentPage.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.ContentPage" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div class="WideBox">
	<div class="WideBoxTop">&nbsp;</div>
    <div class="WideBoxContent">
		<div id="divContent" runat="server"></div>
    </div>
</div>
</asp:Content>