<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MutualFriendModalPopup.ascx.cs" Inherits="AspNetDating.Components.MutualFriendModalPopup" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>


<script type="text/javascript">
    
    function pageLoad() {
        debugger;
        var modal = $find("programmaticModalPopupBehavior");
        //jQuery.noConflict();
        jQuery(function($) {
            $('div.ThumbsWrap, div.ThumbsWrapGrid').live('mouseover', function() {
                $('.ThumbIconsWrap').hide();
                $('div.ThumbsWrap, div.ThumbsWrapGrid').css('border', 'solid 1px Lavender');
                $(this).css('border', 'solid 1px DarkCyan');//#12657f
                $(this).find('.ThumbIconsWrap').show();
                $(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
            });
            $('div.MoreIconWrap').live('mouseover', function() {
                $(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
            });
            $('.ThumbIconsWrap, .MoreIconWrap').live('click', function() {
                //var viewer = "<%= (PageBase.GetCurrentUserSession() != null) ? PageBase.GetCurrentUserSession().Username : null%>";
                var imageId = -1;
                if (viewer == null || viewer == "")
                    window.location = "Registration.aspx";
                else {
                    var viewed = $(this).attr("viewed");
                    imageId = $(this).attr("image_id");
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
                    ShowModalPopup(viewer, viewed, imageId);
                }
                return false;
            });
        });
    }
    function PopulateMutualFriendsModal(jsonData, viewed) {
        SetModalLinksVisibility(jsonData);
        var mfList = jsonData.MutualFriends;
        var lbAskIds = ["#" + "<%=lbModalMF1_AskAbout.ClientID%>", "#" + "<%=lbModalMF2_AskAbout.ClientID%>", "#" + "<%=lbModalMF3_AskAbout.ClientID%>"];
        var lbReqIds = ["#" + "<%=lbModalMF1_MatchReq.ClientID%>", "#" + "<%=lbModalMF2_MatchReq.ClientID%>", "#" + "<%=lbModalMF3_MatchReq.ClientID%>"];
        if (mfList.length == 0) $("#pnlPopMF").hide();
        else $("#pnlPopMF").show()
        for (var i = 0; i < mfList.length; i++) {
            //if (jsonData.length > 0) {
            var divId = "#mf" + (i + 1) + "div";
            if (mfList[i].PhotoId != -99) {
                $(divId).attr('title', mfList[i].Username);
                $(divId).attr('style', 'background:url(Image.ashx?id='
                    + mfList[i].PhotoId + '&width=60&height=60) no-repeat');
                $(lbAskIds[i]).attr('href', "Ask_" + mfList[i].Username + "_about_" + viewed + ".aspx");
                //$(lbPassIds[i]).attr('href', "SendMessage.aspx?src=profile&type=mfMsg&to_user=" + jsonData[i].Username + "&uid=" + viewed);
                $(lbReqIds[i]).attr('href', "Request_" + mfList[i].Username + "_Match_With_" + viewed+".aspx");
            }
            else {
                $(divId).attr('style', 'visibility:hidden');
            }
        }
        $('div.ThumbsWrapModal').mouseenter(function() {
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
            $(lbReqIds[i]).attr('href', "");            
        }
        var modal = $find("programmaticModalPopupBehavior");
        modal.hide();
    }
    function SetModalLinksVisibility(jsonData){
        var viewer = jsonData.CurrentUsername;
        var viewed = jsonData.QryUsername;
        var isFriend = jsonData.IsFriend;
        var hasRequest = jsonData.InFriendshipRequest;
        var isFavorite = jsonData.IsFavorite;
        var isBlocked = jsonData.IsBlocked;
        //debugger;
        $("#" + "<%=lbModalInviteFriend.ClientID%>").css('display', isFriend ? "none" : "");
        $("#" + "<%=lbModalRemoveFriend.ClientID%>").css('display', isFriend ? "" : "none");                
        $("#" + "<%=lbModalAcceptFriend.ClientID%>").css('display', hasRequest ? "" : "none");
        $("#" + "<%=lbModalDenyFriend.ClientID%>").css('display', hasRequest ? "" : "none");                
        $("#" + "<%=lbModalFavorite.ClientID%>").css('display', isFavorite ? "none" : "");
        $("#" + "<%=lbModalRemoveFavorite.ClientID%>").css('display', isFavorite ? "" : "none");                
        $("#" + "<%=lbModalBlock.ClientID%>").css('display',  isBlocked ? "none" : "");        
    }
    
    function ShowModalPopup(viewer, viewed, imageId) {
        var username = viewed.trim();
        var modal = $find("programmaticModalPopupBehavior");
        jQuery(function($) {
            $("#" + "<%=modalHeader.ClientID%>").html(username);
            $("#" + "<%=hfViewed.ClientID%>").val(username);
                        
            $("#" + "<%=lbModalBlock.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFavorite.aspx?cmd=block&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalFavorite.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFavorite.aspx?cmd=add&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalRemoveFavorite.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFavorite.aspx?cmd=remove&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalInviteFriend.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFriend.aspx?cmd=add&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalRemoveFriend.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFriend.aspx?cmd=remove&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalAcceptFriend.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFriend.aspx?cmd=add&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalDenyFriend.ClientID%>").hover(
              function() {
                  $(this).attr('href', "AddRemoveFriend.aspx?cmd=remove&src=profile&uid=" + username);
              }, function() {});
            $("#" + "<%=lbModalSendMsg.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&to_user=" + username);
              }, function() {}
            );
            $("#" + "<%=lbModalSendGift.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&type=gift&to_user=" + username);
              }, function() {
              });
            $("#" + "<%=lbModalWink.ClientID%>").hover(
              function() {
                  $(this).attr('href', "SendMessage.aspx?src=profile&type=wink&to_user=" + username);
              }, function() {
              });
            $("#" + "<%=lbModalMatchRequest.ClientID%>").hover(
                function() {
                    debugger;
                    $(this).attr('image_id',imageId);
                    $(this).attr('username',viewed);
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
                }, function() {});
            modal.show();
            for (var i = 0; i < 3; i++) {
                var divId = "#mf" + (i + 1) + "div";
                if (!$(divId).attr('title')) $(divId).hide();
            }
        });
    }
</script>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" BackColor="White" Width="225" EnableViewState="false"
    BorderStyle="Solid" BorderColor="#B6006D" BorderWidth="1px" Style="display: none">
    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="28px" Font-Bold="true"
        CssClass="ModalBoxTop">
        <div id="modalHeader" runat="server" style="width: 80%; float: left; padding: 5px;
            color: White;">
        </div>
        <input type="button" id="modalCloseButton" value="x" onclick="closeModal();" style="float: right;
            background: transparent; color: Black; border: none; cursor: pointer; z-index: inherit;" />
        <asp:HiddenField ID="hfViewed" runat="server" />
    </asp:Panel>
    <div id="modalContent">
        <div class="PopModalSmallIconsBg" align="center" style="margin: 0 auto;">
            <div style="margin: 0 auto; padding-top: 20px; height: 50px; display: inline-block;
                vertical-align: bottom;">
                <asp:LinkButton ID="lbModalSendMsg" runat="server" CssClass="MsgBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalWink" runat="server" CssClass="WinkBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalSendGift" runat="server" CssClass="GiftBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalInviteFriend" runat="server" CssClass="AddBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalRemoveFriend" runat="server" CssClass="RemoveFriendBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalAcceptFriend" runat="server" CssClass="AcceptBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalDenyFriend" runat="server" CssClass="DenyBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalBlock" runat="server" CssClass="BlockBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalFavorite" runat="server" CssClass="FavoriteBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalRemoveFavorite" runat="server" CssClass="FavoriteBtnSmall tipsys" />
                <asp:LinkButton ID="lbModalMatchRequest" runat="server" ToolTip='<%# Lang.Trans("Find him/her a match") %>' CssClass="MatchBtnSmall tipsys" 
                     OnClientClick="setFriendToMatch(this.id);closeModal();return false;"/>
                                    
            </div>
        </div>
        <div id="pnlPopMF" style="width: 100%; height: 95px;">
            <table>
                <tr>
                    <td>
                        <div id="mf1div" class="ThumbsWrapModal">
                            <div id="mf1pop" class="ThumbIconsWrap MfPopIconsBg" style="display: none;">
                                <asp:LinkButton ID="lbModalMF1_AskAbout" runat="server" OnClientClick="openAskAboutModal();"
                                    CssClass="ModalAskAboutBtnSmall tipsys" />
                                <asp:LinkButton ID="lbModalMF1_MatchReq" runat="server" OnClientClick="openMatchRequestModal();"
                                    CssClass="ModalMatchRequestBtnSmall tipsys" />
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="mf2div" class="ThumbsWrapModal">
                            <div id="mf2pop" class="ThumbIconsWrap MfPopIconsBg" style="display: none;">
                                <asp:LinkButton ID="lbModalMF2_AskAbout" runat="server" CssClass="ModalAskAboutBtnSmall tipsys"
                                    OnClientClick="openAskAboutModal();" />
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
