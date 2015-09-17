<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="BirthdayPicker.ascx.cs" 
            Inherits="ezFixUp.Components.BirthdayPicker" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Extenders" TagPrefix="cc2" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Controls" TagPrefix="cc1" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    jQuery(function($) {
        $("button").css( {padding:0});
    });
</script>
<asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
<asp:UpdatePanel ID="updPanel1" runat="server" UpdateMode="Always">
<ContentTemplate>
<cc1:DatePicker ID="MyDatePicker1" runat="server" ChangeYear="true" ChangeMonth="true" CssClass="hasbirthdaypicker l_form_txt" EnableViewState="true" NumberOfMonths="[1,1]"
     ShowButtonPanel="false" MinDate="01/01/1950" MaxDate="12/31/1999"  DefaultDate="06/15/1977" DateFormat="dd/mm/yy"  ButtonText="Open Calander"  ShowOptions="direction: 'right'"
     OnClientBeforeShow="jQuery(this).fadeOut().fadeIn();"  GotoCurrent="false" ShowOn="both" ButtonImage="Images/form-date.png">
</cc1:DatePicker>
</ContentTemplate>
</asp:UpdatePanel>