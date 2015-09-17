<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AspNetDating.Mobile.Profile" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="../scripts/jquery.min.js" type="text/javascript"></script>
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>    
    <div align="center"><a href="UploadPhotos.aspx"><%= "Upload Photos".Translate()%></a></div>
    <div class="Separator"></div>    
    <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label>
    <asp:PlaceHolder id="plhProfile" runat="server"></asp:PlaceHolder>
    <asp:Button id="btnSave" runat="server" onclick="btnSave_Click" CssClass="LoginBtn"></asp:Button>
    <asp:Literal ID="litAlert" Runat="server"></asp:Literal><br>
</asp:Content>
