<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="EditProfile.ascx.cs"
    Inherits="ezFixUp.Components.Profile.EditProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <input type="hidden" id="hidUsername" runat="server"/>
        <div id="profileCompleted">
            <p><%= String.Format("Your Profile is {0} % completed".Translate(),GetProfileCompletePercentage())%></p>
            <div id="pct100" style="width:300px;height:20px;background:Navy;padding:3px;">
                <asp:Panel ID="completedPctPanel" runat="server" Background="Green" Height="14" Width="2">
                </asp:Panel>
            </div>
        </div>
        <asp:PlaceHolder ID="plhProfile" runat="server"/>
        <div class="buttons">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"/>
        </div>
        <asp:Literal ID="litAlert" runat="server"/>
        <br>
        <div class="srvmsg">
            <asp:Label ID="lblError" CssClass="error" runat="server"/>
        </div>
        <br>
    </ContentTemplate>
</asp:UpdatePanel>
