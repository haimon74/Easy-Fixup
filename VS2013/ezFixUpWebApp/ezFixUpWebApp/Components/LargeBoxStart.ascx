<%@ Control Language="c#" 
            AutoEventWireup="True" 
            Codebehind="LargeBoxStart.ascx.cs" 
            Inherits="ezFixUp.Components.LargeBoxStart" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class='<%= cssClass %>'>
<div id="StandardBoxTop" class="StandardBoxTop"><h2 class="StandardBoxHeader"><asp:Label ID="lblTitle" Runat="server"></asp:Label></h2></div>
<div class="StandardBoxContent skincolor">