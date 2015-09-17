<%@ Page Language="C#" MasterPageFile="~/ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUser.aspx.cs" Inherits="ezFixUp.ShowUserPage" %>
<%@ Register TagPrefix="uc1" TagName="ViewProfile" Src="Components/Profile/ViewProfile.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphInnerContent" runat="server">
    <uc1:ViewProfile id="ViewProfileCtrl" runat="server" IsIconsVisible="true" />
</asp:Content>