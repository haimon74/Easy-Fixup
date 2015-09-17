<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AffiliateBanners.aspx.cs" Inherits="ezFixUp.Affiliates.AffiliateBanners" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.Trans("Affiliate Banners") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	    <form id="form1" runat="server">
			<uc1:AffilaiteHeader ID="AffiliateHeader1" runat="server"></uc1:AffilaiteHeader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_browsebanners.jpg" />
                    <div id="crumpstxt"><%= Lang.Trans("Affiliate Banners")%></div>
                </div>
                <div id="sidebar">
                    <uc1:AffiliateMenu ID="AffiliateMenu1" runat="server"></uc1:AffiliateMenu>
                </div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.Trans("Affiliate Banners")%></div>
                        <div class="ch-description">
                            <%= Lang.Trans("Use this section to view available banners...") %>
                        </div>
					</div>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<asp:Repeater ID="rptAffiliateBanners" runat="server">
					    <ItemTemplate>
					     <table cellpadding="0" cellspacing="0" id="stable">
                    	<tr>
                        	<td class="table_header" colspan="2"><%= Lang.Trans("Banner") %></td>
                        </tr>
                        <tr>
                        	<td colspan="2" valign="middle" align="center" class="table_cell">
                        		<img src='<%= Config.Urls.Home%>/AffiliateBannerImage.ashx?id=<%# Eval("ID") %>' border="0" /></td>
                        </tr>
                            <tr>
                              <td class="table_cell"><%= Lang.Trans("Name") %>:</td>
                              <td class="table_cell"><%# Eval("Name") %></td>
                            </tr>
                            <tr>
                              <td class="table_cell"><%= Lang.Trans("HTML Code") %>:</td>
                              <td class="table_cell"><asp:TextBox TextMode="MultiLine" CssClass="multilinetextbox" ID="txtHTMLCode" runat="server" ReadOnly="true" Text='<%# Eval("HTMLCode") %>'></asp:TextBox></td>
                            </tr>
                        </table>
						<div class="separator10"></div>
					    </ItemTemplate>
					</asp:Repeater>
                </div>
            </div>
	    </form>
	</body>
</html>
