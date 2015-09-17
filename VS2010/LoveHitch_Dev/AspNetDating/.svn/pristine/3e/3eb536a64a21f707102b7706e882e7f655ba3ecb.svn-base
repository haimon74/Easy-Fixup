<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="NewUsers.js" type="text/javascript"></script>
</head>
<body onload="StartTimer()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" EnablePartialRendering="false" EnablePageMethods="false" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Services/Gadgets.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="sitename"><%= Config.Misc.SiteTitle %></div>
        <table class="photoframe"  border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td align="center" valign="middle"><a id="profilelink" href="#"><img id="profileimage" src="#" border="0" /></a></td>
          </tr>
        </table>
        <div class="userlink"><a id="profilelink2" href="#"></a></div>
    </form>
</body>
</html>