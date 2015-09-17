<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="LoginBox.ascx.cs" Inherits="AspNetDating.Components.LoginBox"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<script type="text/javascript">
    function postbackOnEnterKeyDown() {
        if (event.which || event.keyCode) {
            if ((event.which == 13) || (event.keyCode == 13)) {
                cancelEvent(event);
                var fireOnThis = document.getElementById("<%= fbLogin.ClientID%>");
                var evObj = document.createEvent('MouseEvents');
                //debugger;
                //evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
                evObj.initEvent('click', true, true);
                fireOnThis.dispatchEvent(evObj);
            }
        }
    }
</script>
<div class="LoginBox">
    <%--<div>--%>
    <%--<div class="LoginBoxTop">
        <h2 class="LoginBoxHeader">
            <%= Lang.Trans("login | register") %>
        </h2>
    </div>--%>
    <asp:Button ID="fbRegister" runat="server" TabIndex="7" Width="180" Height="60"
            CssClass="JoinForFreeBtn" OnClick="fbRegister_Click" SkinID="Register"/>
    <div class="clear3"></div>
    <asp:Panel ID="pnlLogin" runat="server" CssClass="LoginBoxContent tipsy" DefaultButton="fbLogin" >
        <div class="loginbtnswrap" id="pnlLoginButtons" runat="server">
            <span id="divCardSpace" class="cardspace" runat="server">
                <a href="#" onclick="window.open('<%= Config.Urls.SecureHome + "/LoginCardSpace.aspx" %>', 'cardspace', 'width=300,height=210,resizable=0,menubar=0,status=0,toolbar=0'); return false;"><img src='images/InfoCardBtn.png' alt='<%= Lang.Trans("Use information card") %>' title='<%= Lang.Trans("Use information card") %>' /></a>
            </span>
            <span id="divMySpace" runat="server" class="myspacebtn">
                <asp:ImageButton ID="btnUseMySpace" ImageUrl="~/images/MySpaceBtn.png" runat="server" 
                onclick="btnUseMySpace_Click" />
            </span>
            <div id="divFacebook" class="facebookbtn" runat="server" visible="false">
                <%--<f:view contentType="text/html">
                    <fb:login-button onlogin="window.location='LoginThroughFacebook.aspx'"></fb:login-button>
                </f:view>
                <script type="text/javascript">
                    FB.init("<%= AspNetDating.Classes.FacebookHelper.ApiKey %>", "Facebook/xd_receiver.htm");
                </script>--%>
                <%--<div class="fb-login-button" data-show-faces="true" data-width="200" data-max-rows="1"></div>--%>
            </div>
        </div><br />
        <table cellpadding="3" cellspacing="3" class="LoginForm w100">
            <tr>
                <td class="w30" valign="middle">
                    <%= Lang.Trans("Username") %></td>
                <td valign="middle" class="w70">
                    <asp:TextBox ID="txtUsername" TabIndex="1" onkeydown="postbackOnEnterKeyDown();" runat="server"></asp:TextBox>
                </td>
                <td rowspan="2" valign="middle" class="w95">
			        <uc1:FlexButton ID="fbLogin2" Visible="false" TabIndex="5" CssClass="FormLoginBtn" runat="server" OnClick="fbLogin_Click" SkinID="FormLogin" />                
                    <uc1:FlexButton ID="fbForgotPassword2" Visible="false" TabIndex="6" runat="server" SkinID="FormForgotPassword" CssClass="PassLink" OnClick="fbForgotPassword_Click" />
                </td>
            </tr>
            <tr>
                <td valign="middle" class="w30">
                    <%= Lang.Trans("Password") %></td>
                <td valign="middle" class="w70">
                    <asp:TextBox ID="txtPassword" TabIndex="2" onkeydown="postbackOnEnterKeyDown();"  runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="LoginModes">
            <asp:CheckBox ID="cbStealthMode" TabIndex="3" CssClass="StealthMode" runat="server"></asp:CheckBox>
            <asp:CheckBox ID="cbRememberMe" TabIndex="4" runat="server" />
        </div>
        <uc1:FlexButton ID="fbLogin" runat="server" TabIndex="5" CssClass="medium zurbtn blue" 
            RenderAs="LinkButton"  OnClick="fbLogin_Click" />
        <uc1:FlexButton ID="fbForgotPassword" runat="server" TabIndex="6" 
            SkinID="ForgotPassword" CssClass="PassLink" OnClick="fbForgotPassword_Click" />        
    </asp:Panel>
</div>