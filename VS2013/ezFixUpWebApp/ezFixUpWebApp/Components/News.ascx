<%@ Control Language="c#" 
            AutoEventWireup="True" 
            Codebehind="News.ascx.cs" 
            Inherits="ezFixUp.Components.News" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="ezFixUp.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class="NewsBoxContent">
	<div class="scrollbarwrap">
		<asp:Repeater ID="rptNews" Runat="server" EnableViewState="false">
			<ItemTemplate>
			<div id="newswrap">
				<div class="date">
					<%# ((DateTime)Eval("Date")).ToShortDateString() %></div>
				<div class="text">
					<%# Eval("Title") %>&nbsp;[&nbsp;<a href='<%# "News.aspx?id=" + Eval("NID") %>'><%= Lang.Trans("more") %></a>&nbsp;]
				</div>
			</div>	
			</ItemTemplate>
		</asp:Repeater>
	</div>
</div>