<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="NewVideosWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.NewVideosWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<asp:MultiView ID="mvNewVideos" ActiveViewIndex="0" runat="server">
    <asp:View ID="vNewVideos" runat="server">
    <asp:DataList ID="dlNewVideos" CssClass="UsersBoxContent" Runat="server" 
            SkinID="NewVideosHome" Width="100%" RepeatDirection="Horizontal" 
            RepeatLayout="Table" CellSpacing=0 cellpadding="0" RepeatColumns="3" 
            onitemcreated="dlNewVideos_ItemCreated">
	    <ItemTemplate>
		    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><img src='<%# Eval("ThumbnailUrl") %>' class="photoframe" style="behavior: none" /></a>
		   <div class="clear"></div>
		    <a chref='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><br><span id="pnlGenderAge" runat="server" ><%# Lang.Trans(Eval("Gender").ToString()) %><span id="pnlDelimiter" runat="server">/</span><%# Eval("Age") %></span>
	    </ItemTemplate>
    </asp:DataList>
    </asp:View>
    <asp:View ID="vNoNewVideos" runat="server">
        <div class="separator"></div>
        <div class="center">
            <%= Lang.Trans("There are no new videos!") %></div>
        <div class="separator"></div>    
    </asp:View>
</asp:MultiView>
<div class="StandardWebpartBtm"></div>