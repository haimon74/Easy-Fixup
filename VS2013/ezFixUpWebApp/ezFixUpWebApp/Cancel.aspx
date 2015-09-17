<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="Cancel.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Cancel" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
	    <div class="center"><asp:Label ID="lblMessage" runat="server"></asp:Label></div>
    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>