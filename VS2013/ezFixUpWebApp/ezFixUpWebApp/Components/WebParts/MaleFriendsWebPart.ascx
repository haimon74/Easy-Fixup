<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="MaleFriendsWebPart.ascx.cs" 
            Inherits="ezFixUp.Components.WebParts.MaleFriendsWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="ProfileList" Src="~/Components/Search/ProfileResultList.ascx" TagPrefix="dl" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div class="UsersBoxContent" style="text-indent: 0px !important;">
    <table class="w100">
    <tr>
        <td style="padding:0 !important;">
            <div class="scrollbarwrap">
                <dl:ProfileList ID="dlMaleFriends" runat="server" ResultType="MySingleMaleFriends" 
                    LimitRows="1" RepeatColumns="2" GridMode="false" PaginatorEnabled="false" >
                </dl:ProfileList>
        
            </div>
        </td>
        <td class="w3" style="padding:3px 0 !important; vertical-align:bottom;">
            <asp:Button ID="btnContinueMaleSingles" runat="server"  CssClass="btnContinueSymbol" 
                        Text="»"
                ToolTip="continue" OnClick="btnContinueMaleSinglesClicked" /> 
        </td>
    </tr>
    </table>    
</div>
<div class="StandardWebpartBtm">
</div>
