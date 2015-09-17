<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="ViewEvents.ascx.cs" 
            Inherits="AspNetDating.Components.Profile.ViewEvents" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
    <asp:UpdatePanel ID="UpdatePanelEvents" runat="server">
        <ContentTemplate>
            <div class="srvmsg"><asp:Label ID="lblError" runat="server" EnableViewState="False"></asp:Label></div>
            <asp:PlaceHolder ID="plhEvents" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"></uc1:LargeBoxEnd>