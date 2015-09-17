<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="NewGroupsWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.NewGroupsWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<asp:DataList ID="dlNewGroups" CssClass="UsersBoxContent" Runat="server" SkinID="NewGroups" 
Width="100%" RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing=2 RepeatColumns=4>
	<ItemStyle HorizontalAlign="Center"></ItemStyle>
    <ItemTemplate>
        <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
    		<img class="photoframe" src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=90&height=90&diskCache=1' border="0"/>
		</a>
		<div class="clear"></div>
		<div class="overflow" style="width:100px;">
		<a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'><%# Eval("Name") %></a><br />(<%# Eval("AccessLevel")%>)
		</div>
    <div class="separator6"></div>
    </ItemTemplate>
</asp:DataList>
<div class="StandardWebpartBtm"></div>