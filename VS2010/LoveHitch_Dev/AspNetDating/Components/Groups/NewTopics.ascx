<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewTopics.ascx.cs" Inherits="AspNetDating.Components.Groups.NewTopics" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import namespace="AspNetDating"%>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<div id="pnlSearchTopic" runat="server">
    <asp:Panel DefaultButton="btnSearch" runat="server">
        <%= Lang.Trans("Find Topics") %>:
        <asp:TextBox ID="txtTopicToSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
        <div class="separator6"></div>
        <asp:CheckBox ID="cbSearchInPosts" runat="server" />
        <%= Lang.Trans("Search in posts") %></asp:Panel>
    <div class="separator6"></div>
    <div class="line"></div>
    <div class="separator6"></div>
</div>
<asp:DataGrid ID="dgGroupTopics" Runat="server" GridLines="None" 
AutoGenerateColumns="false" HorizontalAlign="Center" CssClass="TableWrap" CellPadding="0" CellSpacing="0">
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
            <div class="center">
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
<div id="pnlViewAllTopics" runat="server" visible="false" class="morelink">
	<asp:LinkButton ID="lnkViewAllTopics" runat="server" OnClick="lnkViewAllTopics_Click"></asp:LinkButton>
</div>
<div class="clear"></div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>	