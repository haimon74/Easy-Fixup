<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="AspNetDating.Mobile._default" %>

<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>      
    <div class="ContentWrap">    
    <div class="LoginBox">      
        <%= Lang.Trans("Username") %>:
        <br />
        <asp:TextBox ID="txtUsername" CssClass="TextField" runat="server"></asp:TextBox>
        <br />
        <%= Lang.Trans("Password") %>:
        <br />
        <asp:TextBox ID="txtPassword" CssClass="TextField" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:CheckBox ID="cbStealthMode" CssClass="StealthMode" Visible="false" runat="server"></asp:CheckBox>
        <br />
        <asp:Button ID="btnLogin" CssClass="LoginBtn" runat="server" 
            onclick="btnLogin_Click" />
    </div></div>
    <div class="SeparatorLine"></div>     
    <div class="RegisterText">
        Not registered yet? Registration is 100% free and easy. <br />
        <a href="Register.aspx">Register
            now!</a>    
	</div>
</asp:Content>
