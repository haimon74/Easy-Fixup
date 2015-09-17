<%@ Control Language="C#" AutoEventWireup="true" 
            CodeBehind="QuickPairSearchWebPart.ascx.cs" 
            Inherits="ezFixUp.Components.WebParts.QuickPairSearchWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register src="../Search/SearchBoxContent.ascx" tagname="SearchBoxContent" tagprefix="uc1" %>

<div id="SearchBox">
    <div class="SearchBoxContent">        
    	<uc1:SearchBoxContent ID="SearchBoxContent1" runat="server" />
    </div>
</div>
<div class="SideMenuWebpartBtm"></div>
