<%@ Control Language="c#" 
            AutoEventWireup="True" 
            CodeBehind="EditTopic.ascx.cs" 
            Inherits="AspNetDating.Components.Profile.EditTopic"
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    function processEventInfo() {
        $get("hfIsNext").Value = isNextText;
    }
    var isNextText = '';
    function setIsNextHiddenField(isNext) {
        isNextText = isNext;
    }
</script>

<asp:Panel ID="EditTopicPanel1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <input type="hidden" id="hidUsername" runat="server" />
            <asp:HiddenField ID="hfCurrentTopic" runat="server" />
            <asp:HiddenField ID="hfNextTopic" runat="server" />
            <asp:HiddenField ID="hfIsNext" runat="server" />
            <asp:PlaceHolder ID="plhProfile" runat="server" />
            <table id="profileCompleted" style="margin: 5px;" width="190">
                <tr>
                    <td colspan="2" style="color: Navy;">
                        <%= String.Format("Your Profile is {0} % completed".Translate(), GetProfileCompletePercentage())%>
                    </td>
                </tr>
                <tr>
                    <td id="completed" runat="server" style="background: rgb(190,30,45); height: 8px;">
                    </td>
                    <td id="notCompleted" runat="server" style="height: 5px;">
                    </td>
                </tr>
            </table>
            <div class="buttons">
                <asp:Button ID="btnBack" runat="server" CssClass="medium zurbtn blue" OnClick="btnBack_Click"
                    OnClientClick="setIsNextHiddenField('');" />
                <%= Lang.Trans("topic") + " " +(CurrentTopicIndex+1).ToString() + " " + Lang.Trans("out of") + topicsPanels.Keys.Count %>
                <asp:Button ID="btnSkip" runat="server" CssClass="medium zurbtn blue" OnClick="btnSkip_Click"
                    OnClientClick="setIsNextHiddenField('true');" />
                <asp:Button ID="btnSave" runat="server" CssClass="medium zurbtn blue" OnClick="btnSave_Click"
                    OnClientClick="setIsNextHiddenField('true');" />
            </div>
            <asp:Literal ID="litAlert" runat="server" />
            <br>
            <div class="srvmsg">
                <asp:Label ID="lblError" CssClass="error" runat="server" />
            </div>
            <br>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
