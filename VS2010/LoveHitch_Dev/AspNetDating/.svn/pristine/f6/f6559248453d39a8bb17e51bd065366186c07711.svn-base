<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BrowseVideos.aspx.cs" Inherits="AspNetDating.BrowseVideos" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <uc1:smallboxstart id="SmallBoxStart2" runat="server"></uc1:smallboxstart>
        <div class="SideMenuBoxInfo">
        <table class="searchtable" cellpadding="0" cellspacing="0">
				<td class="searchlabel" width="30%"><%= Lang.Trans("Video") %></td>
    			<td width="70%">
            <asp:DropDownList CssClass="dropdownlist" ID="ddVideoType" runat="server" 
                    AutoPostBack="true" onselectedindexchanged="ddVideoType_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr id="pnlGenderFilterOnline" runat="server">
            	<td class="searchlabel"><%= Lang.Trans("Gender") %></td>
				<td><asp:DropDownList CssClass="dropdownlist" ID="ddGender" runat="server"></asp:DropDownList>
	            </td>
            </tr>
            <tr id="pnlAgeFilterOnline" runat="server">
                <td class="searchlabel"><%= Lang.Trans("Age") %></td>
                <td>
                <asp:TextBox ID="txtFromAge" runat="server" CssClass="smalltextbox" size="2" MaxLength="2"></asp:TextBox>
                <%= Lang.Trans("to") %>
                <asp:TextBox ID="txtToAge" runat="server" CssClass="smalltextbox" size="2" MaxLength="2"></asp:TextBox>
                </td>
            </tr>
            <tr id="pnlKeyword" runat="server" visible="false">
                <td class="searchlabel"><%= "Keyword".Translate() %></td>
                <td><asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
            	<td colspan="2">
	            <div class="buttons"><asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" /></div>
    	        </td>
            </tr>
        </table></div> 
        <uc1:smallboxend id="SmallBoxEnd2" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <uc1:largeboxstart id="LargeBoxStart" runat="server"></uc1:largeboxstart>
        <asp:UpdatePanel ID="UpdatePanelVideos" runat="server">
            <ContentTemplate>          
                <asp:DataList id="dlVideos" RepeatColumns="4" CssClass="UsersBoxContent" CellSpacing=0 cellpadding="0"
                    RepeatLayout="table" RepeatDirection="Horizontal" runat="server" Width="100%" 
                    EnableViewState="false">
	                <ItemTemplate>                    
	                    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
	                        <img src='<%# Eval("ThumbnailUrl") %>' />
	                    </a>
                        <div class="clear"></div>
	                    <%# Eval("Title") %>
	                    <div class="clear"></div>
	                    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                            <%# Eval("Username") %></a><br>
                        <span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %></span>
	                </ItemTemplate>
	            </asp:DataList>
	            <asp:Panel ID="pnlPaginator" Visible="True" Runat="server">
                    <div class="line"></div>
                    <div class="endbox">
	                    <asp:LinkButton id="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
	                    <asp:LinkButton id="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
	                    <asp:Label id="lblPager" runat="server"></asp:Label>
	                    <asp:LinkButton id="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
	                    <asp:LinkButton id="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:largeboxend id="LargeBoxEnd" runat="server"></uc1:largeboxend>
    </div>
</asp:Content>
