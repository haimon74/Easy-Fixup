<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="GroupBans.ascx.cs" Inherits="ezFixUp.Components.Groups.GroupBans" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
<div class="srvmsg"><asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label></div>
<div class="separator"></div>
<asp:DataGrid ID="dgGroupBans" runat="server" OnItemCommand="dgGroupBans_ItemCommand" AutoGenerateColumns="false"
CssClass="TableWrap" CellPadding="0" CellSpacing="0" GridLines="None">
    <HeaderStyle CssClass="HeaderRow" />
    <AlternatingItemStyle CssClass="AlternativeRow" />
    <Columns>
        <asp:TemplateColumn>
        <HeaderStyle Font-Bold="True"></HeaderStyle>        
            <ItemTemplate>
        			<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
        <HeaderStyle Font-Bold="True"></HeaderStyle>
            <ItemTemplate>
                <%= Lang.Trans("Expires on:") %>&nbsp;<%# Eval("ExpirationDate") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:LinkButton ID="lnkUnban" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Unban" Text='<%# Lang.Trans("Unban") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>