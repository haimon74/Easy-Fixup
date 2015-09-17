<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchResults.ascx.cs" Inherits="AspNetDating.Components.Groups.SearchResults" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:DataList ID="dlGroups" Width="100%" runat="server">
    <ItemTemplate>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" width="94">
                    <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
                        <img class="photoframe" src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=90&height=90&diskCache=1' border="0" />
                    </a>
                </td>
                <td valign="top" class="pleft5">
                    <a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
                        <%# Eval("Name") %></a>&nbsp;(<%# Eval("AccessLevel")%>)
                    <br />
                    <div class="justify">
                        <%# Eval("Description") %><br />
                    </div>
                    <%# Lang.Trans("Members") %>:&nbsp;<b><%# Eval("MembersCount")%></b>
                    <div class="date">
                        <%# Lang.Trans("Created") %>&nbsp;<%# Eval("DateCreated")%>
                    </div>
                    <div class="clear">
                    </div>
                </td>
            </tr>
        </table>
    </ItemTemplate>
    <SeparatorTemplate>
        <div class="line">
        </div>
        <div class="separator6">
        </div>
    </SeparatorTemplate>
</asp:DataList>
<asp:Panel ID="pnlPaginator" runat="server">
    <div class="endbox">
        <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
        <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
        <asp:Label ID="lblPager" runat="server"></asp:Label>
        <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
        <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
    </div>
</asp:Panel>
