<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="NewGroups.ascx.cs" Inherits="AspNetDating.Components.Groups.NewGroups" %>
<asp:DataList ID="dlNewGroups" CssClass="UsersBoxContent" Runat="server" SkinID="NewGroups" 
Width="100%" RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing=2>
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
    		<img class="photoframe" src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=90&height=90&diskCache=1' border="0"/>
		</a>
		<div class="clear"></div>
		<a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'><%# Eval("Name") %></a><br />(<%# Eval("AccessLevel")%>)
    <div class="separator6"></div>
    </ItemTemplate>
</asp:DataList>