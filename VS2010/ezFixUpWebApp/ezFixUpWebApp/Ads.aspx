<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ads.aspx.cs" Inherits="ezFixUp.Ads" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<asp:UpdatePanel ID="UpdatePanelCategories" runat="server">
	    <ContentTemplate>
    <div id="sidebar">
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
	        <div id="pnlAllAds" runat="server" visible="false" class="SideMenuItem"><asp:linkbutton ID="lnkAllAds" CssClass="SideMenuLink" runat="server" onclick="lnkAllClassifieds_Click"></asp:linkbutton></div>
		    <div id="pnlPostAd" runat="server" class="SideMenuItem"><asp:linkbutton ID="lnkPostAd" CssClass="SideMenuLink" runat="server" onclick="lnkPostAd_Click"></asp:linkbutton></div>
		    <div id="pnlMyAds" runat="server" class="SideMenuItem"><asp:linkbutton ID="lnkMyAds" CssClass="SideMenuLink" runat="server" onclick="lnkMyClassifieds_Click"></asp:linkbutton></div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
		<uc1:SmallBoxStart ID="SmallBoxStart2" runat="server" />
		<div class="center">
		    <div class="separator"></div>
		    <%= "Keyword".Translate() %>:
		    <asp:TextBox ID="txtKeyword" CssClass="textbox" runat="server"></asp:TextBox>
		    <div class="separator"></div>
		    <uc1:FlexButton ID="fbSearch" runat="server" RenderAs="Button" OnClick="lnkSearch_Click"/>
		    </div>
		<uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server" />
	</div>
	<div id="main">
	        <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
	        <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
	            <asp:MultiView ID="mvCategories" runat="server">
	                <asp:View ID="viewCategories" runat="server">
	                    <asp:DataList ID="dlCategories" Width="100%" runat="server" onitemdatabound="dlCategories_ItemDataBound">
	                        <ItemTemplate>
	                            <h3 class="SectionHeading"><%# Eval("Title") %></h3>
	                            
	                            <asp:Repeater ID="rptSubcategories" runat="server" >
	                                <ItemTemplate>
	                                    <div>
	                                    <a href='Ads.aspx?cid=<%# Eval("ID")%>'><%# Eval("Title") %></a></div>
	                                </ItemTemplate>
	                            </asp:Repeater>
	                        </ItemTemplate>
	                        <SeparatorTemplate>
	                        	<div class="separator"></div>
	                        </SeparatorTemplate>
	                    </asp:DataList>
	                </asp:View>
	                <asp:View ID="viewAds" runat="server">
	                    <asp:DataList ID="dlAds" runat="server">
	                        <ItemTemplate>
	                            <a href='ShowAd.aspx?id=<%# Eval("ID") %>'>
	                                <img class="photoframe" src='AdPhoto.ashx?id=<%# Eval("AdPhotoID") %>&width=90&height=90&diskCache=1' border="0"/>
	                            </a>
	                            <br />
	                            <span class="label"><%# Eval("Subject") %></span>&nbsp;<%# Eval("Pending") %><br /><span class="label"><%= "Posted by".Translate() %>:</span>&nbsp;<a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("PostedBy"))%>'><%# Eval("PostedBy") %></a><br /><span class="label"><%= "Date".Translate() %>:</span>&nbsp;<%# Eval("Date") %>
	                        </ItemTemplate>
	                        <SeparatorTemplate>
		                        <div class="line"></div>
		                        <div class="separator6"></div>
			                </SeparatorTemplate>  
	                    </asp:DataList>
	                    <asp:Panel ID="pnlPaginator" runat="server">
		                    <div class="endbox">
                                <asp:LinkButton ID="lnkFirst" runat="server" onclick="lnkFirst_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkPrev" runat="server" onclick="lnkPrev_Click"></asp:LinkButton>
                                <asp:Label ID="lblPager" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkLast" runat="server" onclick="lnkLast_Click"></asp:LinkButton>
                            </div>
                        </asp:Panel>
	                </asp:View>
	            </asp:MultiView>
	        <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
