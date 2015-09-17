<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="EditSingleLine.ascx.cs"
    Inherits="ezFixUp.Components.Profile.EditSingleLine" 
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="../HeaderLine.ascx" TagName="HeaderLine" TagPrefix="uc1" %>
<div id="pnlID" runat="server">
    <table cellpadding="0" cellspacing="0" class="w100">
        <tr>
            <td>
                <div id="tdUser" runat="server">
                    <uc1:HeaderLine ID="hlName" Visible="false" runat="server" />
                    <asp:Label ID="lblName" runat="server" Font-Bold="true"></asp:Label>:&nbsp;
                </div>
                <div id="tdAdmin" runat="server">
                    <div class="label">
                        <%= lblName.Text %>
                    </div>
                </div>
                <div class="fl note" id="divDescription" style="display: none;">
                    <asp:Label ID="lblDescription" Visible="false" runat="server" />
                </div>
                <div class="hint" id="divHint" style="display: none;">
                    <asp:Label ID="lblHint" Visible="false" runat="server" />
                </div>
                <asp:TextBox ID="txtValue" TextMode="SingleLine" MaxLength="80" CssClass="textbox"
                    runat="server" Width="85%"/>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidQuestionId" runat="server">&nbsp;
    <div class="separator">
    </div>
</div>
