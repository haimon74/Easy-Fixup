<%@ Page Language="c#" MasterPageFile="~/Site.Master" Codebehind="SmsConfirm.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.SmsConfirm" %>
<%@ Register TagPrefix="cv" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
        <cv:ContentView ID="ContentView1" Key="SmsConfirm" runat="server"></cv:ContentView>
        <%= Lang.Trans("Confirmation code:") %>
        <asp:TextBox ID="txtConfirmationCode" runat="server"></asp:TextBox>
        <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" />
    <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
</asp:Content>
