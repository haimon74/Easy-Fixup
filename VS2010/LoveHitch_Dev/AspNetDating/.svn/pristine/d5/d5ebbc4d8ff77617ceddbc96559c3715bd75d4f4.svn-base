<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ViewTopics.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewTopics" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Groups/SearchResults.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:Panel ID="pnlFilterTopics" DefaultButton="btnSearch" runat="server">
    <%= Lang.Trans("Find Topics") %>:
    <asp:TextBox ID="txtTopicToSearch" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <div class="separator6"></div>
    <asp:CheckBox ID="cbSearchInPosts" runat="server" />
    <%= Lang.Trans("Search in posts") %><div class="separator6"></div>
    <div class="line"></div>
	<div class="separator6"></div>
</asp:Panel>
<asp:DataGrid ID="dgGroupTopics" runat="server" AllowPaging="False" AutoGenerateColumns="False" 
HorizontalAlign="Center" GridLines="None" CssClass="TableWrap" CellPadding="0" CellSpacing="0" EnableViewState="false">
    <HeaderStyle CssClass="HeaderRow" />
    <AlternatingItemStyle CssClass="AlternativeRow" />
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>
	            <div class="center">
	                <%# Eval("Icon") %></div>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="center bold"></HeaderStyle>
            <ItemTemplate>
            <div class="center overflow">
                <a href='ShowUser.aspx?uid=<%# Eval("Username") %>'>
                    <%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
                    <a class="user-name" href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a></div>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="bold"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
            <div class="tdfix">
                <a href='<%# UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString(), (string) Eval("GroupTopicID")) %>'>
                    <%# Eval("GroupTopicName") %></a></div>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="center bold"/>
            <ItemStyle />
            <ItemTemplate>
            <div align="center">
                <%# Eval("Posts") %></div>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="center bold"/>
            <ItemStyle />
            <ItemTemplate>
            <div align="center">
                <%# Eval("Views") %></div>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="center bold"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
            <div align="center">
                <%# Eval("DateCreated") %></div>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<asp:Panel ID="pnlPaginator" runat="server">
	<div class="endbox">
            <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
            <asp:Label ID="lblPager" runat="server"></asp:Label>
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
	</div>
</asp:Panel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
