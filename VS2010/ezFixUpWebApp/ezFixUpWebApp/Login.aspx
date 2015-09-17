<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ezFixUp.Login" Async="true" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        function postbackOnEnterKeyDown() {
            if (event.which || event.keyCode) {
                if ((event.which == 13) || (event.keyCode == 13)) {
                    var fireOnThis = document.getElementById("<%= fbLogin.ClientID%>");
                    var evObj = document.createEvent('MouseEvents');
                    evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
                    fireOnThis.dispatchEvent(evObj);
                }
            }
        }
    </script>
    <uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
        <div class="clear3"></div>
        <asp:Panel ID="pnlLogin" runat="server" DefaultButton="fbLogin">
            <div class="loginbtnswrap">
                <span id="divCardSpace" class="cardspace" runat="server">
                    <a href="#" onclick="window.open('<%= Config.Urls.SecureHome + "/LoginCardSpace.aspx" %>', 'cardspace', 'width=300,height=210,resizable=0,menubar=0,status=0,toolbar=0'); return false;"><img src='images/InfoCardBtn.png' alt='<%= Lang.Trans("Use information card") %>' title='<%= Lang.Trans("Use information card") %>' /></a>                
                </span>
                <span id="divMySpace" runat="server" class="myspacebtn">
                    <asp:ImageButton ID="btnUseMySpace" ImageUrl="~/images/MySpaceBtn.png" runat="server" 
                    onclick="btnUseMySpace_Click" />
                </span>
                <span id="divFacebook" runat="server" class="facebookbtn" visible="false">
                    <%--<f:view contentType="text/html">
                        <fb:login-button onlogin="window.location='LoginThroughFacebook.aspx'"></fb:login-button>
                    </f:view>
                    <script type="text/javascript">
                        window.onload = function() {
                            jQuery(document).ready(function() {
                                FB.init("<%= ezFixUp.Classes.FacebookHelper.ApiKey %>", "Facebook/xd_receiver.htm");
                            });
                        };
                    </script>    	--%>
                </span>
            </div>  
            <div class="srvmsg"><asp:Label ID="lblError" CssClass="InfoMessage" runat="server" Visible="false"></asp:Label></div>
            
            <uc1:FlexButton ID="fbRegister" runat="server" RenderAs="Button" CssClass="RegisterLink" OnClick="lnkRegister_Click" SkinID="InnerRegister"/>
            
            <table cellpadding="2" cellspacing="0" width="100%" class="LoginForm">
                <tr>
                    <td align="right" valign="middle" class="FormLabel" ><%= Lang.Trans("Username") %></td>
                    <td align="left" valign="middle" class="FormField">
                        <asp:TextBox ID="txtUsername"  onkeydown="postbackOnEnterKeyDown();" runat="server" Width="94%"></asp:TextBox>	
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="middle" class="FormLabel"><%= Lang.Trans("Password") %></td>
                    <td align="left" valign="middle" class="FormField">
                        <asp:TextBox ID="txtPassword"  onkeydown="postbackOnEnterKeyDown();"  runat="server" TextMode="Password" Width="94%" ></asp:TextBox>
                    </td>
                </tr>
            </table>				
            <div class="LoginModes">
                <asp:CheckBox ID="cbStealthMode" CssClass="StealthMode" runat="server"></asp:CheckBox>
                <asp:CheckBox ID="cbRememberMe" runat="server" />
            </div>
            <uc1:FlexButton ID="fbLogin" runat="server" CssClass="medium zurbtn blue" RenderAs="LinkButton" 
                  OnClick="btnLogin_Click"/>
            <uc1:FlexButton ID="fbForgotPassword" CssClass="PassLink" runat="server" RenderAs="Button" OnClick="lnkForgotPassword_Click" SkinID="InnerForgotPassword"/>
            <div class="LoginText">
                <components:ContentView ID="cvLoginBoxNotes" Key="LoginNotes" runat="server">
                </components:ContentView>
            </div>
        </asp:Panel>
    <uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>
