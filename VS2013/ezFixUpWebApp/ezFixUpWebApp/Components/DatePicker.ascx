<%@ Control Language="C#" 
            AutoEventWireup="true" 
            Codebehind="DatePicker.ascx.cs"
            Inherits="ezFixUp.Components.DatePicker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div id="divDatePicker" runat="server" >
    <asp:DropDownList ID="dropDay" runat="server" AutoPostBack="false" EnableViewState="true"
         BackColor="#99CCFF" BorderWidth="0" Width="50px" AppendDataBoundItems="true" >
    </asp:DropDownList>
    <asp:DropDownList ID="dropMonth" runat="server" AutoPostBack="false" EnableViewState="true"
         BackColor="#99CCFF" BorderWidth="0" Width="80px"  >
    </asp:DropDownList>
    <asp:DropDownList ID="dropYear" runat="server" AutoPostBack="false" EnableViewState="true"
         BackColor="#99CCFF" BorderWidth="0" Width="50px" >
    </asp:DropDownList>
</div>

