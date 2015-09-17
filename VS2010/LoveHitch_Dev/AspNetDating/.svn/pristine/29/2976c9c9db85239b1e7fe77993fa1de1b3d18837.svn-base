<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="InviteFriends.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.InviteFriends" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <style type="text/css">
        h2
        {
            color: #C2272D !important;
        }
        h3
        {
            color: Navy !important;
        }
        h4
        {
            color: DodgerBlue !important;
        }
    </style>
    <div id="sidebar">
        <div style="border: 2px solid #4497CD; padding: 5px;">
            <div class="note">
                <components:ContentView ID="cvPromotion" runat="server" Key="FriendsInvitationPromotion">
                </components:ContentView>
            </div>
        </div>
        <br />
        <div style="border: 2px solid #4497CD; padding: 5px;">
            <div class="note">
                <components:ContentView ID="cvWhyShouldInviteFriends" runat="server" Key="FriendsInvitationPromotion" />
                <%--Key="WhyShouldInviteFriends" ></components:ContentView>--%>
            </div>
        </div>
    </div>
    <div id="main">
        <uc1:WideBoxStart ID="WideBoxStart1" runat="server"></uc1:WideBoxStart>
        <div id="divFacebook" runat="server">
            <%--<script type="text/javascript">
            window.fbAsyncInit = function() {
                FB.getLoginStatus(function(response) {
                    if (response.authResponse) {
                        // logged in and connected user, someone you know
                    } else {
                        FB.init({
                            appId: "<%= FacebookHelper.ApiKey %>", // App ID
                            //channelURL: '//WWW.YOUR_DOMAIN.COM/channel.html', // Channel File
                            status: true, // check login status
                            cookie: true, // enable cookies to allow the server to access the session
                            oauth: true, // enable OAuth 2.0
                            frictionlessRequests: true,
                            xfbml: true  // parse XFBML
                        });

                        FB.login(function(response) {
                            if (response.session) {
                                if (response.perms) {
                                    window.location = 'LoginThroughFacebook.aspx?access_token=' + response.session.access_token
                                } else {
                                    window.location = 'InviteFriends.aspx'
                                }
                            } else {
                            }
                        }, { scope: '<%=ConfigurationManager.AppSettings["FacebookPermissions"] %>' });
                    }
                });
            };
            // Load the SDK Asynchronously
            (function(d) {
                var js, id = 'facebook-jssdk'; if (d.getElementById(id)) { return; }
                js = d.createElement('script'); js.id = id; js.async = true;
                js.src = "//connect.facebook.net/en_US/all.js";
                d.getElementsByTagName('head')[0].appendChild(js);
            } (document));
            
//            function sendRequestToManyRecipients() {
//                var user_ids = document.getElementsByName("user_ids")[0].value;
//                FB.ui({method: 'apprequests',
//                  message: 'My Great Request',
//                  to: user_ids,
//                }, requestCallback);
//            }
        </script>--%>
            <script type="text/javascript">
                var fbml;
                function getDialogFbml() {
                    fbml =
                    '          <fb:fbml>' +
                    '              <fb:request-form' +
                    '                action=\"apps.facebook.com\lovehitch\default.aspx\"' +
                    '                method="POST"' +
                    '                invite="true"' +
                    //'                target="_top"' +
                    //'                email_invite="true"' +
                    "                type='LoveHitch'" +
                    "                content='This is an invitation from http://LoveHitch.com" +
                    "                         <fb:req-choice url=\"http://LoveHitch.com/default.aspx\" label=\"OK\" />' >" +
                    '                <fb:multi-friend-selector rows="15" cols="4" max="80" bypass="cancel" showborder="true"' +
                    '                    actiontext="Invite your friends to use LoveHitch."' +
                    '                    filters=["app_non_users"]' +
                    '                    exclude_ids=\"<c:out value=\'${appUserFriends}\' />\" >' +
                    '            </fb:request-form>' +
                    '          </fb:fbml>';
                    return fbml;
                }
                function showFbmlInviteDialog() {
                    fbml = getDialogFbml();
                    debugger;
                    $("#invite_box").html("<script type='text/fbml'>" + fbml + "<\/script>");
                    FB.XFBML.parse($("#box")[0]);
                }
                function fbmlInviteFriends() {
                    var obj = {
                        method: 'fbml.dialog',
                        fbml: getDialogFbml(),
                        size: { width: 700, height: 480 }, width: 700, height: 480
                    };
                    //var uiSize = FB.UIServer.Methods["fbml.dialog"].size;
                    //FB.UIServer.Methods["fbml.dialog"].size = { width: 760, height: 450 };

                    function dialogCallback(response) {
                        //FB.UIServer.Methods["fbml.dialog"].size = uiSize;
                    }
                    FB.ui(obj, dialogCallback);
                }

                function postToFeed() {
                    // calling the API ...
                    var obj = {
                        method: 'feed',
                        link: 'http://LoveHitch.com',
                        picture: 'lovehitch.com/Images/logo.png',
                        name: 'LoveHitch Feed',
                        caption: 'LoveHitch - Is time to update your status.',
                        description: 'Join me to the great Application LoveHitch, which help you to matchmaking between your friends. Inviting 10 Friends will give you 1 month of premium membership'
                    };

                    function feedCallback(response) {
                        //document.getElementById('msg').innerHTML = "Post ID: " + response['post_id'];
                    }

                    FB.ui(obj, feedCallback);
                }
                function sendRequestToManyRecipients() {
                    var obj = {
                        method: 'apprequests',
                        email_invite: "true",
                        link: 'http://LoveHitch.com',
                        title: 'LoveHitch - Is time to update your status.',
                        message: 'Invite you friends to LoveHitch on Facebook: 10 Friends that will join will give you 1 month of premium membership',
                        //invite: 'true',
                        //filters : "app_users",
                        app_id: "<%= FacebookHelper.ApiKey %>"
                    };
                    function sendRequestCallback(response) {
                        //console.log(response);
                    }
                    FB.ui(obj, sendRequestCallback);
                }
                function sendMessageToManyRecipients() {
                    var obj = { method: 'send', //'apprequests',
                        app_id: "<%= FacebookHelper.ApiKey%>",
                        name: 'LoveHitch - Is time to update your status.',
                        description: '10 Friends that will join will give you 1 month of premium membership.',
                        link: 'http://LoveHitch.com',
                        picture: 'lovehitch.com/Images/logo.png'
                    };
                    function msgRequestCallback(response) {
                        //console.log(response);
                    }
                    FB.ui(obj, msgRequestCallback);
                }
                var inviteDialog;
                function showInviteDialog(title) {
                    inviteDialog = new FB.UI.FBMLPopupDialog();
                    inviteDialog.showChoice(title, dialog_fbml);
                }
                function a139801212735949_inviteAction() {
                    inviteDialog.hide();
                }
                jQuery(function ($) {
                    $("#accordion").accordion({ autoHeight: false});
                });            
            </script>
            <%--<style type="text/css">
             .Generic_dialog_popup {width: 650px;} 
             .Contextual_dialog .Dialog_buttons .Inputbutton, .Inputsubmit {display: none;} 
        </style>--%>
            <%--<script type="text/javascript">
            FB.init({
                appId: "<%= FacebookHelper.ApiKey %>",
                status: true, // check login status
                cookie: true, // enable cookies to allow the server to access the session
                xfbml: true, // parse XFBML
                //channelURL: 'http://WWW.LoveHitch.COM/channel.html', // channel.html file
                oauth: true // enable OAuth 2.0
            });
            FB.login(function(response) {
                if (response.session) {
                    if (response.perms) {
                        window.location = 'LoginThroughFacebook.aspx?access_token=' + response.session.access_token
                    } else {
                        window.location = 'InviteFriends.aspx'
                    }
                } else {
                }
            }, { scope: '<%=ConfigurationManager.AppSettings["FacebookPermissions"] %>' });
            //            (function(d, s, id) {
            //                var js, fjs = d.getElementsByTagName(s)[0];
            //                if (d.getElementById(id)) { return; }
            //                js = d.createElement(s); js.id = id;
            //                js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
            //                fjs.parentNode.insertBefore(js, fjs);
            //            } (document, 'script', 'facebook-jssdk'));
        </script>--%>
            <%--<div id="divFacebookFriends" style="width: 90%;" runat="server" >
		    <fb:serverfbml  >
			    <fb:js-string var="dialog_fbml">
			        <div id="dialog_content" style="">
				          <fb:fbml>
					          <fb:request-form
						        action = "javascript: a139801212735949_inviteAction()"
						        method="POST"
						        invite="true"
						        type='<%= "LoveHitch".Translate() %>'
						        content="<%= "This is an invitation from http://www.LoveHitch.com".Translate() %>
						        <% = Server.HtmlEncode("<fb:req-choice url=\"" +Config.Urls.Home + "/default.aspx" + "\" label=\"OK\" />")  %> " 
						      >
						            <fb:multi-friend-selector rows="15" cols="4" bypass="cancel" showborder="true"
							            actiontext="<%= "Invite your friends to use LoveHitch.".Translate() %>"
							            exclude_ids="<c:out value='${appUserFriends}' />" >
					        </fb:request-form>
				          </fb:fbml>
				    </div>
			    </fb:js-string>
		    </fb:serverfbml>
		    
	    </div>--%>
            <%--<script type="text/javascript">
	        FB_RequireFeatures(["XFBML"], function() {
	            FB.Facebook.init("<%= FacebookHelper.AppID %>", "Facebook/xd_receiver.htm");
	        });
        </script>--%>
            <div id="divFacebookButton" runat="server" class="wrap-sized fl">
                <table>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <h2>
                                <%= Lang.Trans("(1) - Enter Your Personal Invitation Message").ToUpper() %></h2>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEditMessage" runat="server" TextMode="MultiLine" Rows="5" Columns="80"
                                Width="550" Height="150" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="fl">
                            <%--<div class="fckeditor">
                            <asp:PlaceHolder ID="phEditor" runat="server" Visible="true"></asp:PlaceHolder>
                        </div>--%>
                            <asp:TextBox ID="advancedHtmlEditor" runat="server" TextMode="MultiLine" Enabled="true"
                                ReadOnly="false" Wrap="false" Columns="80" Rows="8"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <%--<f:view contenttype="text/html">
                <fb:login-button onlogin="window.location='InviteFriends.aspx'">
                </fb:login-button>
            </f:view>--%>
            <%--<input id ="pop-dialog-shower" type="button" value="Invite!" onclick="showFbmlInviteDialog();" /> 
            <input type="button" value="Message To Your Facebook Friends" 
                   onclick="fbmlInviteFriends(); return false;"/>--%>
            <div style="clear:both;">
                <br />
                <h2>
                    <%= Lang.Trans("(2) - Choose Your Method").ToUpper() %></h2>
                <br />
                <div id="accordion">
                    
                    <h3>
                        <a href='#'>
                            <%= " A - Invite Your Friends Through Facebook".Translate().ToUpper() %></a></h3>
                    <div style='height: 250px !important;'>
                        <%= String.Format("<span onclick='sendRequestToManyRecipients(); return false;'><u>{0}</u></span>   ","Click Here To Send Facebook Application Invitation To Your Friends".Translate()) %>
                        <%--<input type="button" value='<%= "Invite Friends".Translate()%>' style="cursor:pointer;width:90px; background:#ECEEF5;color:#3B5998;border-color:#CAD4E7"
                   onclick="sendRequestToManyRecipients(); return false;"/><br />--%>
                        <br />
                        <br />
                        <%--<%= String.Format("{0}   ", "By Publishing on Your Facebook Wall".Translate()) %>--%>
                        <%--<%= String.Format("{0} <br/><br/>  ", "You Can LIKE US Or PUBLISH Message On Your Facebook WALL".Translate()) %>--%>
                        <%--<input type="button" value="Publish" style="width:120px"
                   onclick="postToFeed(); return false;"/>
            <br /><br />--%>
                        <%--<div class="fb-send" data-href="http://LoveHitch.com" data-font="arial"></div>--%>
                        <div class="fb-like" data-href="http://LoveHitch.com" data-send="true" data-width="400"
                            data-show-faces="true" data-font="arial">
                        </div>
                        <script type="text/javascript">
                            jQuery(function ($) {
                                $(".fb-like").attr('data-href', 'http://LoveHitch.com?invitedBy=' + '<%= GetCurrentUserSession().Username %>');
                            });
                        </script>
                        <br />
                    </div>
                    <%--<div id='box'>
            <fb:serverfbml style="width: 500px; height:200px;" id='invite_box'></fb:serverfbml>
        </div>--%>
                    <%--<div id="plhInviteFriends" style="width:90%;height:650px;"></div>--%>
                    <h3>
                        <a href='#'>
                            <%= " B -  Send an Email".Translate().ToUpper() %></a></h3>
                    <div style='height: 350px !important;'>
                        <div class="wrap-sized">
                            <asp:MultiView ID="mvImportFriends" runat="server" ActiveViewIndex="0">
                                <asp:View ID="viewEmail" runat="server">
                                    <br />
                                    <br />
                                    <h4>
                                        <%= "Import Contacts From Your Email Account".Translate().ToUpper() %></h4>
                                    <br />
                                    <%--<%= "The Easiest Way - You can import contacts from your email account".Translate() %><br />--%>
                                    <table>
                                        <tr>
                                            <td>
                                                <%= Lang.Trans("Email address:") %>&nbsp;
                                            </td>
                                            <td>
                                                <input class="l_form_txt" type="text" id="txtEmail" runat="server" />@
                                                <asp:DropDownList CssClass="l_form_txt" ID="ddEmailProvider" runat="server">
                                                    <asp:ListItem>gmail.com</asp:ListItem>
                                                    <asp:ListItem>yahoo.com</asp:ListItem>
                                                    <asp:ListItem>live.com</asp:ListItem>
                                                    <asp:ListItem>hotmail.com</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= Lang.Trans("Password:") %>&nbsp;
                                            </td>
                                            <td>
                                                <input class="l_form_txt" id="txtPassword" type="password" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="srvmsg">
                                                    <asp:Label ID="lblImportError" CssClass="InfoMessage" EnableViewState="false" runat="server"
                                                        Visible="false"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="buttons">
                                                    <%= "NOTE: We don't store your email and password information.".Translate() %>
                                                    <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" CssClass="red zurbtn fr" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="viewMessage" runat="server">
                                    <asp:DataGrid ID="dgContacts" runat="server" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="&lt;input type=checkbox onClick=&quot;a=0;for(i=0; i&lt;this.form.elements.length;i++){if(this.form.elements[i]==this) {a=3}; if ((this.form.elements[i].type=='checkbox') &amp;&amp; (a!=0) &amp;&amp; (i&gt;a)) {this.form.elements[i].checked=this.checked}}&quot;&gt;">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <input type="checkbox" id="cbSelect" value='<%# Eval("Email") %>' runat="server"
                                                        name="cbSelect" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <br />
                                    <div class="separator">
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                        <div class="wrap-sized">
                            <table>
                                <tr>
                                    <td>
                                        <table style="width: 450px;">
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                    <h4>
                                                        <%= "add your friends email manually".Translate().ToUpper() %></h4>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <%= Lang.Trans("Emails") %>&nbsp;
                                                </td>
                                                <td>
                                                    <input class="l_form_txt" type="text" id="txtFriendsEmail" runat="server" style="width: 350px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="srvmsg">
                                            <asp:Label ID="lblError" CssClass="InfoMessage" runat="server" Visible="false"></asp:Label></div>
                                        <div class="buttons">
                                            <asp:Button ID="btnSubmit" TabIndex="1" runat="server" CssClass="medium red zurbtn fr">
                                            </asp:Button>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender2" TargetControlID="advancedHtmlEditor"
                runat="server" Enabled="true" DisplaySourceTab="false">
                <Toolbar>
                    <ajaxToolkit:Bold />
                    <ajaxToolkit:Italic />
                    <ajaxToolkit:Underline />
                    <ajaxToolkit:JustifyLeft />
                    <ajaxToolkit:JustifyRight />
                    <ajaxToolkit:CreateLink />
                    <ajaxToolkit:UnLink />
                    <ajaxToolkit:BackgroundColorSelector />
                    <ajaxToolkit:ForeColorSelector />
                    <ajaxToolkit:FontNameSelector />
                    <ajaxToolkit:FontSizeSelector />
                    <ajaxToolkit:Indent />
                    <ajaxToolkit:Outdent />
                </Toolbar>
            </ajaxToolkit:HtmlEditorExtender>
        </div>
        <uc1:WideBoxEnd ID="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
    </div>
</asp:Content>
