<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="Register.aspx.cs" 
    AutoEventWireup="True" Inherits="AspNetDating.Register" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LoginBox" Src="Components/LoginBox.ascx" %>
<%@ Register Src="Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="NewsBox" Src="Components/NewsBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<%@ Register Src="Components/BirthdayPicker.ascx" TagName="BirthdayPicker" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<script type="text/javascript">
    asyncScripts.push('scripts/pages/register.js');
</script>
<script type="text/jscript">

    var asyncImageId = "<%= asyncImageUpload.ClientID %>";
    //alert(asyncImageId);
    var MatchmakerPopTitleTxt = '<%= "Play Matchmaker".Translate()%>';
    var dialog_flag = true;
    var contentTxt;

</script>

    <div id="continue_popup" style="width: 300px; height: 300px; background: green; display: none;">
    </div>
    <div id="mmDescContent" style="display:none;">
        <components:ContentView ID="cvMatchmakerDescription" Key="WhatIsMatchmakerDescription" runat="server"></components:ContentView>
    </div>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/scripts/pages/Register.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div id="layout_content" style="width: 100%">
        <div id="registration_rt" class="registration_rt">
            <img id="play_mm" src="Images/register_form/play_mm_pop_bg.png" style="display: none;"
                alt="play matchmaker" />
        </div>
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
        <div class="separator6">
        </div>
                <div id="divLogin" runat="server" visible="false">
                    <table>
                        <tr>
                            <td>
                                <%= "If you already have LoveHitch account please continue to login".Translate() %>
                            </td>
                            <td>
                                <asp:Button ID="btnLogin" PostBackUrl="~/Login.aspx?facebook=1" runat="server" CssClass="LoginBtn" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="loginbtnswrap">
                    <span id="divCardSpace" class="cardspace" runat="server"><a href="#" onclick="window.open('<%= Config.Urls.SecureHome + "/LoginCardSpace.aspx" %>', 
                'cardspace', 'width=300,height=210,resizable=0,menubar=0,status=0,toolbar=0'); return false;">
                        <img src='images/InfoCardBtn.png' alt='<%= Lang.Trans("Use information card") %>'
                            title='<%= Lang.Trans("Use information card") %>' />
                    </a></span><span id="divMySpace" class="myspacebtn" runat="server">
                        <asp:ImageButton ID="btnUseMySpace" ImageUrl="~/images/MySpaceBtn.png" runat="server"
                            OnClick="btnUseMySpace_Click" />
                    </span>
                    <div id="divFacebook" runat="server" class="facebookbtn" visible="false">
                        <f:view contenttype="text/html">
                            <fb:login-button 
                                onlogin="window.location='Register.aspx?facebook=1'"
                                registration-url="https://developers.facebook.com/docs/plugins/registration"
                                >
                            </fb:login-button>                            
                        </f:view>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="registration">
                    <div id="registration_lt" class="registration_lt">
                        <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry"
                            Category="Country" EnableViewState="true" ServicePath="~/Services/Service.asmx"
                            ServiceMethod="GetCountries" />
                        <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
                            Category="Region" EnableViewState="true" ServicePath="~/Services/Service.asmx"
                            ServiceMethod="GetRegionsByCountry" ParentControlID="dropCountry" />
                        <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
                            Category="City" ServicePath="~/Services/Service.asmx" LoadingText="[Loading cities...]"
                            ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" EnableViewState="true" />
        <asp:UpdatePanel ID="UpdatePanelRegister" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="asyncImageUpload" EventName="UploadedComplete" />
                <asp:AsyncPostBackTrigger ControlID="btnRegister" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                        <div class="srvmsg">
                            <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label>
                        </div>
                                    </ContentTemplate>
        </asp:UpdatePanel>

                        <asp:MultiView ID="multiViewButtons" runat="server" ActiveViewIndex="0" EnableViewState="true">
                            <asp:View ID="view1" runat="server">
                                <table class="regtable" cellpadding="5" cellspacing="2">
                                    <tr>
                                        <td class="reglabel" style="vertical-align: top">
                                            <br />
                                            <asp:Label ID="lblPhoto" runat="server"></asp:Label>
                                        </td>
                                        <td style="vertical-align: bottom">
                                            <div id="updDiv" class="updDiv fl">
                                                <img id="loadedImage" alt="Avatar" width="90px" height="90px" src="<%= LastAsyncFileUploadLocation %>" />
                                            </div>
                                            <div style="margin-top: 67px;">
                                                <ajaxToolkit:AsyncFileUpload OnClientUploadError="uploadError" OnClientUploadComplete="uploadComplete"
                                                    runat="server" ID="asyncImageUpload" Width="150px" UploaderStyle="Traditional"
                                                    UploadingBackColor="lightskyblue" ThrobberID="myThrobber" Style="margin-top: 60px;
                                                    opacity: 0; position: absolute; left: -150px;" />
                                                &nbsp;
                                                <button id="btnBrowseTrigger" onclick="javascript:triggerAsyncImgUpload();return false;" class="BrowseBtn">
                                                    <%= "Browse".Translate() %></button>
                                                <asp:Label runat="server" ID="myThrobber" Style="display: none;">
                                            <img align="middle" alt="" src="images/uploading.gif" />
                                                </asp:Label>
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="reglabel">
                                            <asp:Label ID="lblName" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" name="fullname" CssClass="l_form_txt" runat="server" EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr  id="trUsername" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblUsername" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUsername"  name="username" CssClass="l_form_txt" runat="server" EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr  id="trPassword1" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblPassword" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" CssClass="l_form_txt" name="password"  runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr  id="trPassword2" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblPassword2" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword2" CssClass="l_form_txt" name="password_confirm" runat="server" EnableViewState="true"  TextMode="Password"></asp:TextBox>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr id="pnlBirthdate" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblBirthdate" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <uc3:BirthdayPicker ID="BirthdayPicker1" runat="server" CssClass="l_form_txt" />
                                            <uc2:DatePicker ID="datePicker1" CssClass="datepicker" runat="server" Visible="false">
                                            </uc2:DatePicker>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="reglabel">
                                            <asp:Label ID="lblEmail" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" CssClass="l_form_txt" runat="server" name="email"></asp:TextBox>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr id="pnlGender" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblGender" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <div class="gender">
                                                <asp:DropDownList ID="dropGender" CssClass="l_form_ddl" runat="server" EnableViewState="true">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trInterestedIn" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblInterestedIn" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dropInterestedIn" CssClass="l_form_ddl mmExplainDialog" runat="server"
                                                EnableViewState="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trCountry" runat="server">
                                        <td class="reglabel">
                                            <%= Lang.Trans("Country") %><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dropCountry" CssClass="l_form_ddl" runat="server" EnableViewState="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trState" runat="server">
                                        <td class="reglabel">
                                            <%= Lang.Trans("Region/State") %><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dropRegion" CssClass="l_form_ddl" runat="server" EnableViewState="true">
                                                <asp:ListItem Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trCity" runat="server" enableviewstate="true">
                                        <td class="reglabel">
                                            <%= Lang.Trans("City") %><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dropCity" CssClass="l_form_ddl" runat="server" EnableViewState="true">
                                                <asp:ListItem Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trInvitationCode" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblInvitationCode" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvitationCode" CssClass="l_form_txt" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr id="trCaptcha" visible="false" class="captha" runat="server">
                                        <td class="reglabel">
                                            <asp:Label ID="lblAntiSpam" runat="server"></asp:Label><span style="color: mediumVioletRed;">&nbsp;*</span>
                                        </td>
                                        <td>
                                            <img src="Captcha.ashx" alt="Captcha" /><br />
                                            <div class="separator">
                                            </div>
                                            <asp:Label ID="lblCaptcha" runat="server"></asp:Label><br />
                                            <asp:TextBox ID="txtCaptcha" CssClass="l_form_txt" runat="server" EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr class="agreementlink">
                                        <td colspan="2">
                                            <span>
                                                <asp:CheckBox ID="cbAgreement" runat="server" CssClass="checkbox" Style="vertical-align: bottom;"
                                                    EnableViewState="true" name="terms">
                                                </asp:CheckBox>
                                                
                                            </span>
                                        </td>
                                        <td class="status"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <div class="buttons">
                                    <uc1:FlexButton ID="btnRegister" name="regButton" runat="server" CssClass="LargeButton" OnClick="btnRegister_Click"
                                        OnClientClick="___openContinuePopup();" RenderAs="LinkButton" />
                                </div>
                            </asp:View>
                            <asp:View ID="view2" runat="server">
                                <div class="PostRegistrationPopupBg" onload="$('.PostRegistrationPopupBg').dialog({modal:true});">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 40px; width: 100%;">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px; width: 100%;">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr style="height: 55px; width: 100%;">
                                            <td>
                                                <div class="fl" style="width: 220px; height: 50px; margin-left: 40px; cursor: pointer;"
                                                    onclick="window.location='ManageProfile.aspx'">
                                                </div>
                                                <div class="fl" style="width: 200px; height: 50px; margin-left: 20px; cursor: pointer;"
                                                    onclick="window.location='Home.aspx'">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px; width: 100%;">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
                <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
    </div>
    <div class="clear">
    </div>


</asp:Content>
