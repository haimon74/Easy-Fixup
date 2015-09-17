<%@ Page Language="C#" MasterPageFile="~/ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUserEvents.aspx.cs" Inherits="ezFixUp.ShowUserEvents" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewEvents" Src="~/Components/Profile/ViewEvents.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphInnerContent" runat="server">
    <uc1:ViewEvents id="ViewEvents1" runat="server"></uc1:ViewEvents>
</asp:Content>
