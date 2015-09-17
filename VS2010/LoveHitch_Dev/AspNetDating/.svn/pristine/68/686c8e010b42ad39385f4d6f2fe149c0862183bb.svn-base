<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchBox.ascx.cs" Inherits="AspNetDating.Components.Search.SearchBox"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Extenders" TagPrefix="cc2" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Controls" TagPrefix="cc1" %>
<%@ Register Src="../SearchLocationPicker.ascx" TagName="SearchLocationPicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<%@ Register TagPrefix="uc3" TagName="SearchGenderPicker" Src="~/Components/Search/SearchGenderPicker.ascx" %>
<%@ Register TagPrefix="uc4" TagName="AgeRangePicker" Src="~/Components/Search/AgeRangePicker.ascx" %>
<%@ Register src="SearchBoxContent.ascx" tagname="SearchBoxContent" tagprefix="uc5" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div id="SearchBox">
    <div class="SearchBoxTop">
        <h2 class="SearchBoxHeader">
            <%= Lang.Trans("Quick Search") %>
        </h2>
    </div>
    <div class="SearchBoxContent">
         <uc5:SearchBoxContent ID="SearchBoxContent1" runat="server" />
    </div>
</div>
