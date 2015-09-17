<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularBlogPosts.ascx.cs" Inherits="AspNetDating.Components.Blog.PopularBlogPosts" %>

<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:Repeater ID="rptNewBlogs" runat="server">
    <ItemTemplate>
        <table cellpadding="2" cellspacing="2" width="100%">
        	<tr>
        		<td width="60" valign="top" align="center">
	        		<div class="overflow" style="width:60px">
				        <a class="newuser-pic" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
				        <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a></div>
				</td>
        		<td valign="top">
			        <%= Lang.Trans("Blog") %>:&nbsp;<%# Eval("BlogName") %>
					<div class="separator"></div>
			        <%= Lang.Trans("Blog Post") %>:&nbsp;
			        <a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"), (int) Eval("BlogPostID")) %>'><%# Eval("BlogPostTitle") %></a><div class="separator"></div>
        			<div class="info left"><%= Lang.Trans("Date posted") %>:&nbsp;<%# Eval("DatePosted") %></div>
       	   		</td>
			</tr>
	    </table>
    </ItemTemplate>
    <separatortemplate>
		<div class="separator6"></div>
		<div class="line"></div>
		<div class="separator6"></div>
    </separatortemplate>
</asp:Repeater>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>