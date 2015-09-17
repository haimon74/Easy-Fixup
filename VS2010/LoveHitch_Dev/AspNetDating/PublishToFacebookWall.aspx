<%@ Page Title="" 
         Language="C#" 
         MasterPageFile="~/Site.Master" 
         AutoEventWireup="true" 
         CodeBehind="PublishToFacebookWall.aspx.cs" 
         Inherits="AspNetDating.PublishToFacebookWall" %>
         
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div id="divFacebook" runat="server" style="height:500px;">
    <div id="fb-root"></div>
    <script src="https://connect.facebook.net/en_US/all.js" type="text/javascript"></script>
    <script type="text/javascript">
        FB.init({
            appId: "<%= AspNetDating.Classes.FacebookHelper.ApiKey %>",
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true, // parse XFBML
            channelURL: 'https://WWW.LoveHitch.COM/channel.html', // channel.html file
            oauth: true // enable OAuth 2.0
        });
        FB.login(function(response) {
        	if (response.session) {
        		if (response.perms) {
        			window.location='LoginThroughFacebook.aspx?access_token='+response.session.access_token
        		} else {
        			window.location='Default.aspx'
        		}
        	} else {
        	}
        }, {scope:"<%=ConfigurationManager.AppSettings["FacebookPermissions"] %>"});
    </script>
    <div id="divFacebookButton" runat="server" style="text-align: center;">
        <f:view contentType="text/html">
            <fb:login-button onlogin="window.location='PublishToFacebookWall.aspx'"></fb:login-button>
        </f:view>
        
    </div>
    <div id="divFacebookPermission" style="width: 90%;" runat="server" visible="true">
        <%--<script type="text/javascript">
            (function(d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) { return; }
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                fjs.parentNode.insertBefore(js, fjs);
            } (document, 'script', 'facebook-jssdk'));
        </script>
        <div class="fb-send" data-href="https://lovehitch.com" data-font="arial"></div>--%>
        
        <%--<input type="button" onclick="postFeed();" value="Publish" class="SendBtn"/>--%>
        <script type="text/javascript">
            //function postFeed() {
            jQuery(function($) {
                FB.ui(
                  {
                      method: 'feed',
                      name: 'LoveHitch Link',
                      link: 'https://apps.facebook.com/LoveHitch/home.aspx',
                      //picture: 'https://apps.facebook.com/lovehitch_local/favicon.ico',
                      caption: 'LoveHitch',
                      description: 'Here is my invitation to the great application of LoveHitch.'
                  },
                  function(response) {
                      if (response && response.post_id) {
                          alert('Post was published.');
                          window.location = "./home.aspx";
                      } else {
                          alert('Post was not published.');
                          window.location = "./InviteFriendsFromFacebook.aspx";
                      }
                  }
                );
            });
       </script>
       <%--<script type="text/javascript">
//            {
//                      method: 'send',
//                      name: 'Enjoying at LoveHitch',
//                      link: 'https://apps.facebook.com/lovehitch_local/home.aspx'//'https://www.LoveHitch.com'
//                  },
//            function statusSubmit(status) {
//                facebook_prompt_permission('email, publish_stream', function(accepted) {
//                    if (accepted) {
//                        // User (already) has permission
//                        alert(status + 'already granted');
//                    }
//                    else {
//                        //  User does not have permission
//                        alert(status + ' not granted');
//                    }
//                });
//            }
//            function facebook_prompt_permission(permission, callbackFunc) {
//                FB.Connect.showPermissionDialog(permission,
//                    function(result) {
//                        if (null == result)
//                            alert('no permissons granted');
//                        else
//                            alert('permissions ' + result);
//                    }, true, null);                        
//            }
        </script>--%>
    </div>
</div>
</asp:Content>
