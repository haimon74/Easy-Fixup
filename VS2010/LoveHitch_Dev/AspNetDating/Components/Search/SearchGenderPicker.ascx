<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGenderPicker.ascx.cs" Inherits="AspNetDating.Components.Search.SearchGenderPicker" %>
<%@ Import Namespace="AspNetDating.Classes" %>


<table class="w100">
    <tr>
        <td class="w40">
            <%= Lang.Trans("Search gender")%><br />
        </td>
        <td class="w60">
            <asp:DropDownList ID="dropGender" runat="server" AutoPostBack="false" EnableViewState="true"
                CssClass="m_search_ddl">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="w40">
            <%= Lang.Trans("Interested in")%>
        </td>
        <td class="w60">
            <asp:DropDownList ID="dropInterestedIn" runat="server" AutoPostBack="false" EnableViewState="true"
                CssClass="m_search_ddl">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    
</table>         