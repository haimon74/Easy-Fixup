<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingSettings.aspx.cs" Inherits="ezFixUp.Admin.BillingSettings" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>

<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Billing Settings") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
    <form id="form1" runat="server">
	<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
	<div id="layout">
		<div id="crupms">
            <img src="images/i_billingsettings.jpg"> 
            <div id="crumpstxt"><%= Lang.TransA("Billing Settings") %></div>
        </div>
		<div id="sidebar">
            <uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
        </div>
		<div id="main">
			<div id="content-head">
                <div class="ch-head"><%= Lang.TransA("Billing Settings") %></div>
                <div class="ch-description"><%= Lang.TransA("Use this section to choose and configure your billing plans...") %></div>
            </div>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:MessageBox ID="MessageBox1" EnableViewState="false" runat="server" />
                <asp:PlaceHolder ID="phBillingPlans" runat="server">
                </asp:PlaceHolder>
                <div class="separator10"></div>
               <div align="center"> <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" />
                <asp:Button ID="btnAddNewPlan" runat="server" onclick="btnAddNewPlan_Click" /></div>
            </ContentTemplate>
            </asp:UpdatePanel>   
        </div>
    </div>
    </form>
</body>
</html>
