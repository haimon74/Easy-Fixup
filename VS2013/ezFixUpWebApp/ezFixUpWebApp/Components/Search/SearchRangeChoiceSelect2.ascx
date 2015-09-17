<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="SearchRangeChoiceSelect2.ascx.cs"
    Inherits="ezFixUp.Components.Search.SearchRangeChoiceSelect2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="ezFixUp.Classes"%>
<div id="pnlID" enableviewstate="false" runat="server">
    <div class="expander">
        <a href="#" onclick="return false;" class="plus1" id="lnkExpender" runat="server">
            <asp:Literal ID="ltrName" runat="server" />
        </a>
    </div>
    <div id="divExpandee" class="expandee" style="display: none;padding-left:15px" runat="server">
        <table>
            <tr>
                <td><%= Lang.Trans("from") %></td>
                <td><asp:DropDownList ID="ddFrom" runat="server" CssClass="m_search_ddl"/></td>
            </tr>
            <tr>
                <td><%= Lang.Trans("to") %></td>
                <td><asp:DropDownList ID="ddTo" runat="server" CssClass="m_search_ddl"/></td>
            </tr>
        </table>        
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" name="hidQuestionId" />
</div>
