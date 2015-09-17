<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopModerators.ascx.cs" Inherits="AspNetDating.Components.TopModerators" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Search/SearchResults.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<%--<%@OutputCache Duration="3600000" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<table id="topuserstable" cellpadding="0" cellspacing="0">
    <tr>
        <td width="50%" valign="top" id="tdTopFemales" runat="server">
            <h4 class="BoxSubHeading"><%= String.Format(
                Lang.Trans("Top {0} Females Moderators"), Config.CommunityModeratedSystem.TopModeratorsCount) %></h4>
            <uc1:SearchResults id="SearchResults1" PaginatorEnabled="false" ShowSlogan="false"
                ShowCity="false" ShowModerationScore="true" ShowLastOnline="false" UseCache="true" runat="server">
            </uc1:SearchResults>
        </td>
        <td width="50%" valign="top" id="tdTopMales" class="verticalSeparator" runat="server">
            <h4 class="BoxSubHeading"><%= String.Format(
                Lang.Trans("Top {0} Males Moderators"), Config.CommunityModeratedSystem.TopModeratorsCount) %></h4>
            <uc1:SearchResults id="SearchResults2" PaginatorEnabled="false" ShowSlogan="false"
                ShowCity="false" ShowModerationScore="true" ShowLastOnline="false" UseCache="true" runat="server">
            </uc1:SearchResults>
        </td>
    </tr>
</table>    