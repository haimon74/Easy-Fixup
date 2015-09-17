<%@ Page Language="C#" MasterPageFile="~/ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUserBlog.aspx.cs" Inherits="ezFixUp.ShowUserBlog" %>
<%@ Register TagPrefix="uc1" TagName="ViewBlog" Src="Components/Blog/ViewBlog.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphInnerContent" runat="server">
    <uc1:ViewBlog id="ViewBlogCtrl" runat="server" />
</asp:Content>