﻿<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="LocationPicker.ascx.cs"
            Inherits="ezFixUp.Components.LocationPicker" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    jQuery(function($) {
        $('select').each(function() {
            //$(this).trigger('changed');
        });
    });
</script>

<ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry" 
    Category="Country" ServicePath="~/Services/Service.asmx" PromptText=" " ServiceMethod="GetCountries" />
<ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
    Category="Region" ServicePath="~/Services/Service.asmx" PromptText=" " ServiceMethod="GetRegionsByCountry"
    ParentControlID="dropCountry" BehaviorID="" />
<ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
    Category="City" ServicePath="~/Services/Service.asmx" 
    PromptText=" " ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
<table class="w100">
    <tr>
        <td  class="w40">
            <%= Lang.Trans("Country") %>
        </td>
        <td  >
            <asp:DropDownList ID="dropCountry" CssClass="l_form_ddl" runat="server"  >
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trState" runat="server">
        <td  class="w40">
            <%= Lang.Trans("Region/State") %>
        </td>
        <td>
            <asp:DropDownList ID="dropRegion" CssClass="l_form_ddl" runat="server"  >
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="w40">
            <%= Lang.Trans("City") %>
        </td>
        <td>
            <asp:DropDownList ID="dropCity" CssClass="l_form_ddl" runat="server" >
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="w40">
            <%= Lang.Trans("Zip/Postal Code") %>
        </td>
        <td>
            <asp:TextBox ID="txtZipCode" CssClass="l_form_txt" runat="server"></asp:TextBox>
        </td>
    </tr>
    
</table>
