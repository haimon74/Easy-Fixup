<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="UpcomingEventsWebPart.ascx.cs" 
            Inherits="ezFixUp.Components.WebParts.UpcomingEventsWebPart" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<asp:MultiView ID="mvUpcomingEvents" ActiveViewIndex="0" runat="server">
<asp:View ID="viewUpcomingEvents" runat="server">
<asp:Repeater ID="rptGroupEvents" Runat="server">
	<ItemTemplate>
		<table cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td valign="top"  width="70" align="center">
				    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>
				        <img src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' class="photoframe" border="0" alt=""/>
				    </a>
				</td>
    			<td valign="top">
    				<div class="headerfix2">
			 	    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a></div>
			 	    <div class="clear"></div>
				    <span class="label"><%= Lang.Trans("Event date:") %></span>&nbsp;<%# Eval("Date") %>
				    <div class="clear"></div>
				    <span class="label"><%= Lang.Trans("Created by:") %></span>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a>
				    <div class="clear"></div>
					<span class="label" runat="server" Visible='<%# (string) Eval("Attenders") != "0" %>'><%= Lang.Trans("Attending:") %>&nbsp;<%# Eval("Attenders") %></span>
				</td>
			</tr>
		</table>   
	</ItemTemplate>
	<SeparatorTemplate>
    <div class="separator6"></div>
	</SeparatorTemplate>
</asp:Repeater>
</asp:View>
<asp:View ID="viewNoUpcomingEvents" runat="server">
    <div class="center">
		<div class="separator"></div>
		    <%= Lang.Trans("There are no upcoming events!") %><div class="separator"></div>
	    </div>
</asp:View>
</asp:MultiView>
<div class="StandardWebpartBtm"></div>