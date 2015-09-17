<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAd.aspx.cs" Inherits="ezFixUp.Admin.EditAd" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditAdCtrl" Src="~/Admin/EditAdCtrl.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
	    <title><%= Lang.TransA("Edit Classified") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
			<uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_manageuser.jpg">
                    <div id="crumpstxt"><%= Lang.TransA("Edit Classified") %></div>
                </div>
                <div id="sidebar">
                    <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Edit Classified") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to edit a classified...") %>
                        </div>
					</div>
                    <uc1:EditAdCtrl ID="EditAdCtrl1" runat="server" />
                </div>
            </div>
	    </form>
	</body>
</html>
