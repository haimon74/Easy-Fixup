<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="SendMessage.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.SendMessage" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GiftsGallery" Src="Components/Messages/GiftsGallery.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Extenders" TagPrefix="cc2" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <%--    <Scripts>
        <asp:ScriptReference Path="~/scripts/components/GiftsGallery.js" />
    </Scripts>--%>
    </asp:ScriptManagerProxy>

    <script type="text/javascript">
        var jqGalleryId = "#" + "<%= GiftsGallery1.ClientID %>";
        jQuery(function($) {
            //debugger;
            //$(jqGalleryId).css('display', 'none !important');
            //hideGiftGallery();
        });
        function onLoad() {
            //hideGiftGallery();
        }
        function showGiftGallery() {
            jQuery(function($) { $(jqGalleryId).show(); });
        }
        function hideGiftGallery() {
            jQuery(function($) { $(jqGalleryId).hide(); });
        }
    </script>

    <div id="sidebar">
        <uc1:SideMenuStart ID="SmallBoxStart1" runat="server">
        </uc1:SideMenuStart>
        <div id="fmt">
            <ul class="fmt">
                <li>
                    <asp:LinkButton ID="lnkBack" runat="server" />
                </li>
            </ul>
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <div style="height: 300px; position: static; background: transparent;">
            <components:BannerView ID="bvSendMessageLeftBottom" runat="server" Key="SendMessageLeftBottom">
            </components:BannerView>
        </div>
    </div>
    <div id="main">
        <div>
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <div id="container">
                <table style="margin: 15px 15px 5px 15px; ">
                <tr>
                    <td style="width:47%;" >
                    <table id="tblFrom"  class="fl">
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="ltrPhotoTo" runat="server"></asp:Literal>
                            </td>
                            <td valign="top" style="width:60%;">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="label c026683">
                                                <%= Lang.Trans("To(sendee)") %>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToUsername" runat="server" ForeColor="#5DA1B3"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="label c026683">
                                                <%= Lang.Trans("Age") %>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToAge" runat="server" ForeColor="#5DA1B3"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="valign_top">
                                            <span class="label c026683">
                                                <%= Lang.Trans("Location") %>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToLocation" runat="server" ForeColor="#5DA1B3"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" colspan="2" >
                                            <table>
                                            <tr>
                                                <td style="width:25px;">
                                                    <uc1:FlexButton ID="fbAddGift" CssClass="GiftBtnSmall fl" RenderAs="LinkButton" runat="server" OnClick="lbAddGift_Click" class="fl"/>
                                                </td>
                                                <td valign="bottom">
                                                    <asp:LinkButton ID="lbAddGift" runat="server" Font-Bold="true" ForeColor="#026683"
                                                        Style="cursor: pointer;" OnClick="lbAddGift_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            </table>
                                            
                                            <%--<a class="c026683"><b>
                                            <%= Lang.Trans("Add gift")%></b></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table><br /><br />
                    <asp:CheckBox ID="cbSendee1" runat="server" Visible="false" Checked="true" />
                    </td>
                    <td style="width:47%;" >
                    <table id="tblAbout" runat="server" class="fr" visible="true" style="visibility:hidden;" >
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="ltrAboutPhoto" runat="server" Visible="true"></asp:Literal>
                            </td>
                            <td valign="top" style="width:60%;">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="label c026683">
                                                <%= this.MessageType == Message.eMessageType.Matchmaking ?
                                                    Lang.Trans("To(sendee)") : Lang.Trans("About")%>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAboutUsername" runat="server" ForeColor="#5DA1B3"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="label c026683">
                                                <%= Lang.Trans("Age") %>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAboutAge" runat="server" ForeColor="#5DA1B3"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="valign_top">
                                            <span class="label c026683">
                                                <%= Lang.Trans("Location") %>:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAboutLocation" runat="server" ForeColor="#5DA1B3"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table><br /><br />
                    <asp:CheckBox ID="cbSendee2" runat="server" Visible="false" Checked="true" />
                    </td>
                    </tr>
                </table>
                <br />
                <div class="separator">
                </div>
                <asp:UpdatePanel ID="UpdatePanelGiftsGallery" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lbAddGift" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="fbAddGift" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
                <uc1:GiftsGallery ID="GiftsGallery1" runat="server" PaginatorEnabled="true"></uc1:GiftsGallery>
                
                <div class="wrap-sized" style="margin: 5px 25px 5px 25px;">
                    <div class="separator">
                    </div>
                    <%--Using wink Language selection--%>
                    <%--<ajaxToolkit:CascadingDropDown ID="CascadingWinkLanguage" runat="server" TargetControlID="ddlLanguage"
                        Category="WinkLanguage" ServicePath="~/Services/Service.asmx" PromptText=" "
                        ServiceMethod="GetLanguages" />
                    <ajaxToolkit:CascadingDropDown ID="CascadingWinkCategory" runat="server" TargetControlID="ddlWinkCategory"
                        Category="WinkCategory" ServicePath="~/Services/Service.asmx" PromptText=" "
                        ServiceMethod="GetWinkCategoriesByLanguageId" ParentControlID="ddlLanguage" />--%>
                    <%--Using wink category selection--%>
                    <%--<ajaxToolkit:CascadingDropDown ID="CascadingWinkCategory" runat="server" TargetControlID="ddlWinkCategory"
                        Category="WinkCategory" ServicePath="~/Services/Service.asmx" PromptText=" "
                        ServiceMethod="GetHebrewWinkCategories" />
                    <ajaxToolkit:CascadingDropDown ID="CascadingWinkPhrase" runat="server" TargetControlID="ddlWinkPhrase"
                        Category="WinkPhrase" ServicePath="~/Services/Service.asmx" LoadingText="[Loading winks...]"
                        PromptText=" " ServiceMethod="GetWinksByCategoryId" ParentControlID="ddlWinkCategory" />--%>
                    <components:BannerView ID="bvSendMessageRight" runat="server" Key="SendMessageRight">
                    </components:BannerView>
                    <div id="pnlWinkSelector" runat="server" class="fl w100">
                        <table style="width: 90%;">
                            <%--<tr>
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("Wink Language") %>:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="l_form_ddl">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("Wink Category") %>:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlWinkCategory" runat="server" CssClass="l_form_ddl">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("Wink Phrase") %>:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlWinkPhrase" runat="server" Width="450">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="pnlMutualFriendMessage" runat="server" class="fl w100">
                        <table class="w100">
                            <tr>
                                <td>
                                    <span class="label c026683">
                                        <asp:Label ID="lblMutualFriendMsgTitle" runat="server"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr id="mfQuestions" runat="server">
                                <td>
                                    <asp:CheckBox ID="cbMfQuestion1" runat="server" Visible="true" /><br />
                                    <asp:CheckBox ID="cbMfQuestion2" runat="server" Visible="true" /><br />
                                    <asp:CheckBox ID="cbMfQuestion3" runat="server" Visible="true" /><br />
                                    <asp:CheckBox ID="cbMfQuestion4" runat="server" Visible="true" /><br />
                                    <asp:CheckBox ID="cbMfQuestion5" runat="server" Visible="true"/><br />
                                </td>
                            </tr>
                            <tr id="mfAnswers" runat="server">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMfAnswer1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblMfAnswer1" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMfAnswer2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblMfAnswer2" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMfAnswer3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblMfAnswer3" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMfAnswer4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMfAnswer4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMfAnswer5" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMfAnswer5" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("More Information") %>:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtMutualFriendMsgContent" CssClass="multilinetextbox" runat="server" style="width:90%;"
                                        TextMode="MultiLine"></asp:TextBox><br />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="pnlMessageEdit" runat="server" class="w95 fl">
                        <table width="100%" >
                            <tr id="trLabelSubject"  runat="server" visible="false">
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("Subject") %>:</span>
                                </td>
                            </tr>
                            <tr id="trSubjectText"  runat="server" visible="false">
                                <td>
                                    <asp:TextBox ID="txtSubject" runat="server" Width="95%" CssClass="l_form_txt"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trSmilies" runat="server" visible="false">
                                <td>
                                    <div id="pnlSmilies" runat="server" visible="false">

                                        <script language="JavaScript" type="text/javascript">
							    <!--
							    function insertSmiley(text) {
								    var area = document.forms[0].<%= MessageBodyClientId %>;
								    area.focus();
								    if (document.selection)
									    document.selection.createRange().text = text;
								    else
									    area.value += text;
								    return false;
							    }
							    //-->
                                        </script>

                                        <table width="95%">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="ibtnPrevSmilies" ImageUrl="~/images/iconprev.gif" OnClick="ibtnPrevSmilies_Click"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updatePanelSmilies" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DataList ID="dlSmilies" SkinID="Smilies" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                                runat="server">
                                                                <ItemTemplate>
                                                                    <a href="#" onclick="return insertSmiley('<%# Eval("Key") %>')">
                                                                        <img alt='smiley image' src="<%# Eval("Image") %>" title="<%# Eval("Description") %>"
                                                                            border="0" /></a>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ibtnPrevSmilies" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ibtnNextSmilies" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ibtnNextSmilies" ImageUrl="~/images/iconnext.gif" OnClick="ibtnNextSmilies_Click"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="label c026683">
                                        <%= Lang.Trans("Content") %>:</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtMessageBody" CssClass="multilinetextbox" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox><br />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="buttons">
                    <div class="separator6"></div>
                    <asp:Button ID="btnSend" runat="server" CssClass="medium zurbtn blue"></asp:Button>
                </div>
                <div id="pnlPreviousMessages" class="text" visible="false" runat="server">
                    <asp:Repeater ID="rptPreviousMessages" runat="server">
                        <ItemTemplate>
                            <div class="separator">
                            </div>
                            <div class="line">
                            </div>
                            <div class="clear">
                            </div>
                            <div class="separator">
                            </div>
                            <b>
                                <%# Eval("Username") %>:</b>
                            <%# Eval("Message") %>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <br />
                            <div class="separator">
                            </div>
                        </SeparatorTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        </div>
        <div style="height: 100px; position: static; background: transparent;">
            <components:BannerView ID="bvSendMessageRightBottom" runat="server" Key="SendMessageRightBottom">
            </components:BannerView>
        </div>
    </div>
</asp:Content>
