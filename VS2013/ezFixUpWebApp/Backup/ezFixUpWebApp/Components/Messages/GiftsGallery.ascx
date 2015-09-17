﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GiftsGallery.ascx.cs"
    Inherits="ezFixUp.Components.Messages.GiftsGallery" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="~/Components/SideMenuStart.ascx" %>
<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>

<style type="text/css">
        td.giftCategory{color:White !important;background:skyBlue;}
        td.giftCategory:hover{background:Red;}
        td.giftCategory.active{background:DodgerBlue !important;}
</style>
<script type="text/javascript">
    var selectedGiftCount = 0;
    var selectedGiftsIds = new Array();
    var pnlSelectedGiftsClientID = "<%= pnlSelectedGifts.ClientID %>";
    var pnlGiftsCategoriesClientID = "<%= pnlGiftsCategoriesMenu.ClientID %>";

    function pageLoad() {
        var jqHidePnlId = "#pnlGiftsGallerySelectId";
        jQuery(function($) { 
            $(jqHidePnlId).hide(); 
            var selector = '#' + pnlGiftsCategoriesClientID + ' td';
//            //jQuery(selector).on('mouseover', '.giftCategory', function (e) {
//            $('td.giftCategory').live('mouseover',function(){
//                $(this).css({background:'Red'});
//            });
//            $('td.giftCategory').live('mouseout',function(){
//            //jQuery(selector).on('mouseout', '.giftCategory', function (e) {
//                $(this).css({background:'Teal'});
//            });
            $('td.giftCategory').live('click', function(){
            //jQuery(selector).on('mouseout', '.giftCategory', function (e) {
                $('td.giftCategory').removeClass('active');
                $(this).addClass('active');
            });
        });
    }
    function hideGiftOptions(giftTblId) {
        var jqTblSelector = "#" + giftTblId + " tbody tr td";
        var jqAddSelector = "#" + giftTblId + " tr td.addGift";
        var jqViewSelector = "#" + giftTblId + " tr td.viewGift";
        jQuery(function($) {
            //$(jqTblSelector).css('border', 'solid 2px transparent');
            $(jqAddSelector).removeClass('addGiftBtn');
            $(jqViewSelector).removeClass('viewGiftBtn');
            $(jqTblSelector).css('color', 'transparent');
        });
    }
    function showGiftOptions(giftTblId) {
        var jqTblSelector = "#" + giftTblId + " tbody tr td";
        var jqAddSelector = "#" + giftTblId + " tr td.addGift";
        var jqViewSelector = "#" + giftTblId + " tr td.viewGift";
        jQuery(function($) {
            //$(jqTblSelector).css('border', 'outset 2px Teal');
            $(jqTblSelector).css('color', 'white');
            $(jqAddSelector).addClass('addGiftBtn');
            $(jqAddSelector).hover(
                 function(){$(this).addClass('addGiftBtnH');}
                ,function(){$(this).removeClass('addGiftBtnH');});
            $(jqViewSelector).addClass('viewGiftBtn');
            $(jqViewSelector).hover(
                 function(){$(this).addClass('viewGiftBtnH');}
                ,function(){$(this).removeClass('viewGiftBtnH');});
        });
    }
    function addGift(id) {
        var cont = true;
        for (i = 0; i < selectedGiftsIds.length; i++) {
            if (id == selectedGiftsIds[i]) cont = false;
        }
        if (cont) {
            selectedGiftCount++;
            var pnlSelector = "#" + "<%= pnlSelectedGifts.ClientID %>";
            var uniqueId = "selgiftid" + selectedGiftCount;
            var jqUniqueId = "#" + uniqueId;
            var xDiv = "<div onclick=\"removeGift(" + id + ", '" + jqUniqueId + "');\" " +
            "style=\"valign:top;float:right;background:Navy;color:white;display:inline;padding:0 0 2px 2px;text-align:right;cursor:pointer;\"><b>X</b></div>";
            var giftImgDiv = "<div id=\"" + uniqueId + "\" style=\"display:inline; width:90px;height:90px;float:left;margin:2px;border:solid 1px Navy;" +
                         "background: url(GiftImage.ashx?id=" + id + "&width=90&height=90) no-repeat;\">" + xDiv +
                         "</div>"
            jQuery(function($) { $(pnlSelector).append(giftImgDiv); });
            selectedGiftsIds.push(id);
            updateGifts();
        }
    }
    function removeGift(gid, jqUniqueId) {
        jQuery(jqUniqueId).remove();
        for (i = 0; i < selectedGiftsIds.length; i++) {
            if (selectedGiftsIds[i] == gid) {
                var idx = i + 1;
                if (idx == selectedGiftsIds.length) {
                    selectedGiftsIds.pop();
                }
                else {
                    var arr1 = selectedGiftsIds.slice(idx);
                    var arr2 = selectedGiftsIds.slice(0, i);
                    selectedGiftsIds = arr1.concat(arr2);
                }
                break;
            }
        }
        updateGifts();
    }
    function viewGift(id, name, phrase) {
        var bgStyle = "url(GiftImage.ashx?id="+id+"&width=270&height=270) no-repeat";
        $("#giftPreviewTable div.giftPreviewBg").css({background: bgStyle});
        $("#giftPreviewTitle").html(name);
        //$("#popGiftPhrase").html(phrase);
        $("#giftPreviewPopup").show();
        $("#popSelectGift").click(function(){
            $("#giftPreviewPopup").hide();
            addGift(id);
        });        
    }
    function hideGalleryView() {
        jQuery(function($) { $("#galleryView").hide(); });
        return false;
    }
    function cancelGifts() {
        selectedGiftCount = 0;
        selectedGiftsIds = new Array();
    }
    function processCallbackResult(returnmessage, context){
        debugger;
        var lbl = $get('<%= lblSelectedGiftsIds.ClientID %>');
        lbl.Text = returnmessage;
        lbl.textContent = returnmessage;
    }

    function processCallbackError(returnmessage, context){
        alert("Callback Error: " + returnmessage + ", " + context);
    }
    function updateGifts() {
        var message = selectedGiftsIds.toString();
        var context = 'lblSelectedGiftsIds';
        <%=CallBackFunctionInvocationStr%>
        
    }
    function showGalleryView() {
        jQuery(function($) { $("#galleryView").show(); });
        return false;
    }
</script>

<div id="giftPreviewPopup" style="display: none; background: url(./images/gift_pop_bg.png)  no-repeat;
    width: 240px; padding: 2px; margin: 0 auto; text-align: center;">
    <table id="giftPreviewTable" style="display: block; width: 240px; height: 286px;
        background: transparent;">
        <thead>
            <tr>
                <td style="height: 17px; text-indent: 5px; color: White; font-weight: bold;">
                    <div style="float: left; width: 90%; text-align: center;" id="giftPreviewTitle">
                    </div>
                    <div id="giftPreviewCloseBtn" style="width: 18px; height: 18px; cursor: pointer;
                        float: right" onclick="$('#giftPreviewPopup').hide();">
                    </div>
                </td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="width: 238px; height: 240px;">
                    <div class="giftPreviewBg">
                        <div style="height: 180px;">
                        </div>
                        <div id="popSelectGift" style="background: transparent; text-align: center;
                            color: Navy; display: block; height: 35px; cursor: pointer;">
                            <div class="selectPreviewGiftBtn" style="display:inline-block;padding:7px 0;"><%= "Select".Translate() %></div>
                        </div>
                        <%--<div id="popGiftPhrase" class="giftPhrase" style="background: rgb(128,222,232); text-align: center;
                            color: White; display: block; height: 20px; line-height: 18px; font-size: smaller;">
                            <%= "Phrase".Translate() %>
                        </div>--%>
                        <div style="height: 20px;">
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <div id="pnlGiftsGallerySelectId" runat="server" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnComplete" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblSelectedGiftsIds" runat="server" EnableViewState="false" Style="display: none;" />
                    <div id="galleryView" runat="server" style="display: none;">
                        <%--<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" Title="Gifts Gallery"></uc1:LargeBoxStart>--%>
                        <table width="100%">
                            <tr>
                                <td width="20%" style="text-align: center; font-weight: bold; vertical-align: top;">
                                    <div id="pnlGiftsCategoriesMenu" runat="server" style="display: block;">
                                        <asp:PlaceHolder ID="plhGiftsCategories" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                    </div>
                                    <div class="clear" />
                                </td>
                                <td width="77%">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnkFirst" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkPrev" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkLast" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:DataList ID="dlGiftsGallery" runat="server" OnItemCreated="DataListGiftsCategoriesItemCreated"
                                                RepeatColumns="5" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                <ItemTemplate>
                                                    <div style="width: 80px; height: 104px; border: solid 1px Navy; background: white;
                                                        text-align: center;" >
                                                        <div style="height: 12px; color: Navy; text-align: center; font-size: 0.9em;">
                                                            <%# ((string)Eval("Name")) %></div>
                                                        <div style="height: 12px; color: Black; text-align: center; font-size: 0.9em;">
                                                            <%# Eval("Price").ToString() %>
                                                            points</div>
                                                        <table id="giftImage_<%# (int)Eval("GiftId") %>" style="display: block; width: 80px;
                                                            height: 80px; background: url('<%# (string)Eval("GiftUrl") %>') no-repeat;"
                                                            onmouseover="showGiftOptions(this.id);" onmouseout="hideGiftOptions(this.id);"
                                                            ondblclick="showGiftOptions(this.id);">
                                                            <thead>
                                                                <tr>
                                                                    <td style="height: 25px;">
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                            <tbody >
                                                                <tr style="width: 80px;display:block;">
                                                                    <td class="addGift" onclick="addGift(<%# (int)Eval("GiftId") %>);" style="width: 80px;display:block;">
                                                                        <%# "Select".Translate() %>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 80px;display:block;">
                                                                    <td class="viewGift" onclick="viewGift(<%# (int)Eval("GiftId") %>, '<%# ((string)Eval("Name")) %>', '<%# (string)Eval("Phrase") %>');" style="width: 80px;display:block;">
                                                                        <%# "View".Translate() %>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Panel ID="pnlPaginator" Visible="True" runat="server" Style="display: block;
                                        clear: both; text-align: center">
                                        <table width="100%">
                                            <tr>
                                                <td style="width:35%;">
                                                    <asp:LinkButton ID="lnkFirst" runat="server" CssClass="firstPageBtn tipsys"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CssClass="prevPageBtn tipsys"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPager" runat="server" ForeColor="Navy" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td style="width:35%;">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CssClass="nextPageBtn tipsys"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkLast" runat="server" CssClass="lastPageBtn tipsys"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <%--<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>--%>
                    </div>
    <div id="pnlPreloadImages">                  
    <%
        string imgTagsStr = "";
     UserSession userSession = PageBase.GetCurrentUserSession();
     if (userSession != null)
    {
        if (userSession.UserFriendsPrelodImages != null && Global.AppPreloadGiftsImages != null)
        {
            imgTagsStr = Global.AppPreloadGiftsImages
                .Aggregate("", (current, imgTag) => 
                    current + String.Format("<img src='{0}' style='display:none;' alt='preload_img' />", imgTag));
            litPreloadImages.Text = "<script type='text/javascript'> window.addEventListener(window, 'load', function() { $('#pnlPreloadImages').html(\"" + imgTagsStr + "\");});</script>"; 
        }
    }%>
    </div>
    <asp:Literal ID="litPreloadImages" runat="server"></asp:Literal>  
    <%--<script type="text/javascript">
        jQuery(function($) {
            var appendStr = "<%=imgTagsStr %>";
            //debugger;
            $("document.body > #pnlPrelodImages").html(appendStr);
        });
    </script>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<uc1:SmallBoxStart ID="SmallBoxStart2" runat="server" Title="Selected gifts"></uc1:SmallBoxStart>--%>
            <div id="pnlSelectedGifts" runat="server" style="height: 100px; display: block;">
            </div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div class="buttons" style="background: transparent; /*rgb(210,229,235); */ clear: both;">
                        <asp:Button ID="btnComplete" runat="server" OnClick="btnComplete_Click" CssClass="medium zurbtn blue"
                            OnClientClick="completeGifts();" Visible="false"/>
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="medium zurbtn red"
                            OnClientClick="cancelGifts();" Visible="false" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server"></uc1:SmallBoxEnd>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
