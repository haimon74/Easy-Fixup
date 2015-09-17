<%@ Import namespace="ezFixUp"%>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="c#" EnableViewState="false" AutoEventWireup="True" Codebehind="NewVideos.ascx.cs" Inherits="ezFixUp.Components.NewVideos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<asp:DataList ID="dlNewVideos" CssClass="UsersBoxContent" Runat="server" Width="100%" RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing=0 cellpadding="0">
	<ItemTemplate>
		<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><img src='<%# Eval("ThumbnailUrl") %>' class="photoframe" style="behavior: none" /></a>
		<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a><div><%# Lang.Trans(Eval("Gender").ToString()) %>/<%# Eval("Age") %></div>
	</ItemTemplate>
</asp:DataList>