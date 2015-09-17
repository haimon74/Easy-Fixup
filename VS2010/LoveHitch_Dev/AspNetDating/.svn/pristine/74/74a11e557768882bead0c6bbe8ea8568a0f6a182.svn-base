<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliate.aspx.cs" Inherits="AspNetDating.Affiliates.EditAffiliate" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditAffiliateCtrl" Src="~/Affiliates/EditAffiliateCtrl.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.Trans("Edit Account Details") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	    <form id="form1" runat="server">
			<uc1:AffilaiteHeader ID="AdminHeader1" runat="server"></uc1:AffilaiteHeader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_editaffiliate.jpg" />
                    <div id="crumpstxt"><%= Lang.Trans("Edit Account Details")%></div>
                </div>
                <div id="sidebar">
                    <uc1:AffiliateMenu ID="AdminMenu1" runat="server"></uc1:AffiliateMenu>
                </div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.Trans("Edit Account Details")%></div>
                        <div class="ch-description">
                            <%= Lang.Trans("Use this section to edit your account details...") %>
                        </div>
					</div>
                    <uc1:EditAffiliateCtrl ID="EditAffiliateCtrl1" runat="server" />
                </div>
            </div>
	    </form>
	</body>
</html>

