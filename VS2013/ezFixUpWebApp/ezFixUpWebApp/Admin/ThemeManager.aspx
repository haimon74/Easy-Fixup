<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThemeManager.aspx.cs" Inherits="ezFixUp.Admin.ThemeManager" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Theme Manager</title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</head>
<body>
    <form id="form1" runat="server">
    <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
    <div id="layout">
        <div id="crupms">
            <img src="images/i_home.jpg">
            <div id="crumpstxt">
                <%= Lang.TransA("Theme Manager") %>
            </div>
        </div>
        <div id="sidebar">
            <uc1:AdminMenu ID="Adminmenu1" runat="server"></uc1:AdminMenu>
        </div>
        <div id="main">
            <div id="content-head">
                <div class="ch-head">
                    <%= "Installed Themes".TranslateA() %>
                </div>
                <div class="ch-description">
                    List of the available themes installed on your site
                </div>
            </div>
            <uc1:MessageBox ID="Messagebox1" runat="server"></uc1:MessageBox>
            <asp:DataList ID="dlThemes" Width="100%" RepeatDirection="Horizontal" RepeatColumns="3"
                CssClass="DataListWrap" runat="server" OnItemCommand="dlThemes_ItemCommand" CellPadding="0"
                CellSpacing="5" BorderWidth="0" GridLines="None" OnItemDataBound="dlThemes_ItemDataBound">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="top" />
                <ItemTemplate>
                    <div class="DataListHeader">
                        <%# Eval("Name") %></div>
                    <asp:Repeater ID="rptPreview" DataSource='<%# Eval("Previews") %>' runat="server">
                        <ItemTemplate>
                            <a href='<%# Eval("Value") %>' class="preview">
                                <img src='<%# Eval("Key") %>' border="0" /></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="DataListLinks" runat="server" visible='<%# Config.Misc.SiteTheme != (string) Eval("Name") %>'>
                        <b><a href='<%# Config.Urls.Home + "?theme=" + Eval("Name") %>' target="preview">preview</a></b>
                        | <b>
                            <asp:LinkButton ID="btnSelect" CommandName="Select" CommandArgument='<%# Eval("Name") %>'
                                runat="server">select</asp:LinkButton></b></div>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <div id="content-head">
                <div class="ch-head">
                    <%= "Themes Online".TranslateA() %>
                </div>
                <div class="ch-description">
                    Download and install new themes
                </div>
            </div>
            <asp:DataList ID="dlOnlineThemes" Width="100%" RepeatDirection="Horizontal" RepeatColumns="3"
                runat="server" OnItemCommand="dlOnlineThemes_ItemCommand" CellPadding="0" CellSpacing="5"
                BorderWidth="0" GridLines="None" CssClass="DataListWrap">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="top" />
                <ItemTemplate>
                    <div class="DataListHeader">
                        <%# Eval("Name") %></div>
                    <div class="DataListDescription">
                        <%# Eval("Description") %></div>
                    <asp:Repeater ID="rptPreview" DataSource='<%# Eval("Previews") %>' runat="server">
                        <ItemTemplate>
                            <a href='<%# Eval("ImageUrl") %>' class="preview">
                                <img src='<%# Eval("ThumbnailUrl") %>' height="90" border="0" /></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="DataListLinks">
                        <asp:LinkButton ID="btnDownload" CommandName="Download" CommandArgument='<%# Eval("Name") %>'
                            Visible='<%# Eval("DownloadUrl") != null %>' runat="server">download</asp:LinkButton>
                        <asp:HyperLink ID="lnkPurchase" NavigateUrl='<%# Eval("PurchaseUrl") %>' Visible='<%# Eval("PurchaseUrl") != null %>'
                            Target="_blank" runat="server">purchase</asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <div id="content-head">
                <div class="ch-head">
                    <%= "Custom Themes".TranslateA() %>
                </div>
                <div class="ch-description">
                    Our designers will create a unique theme for your site. The new design can be based
                    on your specifications, a mockup or existing design
                </div>
            </div>
                <div class="NewsWrap">
                    The service includes custom theme based on your requirements. The custom theme service
                    does not include programming changes or adding new features.
                    <br />
                    <a href="https://www.plimus.com/jsp/buynow.jsp?contractId=1699519" target="_blank">Order
                        custom theme for ezFixUp</a>
                    <br />
                    <br />
                    If you've already received your theme you can upload and install it below:
                    <br />
                    <asp:FileUpload ID="fileUploadTheme" runat="server" />
                    <asp:Button ID="btnUploadTheme" runat="server" OnClick="btnUploadTheme_Click" />
                </div>
                <div class="separator10"></div>
                <div class="separator10"></div>                
            
        </div>
    </div>
    </form>
</body>
</html>
