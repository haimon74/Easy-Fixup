<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopUsers.ascx.cs" Inherits="AspNetDating.Components.TopUsersCtrl" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Search/SearchResults.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<%--<%@OutputCache Duration="3600000" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<table id="topuserstable" cellpadding="0" cellspacing="0">
    <tr>
        <td width="33%" valign="top" id="tdTopFemales" runat="server">
            <div id="divTopFemales" class="hslice">
                <h4 class="BoxSubHeading"><span class="entry-title" style="display: none">
                    <%= String.Format(Lang.Trans("Top {0} Females on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
                <span class="ttl" style="display: none">120</span>
                <%= String.Format(Lang.Trans("Top {0} Females"), Config.Ratings.TopUsersCount) %></h4>
                <div class="entry-content">
                    <uc1:SearchResults ID="SearchResults1" PaginatorEnabled="false" ShowSlogan="false"
                        ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server">
                    </uc1:SearchResults>
                </div>
            </div>
        </td>
        <td width="33%" valign="top" id="tdTopMales" class="verticalSeparator" runat="server">
            <div id="divTopMales" class="hslice">
                <h4 class="BoxSubHeading"><span class="entry-title" style="display: none">
                    <%= String.Format(Lang.Trans("Top {0} Males on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
                <span class="ttl" style="display: none">120</span>
                <%= String.Format(Lang.Trans("Top {0} Males"), Config.Ratings.TopUsersCount) %></h4>
                <div class="entry-content">
                    <uc1:SearchResults ID="SearchResults2" PaginatorEnabled="false" ShowSlogan="false"
                        ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server">
                    </uc1:SearchResults>
                </div>
            </div>
        </td>
        <td width="33%" valign="top" id="tdTopCouples" class="verticalSeparator" runat="server">
            <div id="divTopCouples" class="hslice">
                <h4 class="BoxSubHeading"><span class="entry-title" style="display: none">
                    <%= String.Format(Lang.Trans("Top {0} Couples on {1}"), Config.Ratings.TopUsersCount, Config.Misc.SiteTitle) %></span>
                <span class="ttl" style="display: none">120</span>
                <%= String.Format(Lang.Trans("Top {0} Couples"), Config.Ratings.TopUsersCount) %></h4>
                <div class="entry-content">
                    <uc1:SearchResults ID="SearchResults3" PaginatorEnabled="false" ShowSlogan="false"
                        ShowRating="true" ShowCity="false" ShowLastOnline="false" UseCache="true" runat="server">
                    </uc1:SearchResults>
                </div>
            </div>
        </td>
    </tr> 
</table> 
<div class="center"><asp:Label ID="lblNote" runat="server" /></div>