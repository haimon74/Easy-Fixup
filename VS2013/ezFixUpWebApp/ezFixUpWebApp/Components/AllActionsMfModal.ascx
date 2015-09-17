<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="AllActionsMfModal.ascx.cs" 
            Inherits="ezFixUp.Components.AllActionsMfModal" %>
<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>


<script type="text/javascript">
    function pageLoad() {
        var modal = $find("programmaticModalPopupBehavior");
        //jQuery.noConflict();
        jQuery(function($) {
            $('div.ThumbsWrap, div.ThumbsWrapGrid').mouseover(function() {
                $('.ThumbIconsWrap').hide();
                $('div.ThumbsWrap, div.ThumbsWrapGrid').css('border', 'solid 1px WhiteSmoke');
                $(this).css('border', 'solid 1px Teal/*#12657F*/');
                $(this).find('.ThumbIconsWrap').show();
                $(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
            });
            $('div.MoreIconWrap').mouseover(function() {
                $(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
            });
            $('.ThumbIconsWrap, .MoreIconWrap').bind('click', function() {
                //var viewer = "<%= PageBase.GetCurrentUserSession() != null) ? PageBase.GetCurrentUserSession().Username : null%>";
                if (viewer == null || viewer == "")
                    window.location = "Registration.aspx";
                else {
                    var viewed = $(this).attr("viewed");
                    var serviceUrl = "./handlers/GetJsonMutualFriends.ashx?viewer=" + viewer + "&viewed=" + viewed;
                    //$(document).getAJAJ(serviceUrl, "GET", PopulateMutualFriendsModal, "", "", false);
                    var url = "./handlers/GetJsonMutualFriends.ashx";
                    jQuery.ajax({
                        //type: "POST",
                        type: "GET",
                        //url: "./services/SearchService.asmx/GetMutualFriends",
                        url: serviceUrl,
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        cache: false,
                        dataType: "json",
                        beforeSend: function(x) {
                            if (x && x.overrideMimeType) {
                                x.overrideMimeType("application/json;charset=UTF-8");
                            }
                        },
                        error: function(e, xhr) { alert("Error: " + xhr); },
                        //complete: function(jqXHR, textStatus) {
                        //alert("Complete: " + jqXHR.responseText + " ; " + textStatus);
                        //    PopulateMutualFriendsModal(jqXHR.responseText, viewed);
                        //},
                        //                        dataFilter: function(data) {
                        //                            var response;
                        //                            //if (typeof (JSON) !== "undefined" && typeof (JSON.parse) === "function") {
                        //                            try {
                        //                                debugger;
                        //                                //response = JSON.parse(data); 
                        //                                //alert("parsed: " + data);
                        //                                response = data.parseJSON(); //alert("parsed: " + data);
                        //                            }
                        //                            //else response = eval("(" + data + ")");
                        //                            catch (e) {
                        //                                response = eval("(" + data + ")"); //alert("eval: " + data); 
                        //                            }
                        //                            if (response.hasOwnProperty("d"))
                        //                                return response.d;
                        //                            else return response;
                        //                            //                            var msg; //var res = JSON.parseWithDate(jsonString);
                        //                            //                            //debugger;
                        //                            //                            if (typeof (JSON) !== 'undefined' && typeof (JSON.parse) === 'function')
                        //                            //                                msg = JSON.parse(data);
                        //                            //                            else msg = eval('(' + data + ')');
                        //                            //                            debugger;
                        //                            //                            if (msg.hasOwnProperty('d')) return msg.d;
                        //                            //                            else return msg;
                        //                        },
                        success: function(str, status, jqxhr) {
                            var jsonString = jqxhr.responseText;
                            var jsonObj; //= JSON.parseWithDate(jsonString);
                            //debugger;
                            if (typeof (JSON) !== 'undefined' && typeof (JSON.parse) === 'function')
                                try { jsonObj = jsonString.parseJSON(); }
                            catch (e) { jsonObj = eval('(' + jsonString + ')'); }
                            else jsonObj = eval('(' + jsonString + ')');
                            //debugger;
                            if (jsonObj.hasOwnProperty('d')) jsonObj = jsonObj.d;
                            PopulateMutualFriendsModal(jsonObj, viewed);
                        }
                    });
                    //alert(viewed);
                    ShowModalPopup(viewed);
                }
                return false;
            });
        });
    }
    function PopulateMutualFriendsModal(jsonData, viewed) {
        //jQuery(function($) {
        //debugger;
        //alert(jsonData);
            var lbAskIds = ["#" + "<%=lbModalMF1_AskAbout.ClientID%>", "#" + "<%=lbModalMF2_AskAbout.ClientID%>", "#" + "<%=lbModalMF3_AskAbout.ClientID%>"];
            var lbReqIds = ["#" + "<%=lbModalMF1_MatchReq.ClientID%>", "#" + "<%=lbModalMF2_MatchReq.ClientID%>", "#" + "<%=lbModalMF3_MatchReq.ClientID%>"];
            if (jsonData.length == 0) $("#pnlPopMF").hide();
            else $("#pnlPopMF").show()
            for (var i = 0; i < jsonData.length; i++) {
                //if (jsonData.length > 0) {
                var divId = "#mf" + (i + 1) + "div";
                if (jsonData[i].PhotoId != -99) {
                    $(divId).attr('title', jsonData[i].Username);
                    $(divId).attr('style', 'background:url(Image.ashx?id='
                        + jsonData[i].PhotoId + '&width=60&height=60) no-repeat');
                    $(lbAskIds[i]).attr('href', "Ask_" + jsonData[i].Username + "_about_" + viewed + ".aspx");
                    //$(lbPassIds[i]).attr('href', "SendMessage.aspx?src=profile&type=mfMsg&to_user=" + jsonData[i].Username + "&uid=" + viewed);
                    $(lbReqIds[i]).attr('href', "Request_" + jsonData[i].Username + "_Match_With_" + viewed+".aspx");
                }
                else {
                    $(divId).attr('style', 'visibility:hidden');
                }
            }
//            if (jsonData[0].PhotoId > 0) {
//                $("#mf1div").attr('title', jsonData[0].Username);
//                $("#mf1div").attr('style', 'background:url(Image.ashx?id=' + jsonData[0].PhotoId + '&width=60&height=80) no-repeat');
//            }
//            if (jsonData[1].PhotoId > 0) {
//                $("#mf2div").attr('title', jsonData[1].Username);
//                $("#mf2div").attr('style', 'background:url(Image.ashx?id=' + jsonData[1].PhotoId + '&width=60&height=80) no-repeat');
//            }
//            if (jsonData[2].PhotoId > 0) {
//                $("#mf3div").attr('title', jsonData[2].Username);
//                $("#mf3div").attr('style', 'background:url(Image.ashx?id=' + jsonData[2].PhotoId + '&width=60&height=80) no-repeat');
//            }
            $('div.ThumbsWrapModal').mouseover(function() {
                $('.ThumbIconsWrap').hide();
                $(this).find('.ThumbIconsWrap').show();
                //$(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
            });
        //});
        
    }
    function closeModal() {
        var lbAskIds = ["#" + "<%=lbModalMF1_AskAbout.ClientID%>", "#" + "<%=lbModalMF2_AskAbout.ClientID%>", "#" + "<%=lbModalMF3_AskAbout.ClientID%>"];
        var lbReqIds = ["#" + "<%=lbModalMF1_MatchReq.ClientID%>", "#" + "<%=lbModalMF2_MatchReq.ClientID%>", "#" + "<%=lbModalMF3_MatchReq.ClientID%>"];
        for (var i = 0; i < 3; i++) {
            var divId = "#mf" + (i + 1) + "div";
            $(divId).attr('title', '');
            $(divId).attr('style', '');
            $(lbAskIds[i]).attr('href', "");
            //$(lbPassIds[i]).attr('href', "");
            $(lbReqIds[i]).attr('href', "");            
        }
        var modal = $find("programmaticModalPopupBehavior");
        modal.hide();
    }
    function ShowModalPopup(viewed) {
        var username = viewed.trim();
        //alert(viewed);
        var modal = $find("programmaticModalPopupBehavior");
        jQuery(function($) {
            $("#" + "<%=modalHeader.ClientID%>").html(username);
            $("#" + "<%=hfViewed.ClientID%>").val(username);
            $("#" + "<%=lbModalBlock.ClientID%>").hover(
              function() {
                  $(this).addClass('BlockBtnBigH'); $(this).removeClass('BlockBtnBig');
              }, function() {
                  $(this).removeClass('BlockBtnBigH'); $(this).addClass('BlockBtnBig');
              });
            $("#" + "<%=lbModalFavorite.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFavorite.aspx?cmd=add&src=profile&uid=" + username);
                  $(this).addClass('FavoriteBtnBigH'); $(this).removeClass('FavoriteBtnBig');
              }, function() {
                  $(this).removeClass('FavoriteBtnBigH'); $(this).addClass('FavoriteBtnBig');
              });
            $("#" + "<%=lbModalInviteFriend.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFriend.aspx?cmd=add&src=profile&uid=" + username);
                  $(this).addClass('AddBtnBigH'); $(this).removeClass('AddBtnBig');
              }, function() {
                  $(this).removeClass('AddBtnBigH'); $(this).addClass('AddBtnBig');
              });
            $("#" + "<%=lbModalMatchRequest.ClientID%>").hover(
                function() {
                    var actionUrl; 
                        <% if (!MatchmakerHelper.IsMatchmakerState)
                           { 
                        %>
                            actionUrl = "./MatchMaker.aspx?u=";
                        <%
                           }
                           else if (MatchmakerHelper.IsMatchToFriendAlredySelected)
                           {
                        %> 
                            actionUrl = "./Handlers/MatchWith.ashx?with_user=";
                        <%
                           }
                           else
                           {
                        %>
                            actionUrl = "./Home_mm.aspx?u=";
                        <%
                           }
                        %>
                    $(this).attr('href', actionUrl + username);
                    $(this).addClass('MatchBtnBigH'); $(this).removeClass('MatchBtnBig');
                }, function() {
                    $(this).removeClass('MatchBtnBigH'); $(this).addClass('MatchBtnBig');
                });
            $("#" + "<%=lbModalSendMsg.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&to_user=" + username);
                  $(this).addClass('MsgBtnBigH'); $(this).removeClass('MsgBtnBig');
              }, function() {
                  $(this).removeClass('MsgBtnBigH'); $(this).addClass('MsgBtnBig');
              });
            $("#" + "<%=lbModalSendGift.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&type=gift&to_user=" + username);
                  $(this).addClass('GiftBtnBigH'); $(this).removeClass('GiftBtnBig');
              }, function() {
                  $(this).removeClass('GiftBtnBigH'); $(this).addClass('GiftBtnBig');
              });
            $("#" + "<%=lbModalWink.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&type=wink&to_user=" + username);
                  $(this).addClass('WinkBtnBigH'); $(this).removeClass('WinkBtnBig');
              }, function() {
                  $(this).removeClass('WinkBtnBigH'); $(this).addClass('WinkBtnBig');
              });
            modal.show();
            for (var i = 0; i < 3; i++) {
                var divId = "#mf" + (i + 1) + "div";
                if (!$(divId).attr('title')) $(divId).hide();
            }
        });
    }
</script>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
</asp:ScriptManagerProxy>

<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />

<asp:Panel ID="Panel1" runat="server" BackColor="White" Width="225" EnableViewState="false"
            BorderStyle="Solid" BorderColor="#B6006D" BorderWidth="1px" style="display: none" >
    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="28px" Font-Bold="true" CssClass="StandardBoxTop">
        <div id="modalHeader" runat="server" style="width:80%;float:left;padding:5px;color:White;" >
        </div>
        <input type="button" id="modalCloseButton" value="x" onclick="closeModal();"
                style="height:30px;float:right;background:transparent;color:WhiteSmoke;
                        border:none;cursor:pointer;z-index:inherit;"/>
        <asp:HiddenField ID="hfViewed" runat="server" />        
    </asp:Panel>
    <div id="modalContent">
        <div class="PopModalBigIconsBg">
            <table style="display: block; width: 100%; text-align: justify; clear: both;">
                <tr>
                    <td style="width: 25px;">
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalWink" runat="server" CssClass="WinkBtnBig tipsys" />
                        <%--<asp:LinkButton ID="lbModalWink" runat="server" CssClass="WinkBtnBig" 
                                OnClientClick="closeModal();return false;" />
                        <asp:Panel ID="PanelWink" runat="server">
                            Select the text to send:
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                <asp:ListItem>Hi, I am interested... ;-)</asp:ListItem>
                                <asp:ListItem>Hi, I liked your profile... ;-)</asp:ListItem>
                                <asp:ListItem>Hi, I liked your photo... ;-)</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="DialogButtonSendWink" runat="server" OnClick="DialogButtonSendWink_Click" Text="Send Wink" />
                        </asp:Panel>
                        <cc1:Dialog ID="DialogWink" runat="server" Title="Sending Wink"
                            Modal="true" HideTitle="false" AutoOpen="false"  Width="250"
                            OpenTriggerSelector="#{lbModalWink}" CloseTriggerSelector="#{DialogButtonSendWink}" 
                            Selector="#{PanelWink}" Resizable="false"
                            AjaxOnly="true" Show="Blind" Hide="explode" ZIndex="1000"                            
                         />--%>                        
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalSendGift" runat="server" CssClass="GiftBtnBig tipsys" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalInviteFriend" runat="server" CssClass="AddBtnBig tipsys" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalSendMsg" runat="server" CssClass="MsgBtnBig tipsys" />
                    </td>                    
                    <td style="width: 25px;">
                    </td>
                </tr>
            </table>
            <table style="display: block; width: 100%; text-align: justify; clear: both;">
                <tr>
                    <td style="width: 30px;">
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalFavorite" runat="server" CssClass="FavoriteBtnBig tipsys" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalMatchRequest" runat="server" CssClass="MatchBtnBig tipsys" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lbModalBlock" runat="server" CssClass="BlockBtnBig tipsys" 
                                    OnClick="BlockBtn_Clicked"/>
                    </td>
                    <td style="width: 40px;">
                    </td>
                </tr>
            </table>            
        </div>
        <div id="pnlPopMF" style="width: 100%; height: 95px;">
            <table>
                <tr>
                    <td>
                        <div id="mf1div" class="ThumbsWrapModal">
                            <div id="mf1pop" class="ThumbIconsWrap MfPopIconsBg" style="display: none;">
                                <asp:LinkButton ID="lbModalMF1_AskAbout" runat="server" OnClientClick="openAskAboutModal();"
                                    CssClass="ModalAskAboutBtnSmall tipsys" />
                                <%--<asp:LinkButton ID="lbModalMF1_PassMsg" runat="server" CssClass="ModalMsgPassBtnSmall" 
                                    OnClientClick="openMsgPassModal();"/>--%>
                                <asp:LinkButton ID="lbModalMF1_MatchReq" runat="server" OnClientClick="openMatchRequestModal();"
                                    CssClass="ModalMatchRequestBtnSmall tipsys" 
                                    />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="mf2div" class="ThumbsWrapModal">
                            <div id="mf2pop" class="ThumbIconsWrap MfPopIconsBg" style="display: none;">
                                <asp:LinkButton ID="lbModalMF2_AskAbout" runat="server" CssClass="ModalAskAboutBtnSmall tipsys" 
                                    OnClientClick="openAskAboutModal();"/>
                                <%--<asp:LinkButton ID="lbModalMF2_PassMsg" runat="server" CssClass="ModalMsgPassBtnSmall" 
                                    OnClientClick="openMsgPassModal();"/>--%>
                                <asp:LinkButton ID="lbModalMF2_MatchReq" runat="server" OnClientClick="openMatchRequestModal();"
                                    CssClass="ModalMatchRequestBtnSmall tipsys" />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="mf3div" class="ThumbsWrapModal">
                            <div id="mf3pop" class="ThumbIconsWrap MfPopIconsBg" style="display: none;">
                                <asp:LinkButton ID="lbModalMF3_AskAbout" runat="server" OnClientClick="openAskAboutModal();"
                                    CssClass="ModalAskAboutBtnSmall tipsys" />
                                <%--<asp:LinkButton ID="lbModalMF3_PassMsg" runat="server" CssClass="ModalMsgPassBtnSmall" 
                                    OnClientClick="openMsgPassModal();"/>--%>
                                <asp:LinkButton ID="lbModalMF3_MatchReq" runat="server" OnClientClick="openMatchRequestModal();"
                                    CssClass="ModalMatchRequestBtnSmall tipsys" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Panel>




<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupDragHandleControlID="Panel2"
    CancelControlID="modalCloseButton" RepositionMode="RepositionOnWindowResizeAndScroll" 
    BackgroundCssClass="milki" Drag="true" DropShadow="false" PopupControlID="Panel1"
    BehaviorID="programmaticModalPopupBehavior" TargetControlID="hiddenTargetControlForModalPopup">
</ajaxToolkit:ModalPopupExtender>








