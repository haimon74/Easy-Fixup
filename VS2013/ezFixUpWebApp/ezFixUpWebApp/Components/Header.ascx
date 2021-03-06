﻿<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Control Language="c#" 
            AutoEventWireup="True" 
            CodeBehind="Header.ascx.cs" 
            Inherits="ezFixUp.Components.Header" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLogin" Src="~/Components/HeaderLogin.ascx" %>

<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    
    function setActiveNavCookie(value) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + 1);
        var c_value = escape(value) + "; expires=" + exdate.toUTCString();
        document.cookie = "active_nav" + "=" + c_value;
    }
    
    function getActiveNavCookie() {
        var i, x, y, ARRcookies = document.cookie.split(";");
        for (i = 0; i < ARRcookies.length; i++) {
            x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
            y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
            x = x.replace(/^\s+|\s+$/g, "");
            if (x == "active_nav") {
                return unescape(y);
            }
        }
    }

    var lnkLogout = "#" + "<%=lnkLogout.ClientID%>";
    var topMenuBgColor = "<%= (MatchmakerHelper.IsMatchmakerState) ? "#0163A2" : "dodgerblue" %>";    
    jQuery(function($) {
        $('td.facebookbtn').css({display:''});
    });
</script>
<div id="header">
    <table class="logo_header">
        <tr  style="vertical-align:top;text-align:left;width:99%;">
            <td id="pnlLanguage" class="language SubheaderColor" runat="server" style="float:left;width:130px;">
                <asp:DropDownList ID="ddLanguages" Width="120px" CssClass="dropdownlist" runat="server"
                    AutoPostBack="True" OnSelectedIndexChanged="ddLanguages_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td id="divFacebookLike" class="facebookbtn" runat="server" style="margin-top:6px;float:left;display:none;" >
                <asp:Literal ID="ltrFacebookScript" runat="server">
                    <%--<script src="https://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script> --%>        
                </asp:Literal>
                
                <f:view contentType="text/html">
                    <%
                        bool userAuthenticated = FacebookHelper.IsCurrentSessionConnected && PageBase.IsUserAuthorized();
                        if (!userAuthenticated)
                       {%>
                    <div id="fbConnectBtn" style="margin-left:5px;float:left;">
                        <fb:login-button font="verdana" 
                            onlogin="facebookLoggedIn();">Connect</fb:login-button>
                    </div>
                    <%
                        }
                       else
                       {
                            if (Page is _default) Server.Transfer(MatchmakerHelper.CurrentHomePage);
                            %>
                    <div class="fr" style="margin-left:3px;">
                        <fb:like href="https://ezFixUp.com"  
                             send="true" layout="button_count" width="300" show_faces="false" 
                             action="like" font="verdana" class="fl"></fb:like>
                    </div>
                    <%
                       }%>
                </f:view>
            </td>
            <td id="pnlLoginView" class="logout SubheaderColor w50 fr" runat="server" style="margin:3px 0 0 0;text-align:right;" visible='true'>
                <div id="beforeLoginView" runat="server"> 
                    <uc1:HeaderLogin ID="headerLogin1" runat="server" Visible="true" CssClass="tipsy"></uc1:HeaderLogin>
                </div>
                <div id="welcomeLogout" runat="server"> 
                    <asp:Label ID="lblWelcome" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblCredits" runat="server"></asp:Label>
                    <asp:LinkButton ID="lnkLogout" CssClass="hlinks SkinSubheaderLinkColor" runat="server" ForeColor="Navy" Font-Bold="false"
                        OnClick="lnkLogout_Click" style="display:none;"></asp:LinkButton>&nbsp;&nbsp;
                </div>                
            </td>
        </tr>
        <tr>
            <td colspan="3">                
                <div style='100%;display:block;'>
                        <div style='width:50%;float:left;'>
                            <a href="./default.aspx" alt="home page" style="height:100px;display:block;">
                                <img src='./images/logo.png' alt='ezFixUp logo' style='float:left;'/> 
                            </a>
                        </div>
                        <div style='color:#66CCFF;font-size:24px;width:312px;height:50px;line-height:40px;display:block;float:right;'>
                            <span style='float:left;'><%= (MatchmakerHelper.IsMatchmakerState) ? "Matchmaker Mode".Translate() : "" %></span>
                        </div>
                        <div style='30%;float:right;color:white;background:#66CCFF;padding:10px; font-size:24px; '>
                            <div  class='fr'>       
                                <a href='./contactus.aspx' class='zurbtn red medium'><%= "Bug Reporting".Translate() %></a>                            
                            </div>
                            <div class='fr' style='margin: 5px  10px 0px 10px'><%= "BETA VERSION".Translate() %> </div>
                        </div>
                </div>                
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript" >
    function docReadyAlert() { alert('doc ready!'); }
    function winLoadAlert() { alert('win load!'); }
    //jQuery(document).ready(docReadyAlert);
    //jQuery(window).load(winLoadAlert);
//    jQuery.fn.loadExtended = function(url, completeCallback) {
//        return this.load(url, function(responseText, textStatus, XMLHttpRequest) {
//            if (completeCallback !== undefined && completeCallback !== null) {
//                completeCallback(responseText, textStatus, XMLHttpRequest);
//            }
//            //$(this).trigger("ready");
//            //var e1 = new jQuery.Event("ready");
//            //$(document).trigger(e1);
//            //$(readylist).each(function() { this(); });
//            var e2 = new jQuery.Event("load");
//            $(window).trigger(e2);
//        });
//    };
    jQuery('document').ready(function () {
        jQuery('#navi ul li').on('click', 'a', function (e) {
            var setSessionStateServiceUrl = "./handlers/SetSessionState.ashx";
            var href = jQuery(this).attr('_href');
            jQuery(this).attr('onclick', '');
            setActiveNavCookie(href);
            jQuery.get(setSessionStateServiceUrl, { 'key': 'CurrentPageName', 'val': href }, function (x) {
                window.location = href;
                //                $('div#content').loadExtended(href + ' #content', function() {
                //                    $('#aspnetForm').attr('action', href);
                //                });
            });
        });
    });
    
</script>
<div id="pnlNavi" class="navi_container" runat="server">
    <div id="navi">
    <ul class="fl" style='margin:0 12px;'>
        <li id="liHomePage" runat="server">
            <a href='<%= MenuItems[liHomePage] %>' _href='<%= MenuItems[liHomePage] %>'
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Home") %>
            </a>
        </li>
        <li id="liFavorites" runat="server" >
            <a href="<%= MenuItems[liFavorites] %>" _href="<%= MenuItems[liFavorites] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Favorites") %>
            </a>
        </li>
        <li id="liFriends" runat="server" >
            <a href="<%= MenuItems[liFriends] %>" _href="<%= MenuItems[liFriends] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Friends") %>
            </a>
        </li>
        <li id="liSearch"  runat='server'>
            <a href="<%= MenuItems[liSearch] %>" _href="<%= MenuItems[liSearch] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Search") %>
            </a>
        </li>
        <li id="liProfile" runat="server" >
            <a href="<%= MenuItems[liProfile] %>" _href="<%= MenuItems[liProfile] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Profile") %>
            </a>
        </li>
        <li id="liMailbox" runat="server" >
            <a href="<%= MenuItems[liMailbox] %>" _href="<%= MenuItems[liMailbox] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= String.Format("{0}({1})", Lang.Trans("Mailbox"), 
                    Message.SearchUnread(CurrentUserSession.Username) == null 
                                    ? 0 : Message.SearchUnread(CurrentUserSession.Username).Length)%>
            </a>
        </li>
        <li id="liMatchmaker" runat="server" 
            style="background-color:#D0222C !important;margin:0 3px;">
           <a href="<%= MenuItems[liMatchmaker] %>" _href="<%= MenuItems[liMatchmaker] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent" style='color:WhiteSmoke;'>
                <%= Lang.Trans((string)Global.GetSessionState()["MatchmakerMenuItemName"])%>
             <div id='cupidon' ></div></a>
        </li>
        <li id="liSubscription" runat="server" >
            <a href="<%= MenuItems[liSubscription] %>" _href="<%= MenuItems[liSubscription] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Billing") %>
            </a>
        </li>
        <li id="liInvite" runat="server" >
            <a href="<%= MenuItems[liInvite] %>" _href="<%= MenuItems[liInvite] %>" 
                onclick="cancelEvent(event); setActiveNavCookie(this.href); window.location = this.href; return false;"
                class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Invite Friends") %>
            </a>
        </li>
        <asp:Repeater ID="rptPages" runat="server">
            <ItemTemplate>
                <li><a href='<%# Eval("URL") == null ? UrlRewrite.CreateContentPageUrl((int)Eval("ID")) : (string) Eval("URL")%>'
                    class="skinnavlinkcolor skinheaderlinkcolor">
                    <%# Eval("Title")%></a></li>
            </ItemTemplate>
        </asp:Repeater>
        <li id="tdTopCharts" runat="server"><a href="TopCharts.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Top Charts") %>
        </a></li>
        <li id="tdReviewNewProfiles" visible="false" runat="server"><a href="ReviewNewUsers.aspx"
            class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Approve profiles") %>
        </a></li>
        <li id="tdReviewNewPhotos" visible="false" runat="server"><a href="ReviewNewPhotos.aspx"
            class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Approve photos") %>
        </a></li>
        <li id="tdBlog" runat="server"><a href="Blogs.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Blog") %>
        </a></li>
        <li id="tdGroups" runat="server" >
            <a href="Groups.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Groups") %>
            </a>
        </li>
        <li id="tdVideos" runat="server" >
            <a href="BrowseVideos.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Videos") %>
            </a>
        </li>
        <li id="tdRatePhotos" runat="server" >
            <a href="RatePhotos.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Rate photos") %>
            </a>
        </li>
        <li id="tdAds" runat="server" >
            <a href="Ads.aspx" class="skinnavlinkcolor skinheaderlinkcolor parent">
                <%= Lang.Trans("Classifieds") %>
            </a>
        </li>
        <li id="tdAjaxChat" visible="false" runat="server">
            <asp:LinkButton ID="lnkAjaxChatPay" runat="server" class="skinnavlinkcolor skinheaderlinkcolor parent"
                OnClick="lnkAjaxChatPay_Click">
		        <%= Lang.Trans("Chat") %>
            </asp:LinkButton>
            <a id="lnkAjaxChat" class="skinnavlinkcolor skinheaderlinkcolor parent" href="~/AjaxChat/ChatWindow.aspx"
                target="_ajaxchat" runat="server">
                <%= Lang.Trans("Chat") %>
            </a></li>
        <li id="tdContests" visible="false" runat="server" style="display: none;"><a href="PhotoContests.aspx"
            class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Contests") %>
        </a></li>
        <li id="tdBroadcast" visible="false" runat="server"><a href="BroadcastVideo.aspx"
            class="skinnavlinkcolor skinheaderlinkcolor parent">
            <%= Lang.Trans("Broadcast") %>
        </a></li>
        </ul>
        <table id="liOnlineUsers" runat="server" onmouseover="return false;"  class="fr"
                 style="padding-top:3px;display:table; width:auto;
                        color:white;height:33px;text-align:center;margin:0 10px;">
            <tbody style="display: table-row-group;height:27px;">
            <tr style="display: table-row;height:33px;">
            <td style="width:42px;line-height:13px; display:table-cell;">
                    <%= Lang.Trans("Now<br>Online")%></td>
            <td style="padding:5px;width:auto;vertical-align:middle; display:table-cell;">
                    <span style='color:#003A5C;background-color:white;font-size:larger;padding:2px 3px;'><b><%= OnlineUsersCount%></b></span></td>
            </tr>            
            </tbody>
        </table>
    </div>    
</div>
<p></p>
<div class="separator6"></div>
    
    

