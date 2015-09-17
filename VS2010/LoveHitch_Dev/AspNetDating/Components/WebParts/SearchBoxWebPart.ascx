<%@ Control Language="C#" AutoEventWireup="true" 
            CodeBehind="SearchBoxWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.SearchBoxWebPart" %>
<%@ Register src="../Search/SearchBoxContent.ascx" tagname="SearchBoxContent" tagprefix="uc1" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div id="SearchBox">
    <div class="SearchBoxContent">        
    	<uc1:SearchBoxContent ID="SearchBoxContent1" runat="server" />
    </div>
</div>
<div class="SideMenuWebpartBtm"></div>
