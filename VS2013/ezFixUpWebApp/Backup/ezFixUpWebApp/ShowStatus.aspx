<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ShowStatus.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.ShowStatus" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
        <div class="srvmsg"><asp:Label ID="lblMessage" runat="server" /></div>
        <asp:PlaceHolder ID="plhLinkContainer" runat="server"></asp:PlaceHolder>
        <uc1:FlexButton ID="fbStatus" RenderAs="Button" runat="server" />
    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>	
