<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="NewsBoxWebPart.ascx.cs" 
            Inherits="ezFixUp.Components.WebParts.NewsBoxWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagPrefix="uc1" TagName="News" Src="~/Components/News.ascx" %>
<div id="NewsBox">
	<uc1:News id="News1" runat="server"></uc1:News>	
</div>
<div class="clear"></div>
<div class="SideMenuWebpartBtm"></div>