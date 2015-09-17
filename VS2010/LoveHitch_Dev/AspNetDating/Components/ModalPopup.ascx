<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalPopup.ascx.cs"
    Inherits="AspNetDating.Components.ModalPopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<style type="text/css">
    .modalBackground
    {
        background-color: black;
        opacity: 0.9;
        filter: alpha(opacity=90);
    }
</style>

<script type="text/javascript">
    function onOk() {
        alert('OK');
    }
    function onCancel() {
        alert('Canceled');
    }
</script>

<cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="LinkButton1" PopupControlID="Panel1"
    BackgroundCssClass="modalBackground" DropShadow="true" OkControlID="OkButton"
    CancelControlID="CancelButton" RepositionMode="RepositionOnWindowResize" Drag="true"
    OnOkScript="onOk()" OnCancelScript="onCancel()" PopupDragHandleControlID="Panel2" />
<asp:LinkButton ID="LinkButton1" runat="server"><%= LinkText ?? "Link Button" %></asp:LinkButton>
<asp:Panel ID="Panel1" runat="server" BackColor="WhiteSmoke" Width="400px"
    BorderColor="Black" BorderWidth="3">
    <asp:Panel ID="Panel2" runat="server" BackColor="skyblue" Height="30px">
        <asp:Label ID="LabelTitle" Width="100%"  Font-Bold="true" runat="server" 
            Style="margin: 7px;margin-left:20px;margin-right:20px;"><%= TitleText ?? "The Title" %>: </asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelContent" runat="server" Width="100%" Style="padding: 10px;">
    </asp:Panel>
    <asp:Panel ID="PanelButtons" runat="server" Width="100%" Height="30px" HorizontalAlign="Center"  Style="padding: 50px;">
        <asp:Button ID="OkButton" runat="server" Text="OK" BackColor="OrangeRed" BorderColor="silver" 
            BorderStyle="Solid" Width="50px" BorderWidth="1px" Style="margin:10px;"/>
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" BackColor="OrangeRed" BorderColor="silver" 
            BorderStyle="Solid" BorderWidth="1px" Width="50px" Style="margin:10px;" />
    </asp:Panel>    
</asp:Panel>
