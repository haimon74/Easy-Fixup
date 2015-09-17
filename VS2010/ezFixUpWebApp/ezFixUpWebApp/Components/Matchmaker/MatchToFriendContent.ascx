<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MatchToFriendContent.ascx.cs" Inherits="ezFixUp.Components.Matchmaker.MatchToFriendContent" %>
<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<%@ Register TagName="SendMsgIcon" Src="~/Components/Buttons/SendMsgIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="SendWinkIcon" Src="~/Components/Buttons/SendWinkIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="SendGiftIcon" Src="~/Components/Buttons/SendGiftIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="MoreIcon" Src="~/Components/Buttons/MoreIcon.ascx" TagPrefix="btn" %>



<div id="mm_popup" style="width:416px;height:272px; display:none; position:fixed; z-index:1000000;left:30%;
    background: url(App_Themes/Colors2_MM/find_match_pop_bg.png) no-repeat;">
    <table style="width:100%;height:100%;">
        <tr><td style='height:100px;'>
        </td></tr>        
        <tr><td  style='height:40px;width:100%;'>
            <div style="float:left;position:relative;left:350px;width:35px;height:35px;cursor:pointer;" 
                        title='Close'
                        onclick="jQuery('#mm_popup').hide();window.location='FriendsList.aspx';"></div>
        </td></tr>
        <tr><td style="padding:22px;height:120px;width:416px;text-align:center;">
            <h1 style="color:#C1272D;margin-left:5px;font-size:16pt;"><i><%= "Find a match for your friend".Translate().ToUpper() %></i></h1>
            <br/>
            <asp:Button ID="lbFromSearch" runat="server" Width="150"  Height="40" Font-Size="Large"
                PostBackUrl="~/SearchList.aspx" CssClass="zurbtn large blue"/>
            <asp:Button ID="ibFromFriends" runat="server" Width="190" Height="40" Font-Size="Large"
                style="margin-left:20px;" PostBackUrl="~/FriendsList.aspx" CssClass="zurbtn large blue" />
            
        </td></tr>
        
    </table>            
</div>
        <script type="text/javascript">
            function clearSessionFriendToMatch() {
                var setSessionHandlerUrl = "./handlers/MatchTo.ashx?imgId=null&username= &";
                jQuery.post(setSessionHandlerUrl, function() {
                    jQuery("div#mmFriendImg").css({ background: "white url('<%= ImageHandler.CreateImageUrl(0, 200,200, false,true,true) %>') center center no-repeat" });
                    window.location.reload();
                });
                //__doPostBack(null, '');
            }
        </script>
<table cellspacing="2" style="width:100%" runat="server" id="pnlContent">
    <tr>
    <td style="width:75%">
        <div id="mmFriendImg" style="width:200px;height:200px; border:solid 1px DodgerBlue;
                    background:white url(<%= ImageHandler.CreateMatchToImageUrl(200,200) %>) center center no-repeat;">
            <div onclick="clearSessionFriendToMatch();" 
                 style="valign:top;float:right;background:DodgerBlue;color:white;display:inline;padding:0 0 2px 2px;text-align:right;cursor:pointer;"><b>X</b>
            </div>
        </div>
    </td>
    <td style="width:2%;height:120px;"/>
    <td runat="server" id="pnlIcons" visible="false"  style="width:20%;height:120px;text-align:center;">
        <table>
            <tr><td>
                <btn:SendWinkIcon ID="btnSendWinkIcon" runat="server" Gravity="e" Size="Mini" />
            </td></tr>
            <tr><td>
                <btn:SendGiftIcon ID="btnSendGiftIcon" runat="server" Gravity="e" Size="Mini" />
            </td></tr>
            <tr><td>
                <btn:SendMsgIcon ID="btnSendMsgIcon" runat="server" Gravity="e" Size="Mini" />
            </td></tr>
            <tr><td>
                <btn:MoreIcon ID="btnMoreIcon" runat="server"  Gravity="e" Size="Mini" Visible="false" />
            </td></tr>
        </table>
    </td>
    <td style="width:2%;height:120px;"/>
    </tr>
</table>

