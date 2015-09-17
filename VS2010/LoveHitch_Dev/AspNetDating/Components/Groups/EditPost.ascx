<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPost.ascx.cs" Inherits="AspNetDating.Components.Groups.EditPost" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagName="DatePicker" TagPrefix="uc1" Src="~/Components/DatePicker.ascx" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<div class="wrap-sized">
    <div class="srvmsg">
        <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
    <div id="pnlTopic" runat="server">
        <%= Lang.Trans("Topic") %>
        <asp:TextBox ID="txtTopicName" CssClass="titletextbox" runat="server"></asp:TextBox>
        <div class="separator">
        </div>
    </div>
    <div id="pnlPost" runat="server">
        <%= Lang.Trans("Post content") %>
        <div class="clear">
        </div>
        <div class="separator6">
        </div>
        <div id="pnlSmilies" runat="server">

            <script language="JavaScript" type="text/javascript">
							<!--
							function insertSmiley(text) {
								var area = document.forms[0].<%= MessageBodyClientId %>;
								area.focus();
								if (document.selection)
									document.selection.createRange().text = text;
								else
									area.value += text;
								return false;
							}
							//-->
            </script>

            <table width="100%">
                <tr>
                    <td>
                        <asp:ImageButton ID="ibtnPrevSmilies" ImageUrl="~/images/iconprev.gif" OnClick="ibtnPrevSmilies_Click"
                            runat="server" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatePanelSmilies" runat="server">
                            <ContentTemplate>
                                <asp:DataList ID="dlSmilies" SkinID="Smilies" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                    EnableViewState="false" runat="server">
                                    <ItemTemplate>
                                        <a href="#" onclick="return insertSmiley('<%# Eval("Key") %>')">
                                            <img src="<%# Eval("Image") %>" title="<%# Eval("Description") %>" border="0" /></a>
                                    </ItemTemplate>
                                </asp:DataList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ibtnPrevSmilies" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ibtnNextSmilies" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:ImageButton ID="ibtnNextSmilies" ImageUrl="~/images/iconnext.gif" OnClick="ibtnNextSmilies_Click"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:TextBox ID="txtPost" runat="server" CssClass="multilinetextbox" TextMode="MultiLine"></asp:TextBox>
        <div class="separator">
        </div>
    </div>
    <div id="pnlEditReason" runat="server">
        <%= Lang.Trans("Edit reason") %>
        <asp:TextBox ID="txtEditReason" CssClass="titletextbox" runat="server"></asp:TextBox>
        <div class="separator">
        </div>
    </div>
    <div id="pnlLocked" runat="server">
        <asp:CheckBox ID="cbLocked" runat="server" />
        <%= Lang.Trans("Locked") %>
        <div class="separator6">
        </div>
    </div>
    <div id="pnlSticky" runat="server" class="stickylabel">
        <asp:CheckBox ID="cbSticky" runat="server" />
        <%= Lang.Trans("Sticky Until") %>
    </div>
    <uc1:DatePicker ID="DatePicker1" CssClass="stickydate" runat="server"></uc1:DatePicker>
    <div class="clear">
    </div>
    <div class="separator6">
    </div>
    <div id="pnlMoveTopic" visible="false" runat="server">
        <%= Lang.Trans("Move topic to") %>
        <asp:DropDownList ID="ddMoveToGroups" runat="server" />
    </div>
    <asp:UpdatePanel ID="upPoll" runat="server">
        <ContentTemplate>
            <div id="pnlPoll" runat="server">
                <asp:CheckBox ID="cbCreatePoll" runat="server" AutoPostBack="true" OnCheckedChanged="cbCreatePoll_CheckedChanged" />
                <%= Lang.Trans("Create a poll") %><br>
                <div class="clear">
                </div>
                <div id="pnlChoices" runat="server" visible="false">
                    <asp:Repeater ID="rptChoices" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left">
                                    <%# String.Format(Lang.Trans("Choice {0}"), Eval("NumberOfChoice"))%>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtChoice" CssClass="textbox" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <tr>
                                <td colspan="2" class="separator6">
                                </td>
                            </tr>
                        </SeparatorTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="clear">
</div>
<div class="separator6">
</div>
<div class="buttons">
    <asp:Button ID="btnStartNewTopic" runat="server" OnClick="btnStartNewTopic_Click" />
    <asp:Button ID="btnCancelStartNewTopic" runat="server" OnClick="btnCancelStartNewTopic_Click" />
    <asp:Button ID="btnNewPost" runat="server" OnClick="btnNewPost_Click" />
    <asp:Button ID="btnCancelNewPost" runat="server" OnClick="btnCancelNewPost_Click" />
    <asp:Button ID="btnUpdatePost" runat="server" OnClick="btnUpdatePost_Click" />
    <asp:Button ID="btnCancelUpdatePost" runat="server" OnClick="btnCancelUpdatePost_Click" />
    <asp:Button ID="btnUpdateTopic" runat="server" OnClick="btnUpdateTopic_Click" />
    <asp:Button ID="btnCancelUpdateTopic" runat="server" OnClick="btnCancelUpdateTopic_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
