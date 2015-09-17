<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AspNetDating.Mobile.Register" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>
    <div class="ContentWrap">
    <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label>
    <asp:Panel ID="pnlCountry" runat="server">
        <%= Lang.Trans("Country")%>:
        <br />
        <asp:DropDownList ID="dropCountry" CssClass="DropDownList" runat="server" AutoPostBack="true" 
                onselectedindexchanged="dropCountry_SelectedIndexChanged"></asp:DropDownList>
    </asp:Panel>
    
    <asp:Panel ID="pnlState" runat="server">
    <div class="Separator"></div>
        <%= Lang.Trans("Region/State")%>:
        <br />
        <asp:DropDownList ID="dropRegion" runat="server" CssClass="DropDownList" AutoPostBack="true" 
            onselectedindexchanged="dropRegion_SelectedIndexChanged"></asp:DropDownList>
    </asp:Panel>
    <asp:Panel ID="pnlCity" runat="server">
    <div class="Separator"></div>
        <%= Lang.Trans("City")%>:
        <br />
        <asp:DropDownList ID="dropCity" runat="server" CssClass="DropDownList" AutoPostBack="true"></asp:DropDownList>
    </asp:Panel>
    <asp:Panel ID="pnlZipCode" runat="server">
    <div class="Separator"></div>
        <%= Lang.Trans("Zip/Postal Code") %>:
        <br />
        <asp:TextBox ID="txtZipCode" runat="server" CssClass="TextField"></asp:TextBox>
    </asp:Panel>
	<div class="Separator"></div>
    <%= Lang.Trans("Name") %>:
    <br />
    <asp:TextBox ID="txtName" runat="server" CssClass="TextField"></asp:TextBox>

    <asp:Panel ID="pnlGender" runat="server">
        <div class="Separator"></div>    
        <%= Lang.Trans("Gender") %>:
        <br />
        <asp:DropDownList ID="dropGender" CssClass="DropDownList" runat="server" AutoPostBack="true" 
            onselectedindexchanged="dropGender_SelectedIndexChanged1">
            <asp:ListItem Value=""></asp:ListItem>
		</asp:DropDownList>
    </asp:Panel>
    <asp:Panel ID="pnlInterestedIn" runat="server">
        <div class="Separator"></div>    
        <%= Lang.Trans("Interested in")%>:
        <br />
        <asp:DropDownList ID="dropInterestedIn" CssClass="DropDownList" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
		</asp:DropDownList>
    </asp:Panel>
    <asp:Panel ID="pnlBirthdate" runat="server">
        <div class="Separator"></div>    
        <%= Lang.Trans("Birthdate")%>:
        <br />
        <uc2:DatePicker ID="datePicker1" runat="server"></uc2:DatePicker>
    </asp:Panel>
    <asp:Panel ID="pnlBirthdate2" runat="server" Visible="false">
        <div class="Separator"></div>    
        <%= Lang.Trans("Birthdate 2")%>:
        <br />
        <uc2:DatePicker ID="datePicker2" runat="server"></uc2:DatePicker>
    </asp:Panel>
        <div class="Separator"></div>    
        <%= Lang.Trans("Username") %>:
    <br />
    <asp:TextBox ID="txtUsername" runat="server"  CssClass="TextField"></asp:TextBox>
    <div class="Separator"></div>
    <%= Lang.Trans("E-Mail")%>:
    <br />
    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextField"></asp:TextBox>
    <div class="Separator"></div>
    <%= Lang.Trans("Password")%>:
    <br />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="TextField"></asp:TextBox>
    <div class="Separator"></div>
    <%= Lang.Trans("Confirm password")%>:
    <br />
    <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" CssClass="TextField" EnableViewState="true"></asp:TextBox>
    <div class="Separator"></div>
    <asp:Panel ID="pnlInvitationCode" runat="server">
        <%= Lang.Trans("Invitation Code")%>:
        <br />
        <asp:TextBox ID="txtInvitationCode" runat="server" CssClass="TextField"></asp:TextBox>
    </asp:Panel>
    <br /><div align="center">
    <asp:CheckBox ID="cbAgreement" runat="server"></asp:CheckBox>
    
    <a href="../Agreement.aspx"><%= Lang.Trans("I ACCEPT the terms and conditions") %></a></div>
    <br />
    <asp:Button ID="btnRegister" CssClass="LoginBtn" runat="server" 
        onclick="btnRegister_Click" /></div>
</asp:Content>
