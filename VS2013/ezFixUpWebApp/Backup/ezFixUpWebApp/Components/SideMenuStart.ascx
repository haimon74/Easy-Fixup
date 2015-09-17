<%@ Control Language="c#" 
            AutoEventWireup="True" 
            Codebehind="SideMenuStart.ascx.cs" 
            Inherits="ezFixUp.Components.SideMenuStart" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class='<%= cssClass %>' style="border:<%= borderStyle %> 1px LightGray;">
	<div id="pnlHeader" class="SideMenuBoxTop" runat="server">
	    <h2 class="SideMenuBoxHeader"><asp:Label ID="lblTitle" Runat="server" /></h2>
	</div>
	<div class="SideMenuContent" style="background:transparent !important;">