<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="PopularBlogPostsWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.PopularBlogPostsWebPart" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:MultiView ID="mvNewBlogs" ActiveViewIndex="0" runat="server">
    <asp:View ID="vNewBlogs" runat="server">
    <asp:Repeater ID="rptNewBlogs" runat="server">
        <ItemTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
        	    <tr>
        		    <td width="70" valign="top" align="center">
	        		    <div class="overflow" style="width:70px">
				            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
				            <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a></div>
				    </td>
        		    <td valign="top">
			            <span class="label"><%= Lang.Trans("Blog") %></span>:&nbsp;<%# Eval("BlogName") %><br>
			            <div class="headerfix2">
			            <span class="label"><%= Lang.Trans("Blog Post") %></span>:
			            <a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"), (int) Eval("BlogPostID")) %>'><%# Eval("BlogPostTitle") %></a></div>
        			    <div class="info left"><%= Lang.Trans("Date posted") %>:&nbsp;<%# Eval("DatePosted") %></div>
       	   		    </td>
			    </tr>
	        </table>
        </ItemTemplate>
        <separatortemplate>
        	<div class="separator6"></div>
        </separatortemplate>
    </asp:Repeater>
    </asp:View>
    <asp:View ID="vNoNewBlogs" runat="server">
        <div class="separator"></div>
        <div class="center">
            <%= Lang.Trans("There are no blog posts!") %></div>
        <div class="separator"></div>       
    </asp:View>
</asp:MultiView>
<div class="StandardWebpartBtm"></div>