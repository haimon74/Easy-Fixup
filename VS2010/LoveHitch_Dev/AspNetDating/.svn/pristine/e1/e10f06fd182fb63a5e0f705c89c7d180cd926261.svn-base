<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ExportPhotos.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ExportPhotos" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>
        <%= Lang.TransA("Export Photos") %></title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">

    <script language="javascript">
        function KeyDownHandler(btn) {
            // process only the Enter key
            if (event.keyCode == 13) {
                // cancel the default submit
                event.returnValue = false;
                event.cancel = true;
                // submit the form by programmatically clicking the specified button
                btn.click();
            }
        }
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
    <div id="layout">
        <div id="crupms">
            <img src="images/i_browsephotos.jpg">
            <div id="crumpstxt">
                <%= Lang.TransA("ExportPhotos") %></div>
        </div>
        <div id="sidebar">
            <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
        </div>
        <div id="main">
            <div id="content-head">
                <div class="ch-head">
                    <%= Lang.TransA("Export photos") %></div>
                <div class="ch-description">
                    <%= Lang.TransA("Use this page to export photos to files...") %>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" Interval="5000" runat="server" ontick="Timer1_Tick">
                    </asp:Timer>
                    <uc1:MessageBox ID="MessageBox1" runat="server"></uc1:MessageBox>
                    <asp:Button ID="btnStartExport" runat="server" Text="Start exporting" OnClick="btnStartExport_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
