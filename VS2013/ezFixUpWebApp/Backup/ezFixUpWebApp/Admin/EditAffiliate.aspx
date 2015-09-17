<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliate.aspx.cs" Inherits="ezFixUp.Admin.EditAffiliate" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditAffiliateCtrl" Src="~/Admin/EditAffiliateCtrl.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Edit Affiliate") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
			<uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_manageuser.jpg">
                    <div id="crumpstxt"><%= Lang.TransA("Edit Affilaite") %></div>
                </div>
                <div id="sidebar">
                    <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Edit Affiliate") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to edit affiliate...") %>
                        </div>
					</div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <uc1:EditAffiliateCtrl ID="EditAffiliateCtrl1" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
	    </form>
	</body>
</html>
