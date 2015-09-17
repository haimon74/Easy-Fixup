<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgeRangePicker.ascx.cs" Inherits="AspNetDating.Components.Search.AgeRangePicker" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
<table class="w100">
    <tr >
        <td class="w40">
            <%= "Age".Translate() %>
        </td>
        <td style="white-space: nowrap;text-indent:0;">
                <span class="fl" style="padding-left:1px;">
                <asp:UpdatePanel ID="up1" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                <asp:DropDownList ID="comboAgeFrom" runat="server" 
                    AutoPostBack="false"  OnSelectedIndexChanged="comboAgeFromSelectIndexChange"
                    CssClass="s_search_ddl"
                    EnableViewState="true">
                    <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList>
                </ContentTemplate>
                </asp:UpdatePanel>
                </span>
                <%--<span style="display:inline;margin:0 4px"><b><%= Lang.Trans("~") %></b></span>--%>
                <span class="fr" style="padding-right:4px;">
                <asp:UpdatePanel ID="up2" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                <asp:DropDownList ID="comboAgeTo" runat="server" 
                    AutoPostBack="false" OnSelectedIndexChanged="comboAgeToSelectIndexChange" 
                    CssClass="s_search_ddl"
                    EnableViewState="true">
                    <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList>
                </ContentTemplate>
                </asp:UpdatePanel>
                </span>
        </td>
    </tr>
    
</table>