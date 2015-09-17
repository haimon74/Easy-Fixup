<%@ Page Language="c#" MasterPageFile="Site.Master" Codebehind="Search.aspx.cs" AutoEventWireup="True"
    Inherits="AspNetDating.Search" %>

<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults1.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry"
        Category="Country" ServicePath="~/Services/Service.asmx" PromptText=" "
        ServiceMethod="GetCountries" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
        Category="Region" ServicePath="~/Services/Service.asmx" PromptText=" "
        ServiceMethod="GetRegionsByCountry" ParentControlID="dropCountry" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
        Category="City" ServicePath="~/Services/Service.asmx" LoadingText="[Loading cities...]" PromptText=" "
        ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry2" runat="server" TargetControlID="dropCountry2"
        Category="Country" ServicePath="~/Services/Service.asmx" PromptText=" "
        ServiceMethod="GetCountries" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState2" runat="server" TargetControlID="dropRegion2"
        Category="Region" ServicePath="~/Services/Service.asmx" PromptText=" "
        ServiceMethod="GetRegionsByCountry" ParentControlID="dropCountry2" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity2" runat="server" TargetControlID="dropCity2"
        Category="City" ServicePath="~/Services/Service.asmx" LoadingText="[Loading cities...]" PromptText=" "
        ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion2" />
    <div id="left">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <div class="SideMenuItem">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkQuickSearch" runat="server" OnClick="lnkQuickSearch_Click" />
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkAdvancedSearch" runat="server" OnClick="lnkAdvancedSearch_Click" />
        </div>
        <div class="SideMenuItem" id="trDistanceSearch" runat="server">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkDistanceSearch" runat="server" OnClick="lnkDistanceSearch_Click" />
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkOnlineSearch" runat="server" OnClick="lnkOnlineSearch_Click" />
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton CssClass="SideMenuLink" ID="lnkShowProfileViewers" runat="server"
                OnClick="lnkShowProfileViewers_Click" />
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <div class="clear"></div>
        <uc1:SmallBoxStart ID="SmallBoxStart2" CssClass="SideMenuBoxX" runat="server"></uc1:SmallBoxStart>
        	<div class="SideMenuBoxInfo" id="divSavedSearches" runat="server">
                <asp:DataList ID="dlSavedSearches" runat="server" Width="100%">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSavedSearch" runat="server" CommandName="SavedSearchView"
                            Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("ID") %>'>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkDeleteSavedSearch" runat="server" CommandName="SavedSearchDelete" Text="[x]" CommandArgument='<%# Eval("ID") %>'>
                        </asp:LinkButton>
                    </ItemTemplate>
                    <separatortemplate><br /></separatortemplate>
                </asp:DataList>
            </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div> <!-- fix for IE6, do not delete! -->
    <div id="right">
        <asp:Panel ID="pnlQuickSearch" DefaultButton="btnBasicSearchGo" runat="server">
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <uc1:HeaderLine ID="BasicSearchHeaderLine" runat="server"></uc1:HeaderLine>
            <div class="note"><%= Lang.Trans("This feature allows you to search based on terms such as age, relationship, and the "+ "presence of a photo.") %></div>
            <div class="SmallBoxWrap BoxWrapStyle">
            	<table class="searchtable" cellpadding="0" cellspacing="0">
            		<tr id="pnlGenderQuickSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("I am looking for") %></td>
            			<td>
							<asp:DropDownList ID="dropGender" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="trInterestedIn" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("who is seeking") %></td>
            			<td>
            				<asp:DropDownList ID="dropInterestedIn" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlCountry" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Country") %></td>
            			<td>
            				<asp:DropDownList ID="dropCountry" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlState" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Region/State") %></td>
            			<td>
            				<asp:DropDownList ID="dropRegion" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlCity" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("City") %></td>
            			<td>
            				<asp:DropDownList ID="dropCity" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlZip" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Zip/Postal Code") %></td>
            			<td>
            				<asp:TextBox ID="txtZip" runat="server" CssClass="textbox"></asp:TextBox>
            			</td>
            		</tr>
            		<tr id="pnlAgeRangeQuickSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Age Range") %>&nbsp;<%= Lang.Trans("from") %></td>
            			<td>
                            <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
                            <%= Lang.Trans("to") %>
                            <asp:TextBox ID="txtAgeTo" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td class="searchlabel"><%= Lang.Trans("Photo Required") %></td>
            			<td>
            				<asp:CheckBox ID="cbPhotoReq" CssClass="checkbox" runat="server" Checked="True"></asp:CheckBox>
            			</td>
            		</tr>
            		<tr>
            			<td colspan="2">
	                        <div class="line"></div>
            				<div class="buttons">
            				    <uc1:FlexButton ID="btnBasicSearchGo" runat="server" OnClick="btnBasicSearchGo_Click" 
    				                RenderAs="LinkButton" CssClass="SearchBtn"/>
	                			<%--<asp:Button ID="btnBasicSearchGo" runat="server" OnClick="btnBasicSearchGo_Click"></asp:Button>--%>
			                </div>
            			</td>
            		</tr>
            	</table>
            </div>
            <div class="separator"></div>
            <table cellpadding="5" cellspacing="0" width="100%">
            	<tr>
            		<td valign="top" width="50%">
	            		<asp:Panel ID="pnlUsernameSearch" DefaultButton="btnUsernameSearchGo" runat="server">
	                        <uc1:HeaderLine ID="UsernameSearchHeaderLine" runat="server"></uc1:HeaderLine>
	                        <%= Lang.Trans("Search	for another member by their Username") %><div class="separator"></div>
	                        <div class="LargeBoxWrap BoxWrapStyle center">
	                        <div class="hint"><%= Lang.Trans("For example: sillybilly or simone221") %></div>                      
	                            <%= Lang.Trans("Username") %>
	                            <asp:TextBox ID="txtUsername" cssclass="textbox" runat="server" Width="180px" Size="15"></asp:TextBox>
                                <div class="separator6"></div>
	                            <div class="line"></div>
	                            <div class="buttons">
	                                <uc1:FlexButton ID="btnUsernameSearchGo" runat="server" OnClick="btnUsernameSearchGo_Click" 
    				                    RenderAs="LinkButton" CssClass="SearchBtn"/>
	                            	<%--<asp:Button ID="btnUsernameSearchGo" runat="server" OnClick="btnUsernameSearchGo_Click"></asp:Button>--%>
	                            </div>
	                        </div>
	                	</asp:Panel>
            		</td>
            		<td valign="top">
	                	<asp:Panel ID="pnlKeywordSearch" DefaultButton="btnKeywordSearchGo" runat="server">
	                        <uc1:HeaderLine ID="KeywordSearchHeaderLine" runat="server"></uc1:HeaderLine>
	                        <%=Lang.Trans("Search for another member by Keywords") %><div class="separator"></div>                            
	                        <div class="LargeBoxWrap BoxWrapStyle center">
	                        <div class="hint"><%= Lang.Trans("Use keywords to uncover matches with similar hobbies and interests.") %></div>
                          
	                            <%= Lang.Trans("Keyword") %>
	                            <asp:TextBox ID="txtKeyword" runat="server" cssclass="textbox" Width="180px" Size="15"></asp:TextBox>
                                <div class="separator6"></div>
	                            <div class="line"></div>
	                            <div class="buttons">
	                                <uc1:FlexButton ID="btnKeywordSearchGo" runat="server" OnClick="btnKeywordSearchGo_Click" 
    				                    RenderAs="LinkButton" CssClass="SearchBtn"/>
	                                <%--<asp:Button ID="btnKeywordSearchGo" runat="server" OnClick="btnKeywordSearchGo_Click"></asp:Button>--%>
	                            </div>
	                        </div>
	                	</asp:Panel>
            		</td>
            	</tr>
            </table>
        	<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
		</asp:Panel>
       <asp:Panel ID="pnlCustomSearch" runat="server" DefaultButton="btnCustomSearchGo" Visible="False">
            <uc1:LargeBoxStart ID="LargeBoxStart3" runat="server"></uc1:LargeBoxStart>
            <uc1:HeaderLine ID="CustomSearchHeaderLine" runat="server"></uc1:HeaderLine>
            <div class="note">
                <%= Lang.Trans("This feature allows you to make more customizable search based on terms such as eye color, social status, bad habits, etc.") %></div>
            <div class="SmallBoxWrap BoxWrapStyle">
            	<table class="searchtable" cellpadding="0" cellspacing="0">
            		<tr id="pnlGenderCustomSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("I am looking for") %></td>
            			<td>
            				<asp:DropDownList ID="dropGender2" CssClass="dropdownlist" runat="server" AutoPostBack="True"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="trInterestedIn2" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("seeking") %></td>
            			<td>
            				<asp:DropDownList ID="dropInterestedIn2" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlCountry2" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Country") %></td>
            			<td>
            				<asp:DropDownList ID="dropCountry2" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlState2" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Region/State") %></td>
            			<td>
            				<asp:DropDownList ID="dropRegion2" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlCity2" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("City") %></td>
            			<td>
            				<asp:DropDownList ID="dropCity2" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlZip2" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Zip/Postal Code") %></td>
            			<td>
            				<asp:TextBox ID="txtZip2" CssClass="textbox" runat="server"></asp:TextBox>
            			</td>
            		</tr>
            		<tr id="pnlAgeCustomSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Age Range") %>&nbsp;<%= Lang.Trans("from") %></td>
            			<td>
	            			<asp:TextBox ID="txtAgeFrom2" CssClass="smalltextbox" runat="server" Size="2" MaxLength="2"></asp:TextBox>
	                        <%= Lang.Trans("to") %>
	                        <asp:TextBox ID="txtAgeTo2" CssClass="smalltextbox" runat="server" Size="2" MaxLength="2"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td class="searchlabel"><%= Lang.Trans("Photo Required") %></td>
            			<td>
            				<asp:CheckBox ID="cbPhotoReq2" CssClass="checkbox" runat="server" Checked="True"></asp:CheckBox>
            			</td>
            		</tr>
            	</table>
            </div>
                <asp:PlaceHolder ID="plhMaleSearchTerms" runat="server"></asp:PlaceHolder>
               	<asp:PlaceHolder ID="plhFemaleSearchTerms" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="plhCoupleSearchTerms" runat="server"></asp:PlaceHolder>
                <asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label>
                <asp:UpdatePanel id="ajaxRegionEmailMe" runat="server">
                    <ContentTemplate>
                        <div id="pnlSavedSearchOptions" runat="server">
                            <h3 class="SectionHeading"><%= Lang.Trans("Save search") %></h3>
                            <div class="note"><%= Lang.Trans("This feature allows you to save the prefered search options") %></div>
                            <asp:CheckBox ID="cbSaveSearch" runat="server" AutoPostBack="true" oncheckedchanged="cbSaveSearch_CheckedChanged"></asp:CheckBox>
                            <asp:Label ID="lblSavedSearchText" runat="server"></asp:Label>
                            <asp:TextBox ID="txtSavedSearchName" CssClass="textbox" runat="server"></asp:TextBox>
                            <div class="clear"></div>
                            <div id="pnlEmailMe" runat="server" visible="false">
                                <div class="separator"></div>
						        <asp:CheckBox ID="cbEmailMe" Runat="server"></asp:CheckBox>
						        <%= Lang.Trans("Email me the latest matches") %>&nbsp;
						        <asp:DropDownList cssclass="dropdownlist" ID="ddEmailFrequency" runat="server"></asp:DropDownList>
					        </div>
                    	</div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            	<div class="separator"></div>
                <div class="buttons">
                    <uc1:FlexButton ID="btnCustomSearchGo" runat="server" OnClick="btnCustomSearchGo_Click" 
    				                RenderAs="LinkButton" CssClass="SearchBtn"/>
	                <%--<asp:Button ID="btnCustomSearchGo" runat="server" OnClick="btnCustomSearchGo_Click"></asp:Button>--%>
                </div>
            <uc1:LargeBoxEnd ID="Largeboxend3" runat="server"></uc1:LargeBoxEnd>
        </asp:Panel>
        <asp:Panel ID="pnlDistanceSearch" Visible="False" DefaultButton="btnDistanceSearchGo" runat="server">
            <uc1:LargeBoxStart ID="LargeBoxStart4" runat="server"></uc1:LargeBoxStart>
            <uc1:HeaderLine ID="DistanceSearchHeaderLine" runat="server"></uc1:HeaderLine>
            <div class="note"><%= Lang.Trans("This feature will help you to find users near you") %></div>
            <div class="SmallBoxWrap BoxWrapStyle">
            	<table class="searchtable" cellpadding="0" cellspacing="0">
            		<tr id="pnlGenderDistanceSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("I am looking for") %></td>
            			<td>
            				<asp:DropDownList ID="dropGender3" runat="server" CssClass="dropdownlist"></asp:DropDownList>
            			</td>
            		</tr>
            		<tr id="pnlAgeDistanceSearch" runat="server">
            			<td class="searchlabel"><%= Lang.Trans("Age Range") %>&nbsp;<%= Lang.Trans("from") %></td>
            			<td>
	                        <asp:TextBox ID="txtAgeFrom3" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
	                        <%= Lang.Trans("to") %>
	                        <asp:TextBox ID="txtAgeTo3" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td class="searchlabel"><asp:Label ID="lblDistanceFromUser" runat="server"></asp:Label></td>
            			<td>
            				<asp:TextBox ID="txtDistanceFromUser" runat="server" CssClass="textbox"></asp:TextBox>
            			</td>
            		</tr>
            		<tr>
            			<td class="searchlabel"><%= Lang.Trans("Photo Required") %></td>
            			<td>
            				<asp:CheckBox ID="cbPhotoReq3" CssClass="checkbox" runat="server" Checked="True"></asp:CheckBox>
            			</td>
            		</tr>
            		<tr>
            			<td colspan="2">
	                        <div class="line"></div>
            				<div class="buttons">
                				<uc1:FlexButton ID="btnDistanceSearchGo" runat="server" OnClick="btnDistanceSearchGo_Click" 
    				                RenderAs="LinkButton" CssClass="SearchBtn"/>
    				            <%--<asp:Button ID="btnDistanceSearchGo" runat="server" OnClick="btnDistanceSearchGo_Click"></asp:Button>--%>
							</div>
            			</td>
            		</tr>
            	</table>
            </div>
            <uc1:LargeBoxEnd ID="Largeboxend4" runat="server"></uc1:LargeBoxEnd>
        </asp:Panel>
        <div>
        <asp:Panel ID="pnlSearchResults" runat="server" Visible="False">
            <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server"></uc1:LargeBoxStart>
            <div id="pnlFilterOnline" runat="server" class="UserFilter">
                <span id="pnlGenderFilterOnline" runat="server"><%= Lang.Trans("Gender") %>&nbsp;<asp:DropDownList ID="ddFilterByGender" cssclass="dropdownlist" runat="server"></asp:DropDownList>&nbsp;</span>
                <span id="pnlAgeFilterOnline" runat="server">
                    <%= Lang.Trans("Age") %>
                    <asp:TextBox ID="txtFromFilterOnline" runat="server" CssClass="age_textbox" MaxLength="2"></asp:TextBox>
                    <%= Lang.Trans("to") %>
                    <asp:TextBox ID="txtToFilterOnline" runat="server" CssClass="age_textbox" MaxLength="2"></asp:TextBox>
                    <uc1:FlexButton ID="btnFilterOnline" runat="server" OnClick="btnFilterOnline_Click" 
    				        RenderAs="LinkButton" CssClass="FilterBtn"/>
                <%--<asp:Button ID="btnFilterOnline" runat="server" OnClick="btnFilterOnline_Click"></asp:Button>--%>
                </span>                
            </div>
            <uc1:SearchResults ID="SearchResults" runat="server" PaginatorEnabled="True" EnableGridSupport="True"></uc1:SearchResults>
            <uc1:LargeBoxEnd ID="LargeBoxEnd2" runat="server"></uc1:LargeBoxEnd>
        </asp:Panel>
        </div>
       <div class="clear"></div>
       <div class="srvmsg"><asp:Label ID="lblError2" CssClass="error" runat="server"></asp:Label></div>
    </div></div>
</asp:Content>
