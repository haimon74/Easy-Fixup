<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericIcon.ascx.cs" Inherits="ezFixUp.Components.Buttons.GenericIcon" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagName="FlexButton" Src="~/Components/FlexButton.ascx" TagPrefix="uc1" %>



<script type="text/javascript">
    jQuery(function($) {
        $('.tipsy').tipsy();
        $('.tipsye').tipsy({ gravity: 'e' });
        $('.tipsys').tipsy({ gravity: 's' });
        $('.tipsyw').tipsy({ gravity: 'w' });
    });
</script>


<uc1:FlexButton ID="fbIcon" runat="server" RenderAs="LinkButton" 
                CssClass="MoreBtnMini tipsy" />