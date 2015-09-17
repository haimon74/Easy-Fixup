<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="QuickStats.js" type="text/javascript"></script>
</head>
<body onload="StartTimer()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" EnablePartialRendering="false" EnablePageMethods="false" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Services/Gadgets.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="sitename">
            <%= Config.Misc.SiteTitle %>
        </div>
        <div class="userlink">
            <a href="<%= UrlRewrite.CreateShowUserUrl(Request.Params["Username"]) %>">
                <%= Request.Params["Username"]%>
            </a>
        </div>
        <table border="0" cellspacing="0" cellpadding="2" width="100%">
            <tr>
                <td align="center" valign="middle" class="photoframe">
                    <%= ImageHandler.RenderImageTag(Photo.GetPrimaryOrDefaultId(Request.Params["Username"]), 50, 50, null, false, false)%>
                </td>
                <td align="center" valign="top">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td valign="middle">
                                <img class="ratingimg" src="email.png"></td>
                            <td valign="middle">
                                <div class="ureadmails">
                                    <a href="<%= Config.Urls.Home + "/MailBox.aspx" %>"><span id="newmessages"></span></a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tablewrap2" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="infolabels">
                    <%= Lang.Trans("times viewed") %>
                    :</td>
                <td id="profileviews" class="infovalues">
                </td>
            </tr>
            <tr>
                <td class="infolabels">
                    <%= Lang.Trans("new users") %>
                    :</td>
                <td id="newusers" class="infovalues">
                </td>
            </tr>
            <tr>
                <td class="infolabels">
                    <%= Lang.Trans("users online") %>
                    :</td>
                <td id="onlineusers" class="infovalues">
                </td>
            </tr>
            <tr>
                <td class="infolabels">
                    <%= Lang.Trans("interests") %>
                    :</td>
                <td id="newinterests" class="infovalues">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
