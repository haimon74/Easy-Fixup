<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucComposeMessage.ascx.cs" Inherits="AspNetDating.Components.Messages.ucComposeMessage" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

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
<div>
    <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
    	<tr>
        	<td valign="top" width="60px">
            	<asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
            </td>
            <td valign="top">
            	<span class="label"><%= Lang.Trans("From") %>:</span>&nbsp;<asp:Label ID="lblFromUsername" runat="server"></asp:Label><br />
                <span class="label"><%= Lang.Trans("To") %>:</span>&nbsp;<asp:Label ID="lblToUsername" runat="server"></asp:Label>
            </td>
        </tr>
    </table>        
    <div class="wrap-sized">
	<div class="separator"></div>        
        <div class="separator"></div>
        <div id="pnlUserResponse" runat="server">
            <div id="pnlSmilies" runat="server">                
                <table width="90%">
                    <tr>
                        <td>
                            <asp:ImageButton ID="ibtnPrevSmilies" ImageUrl="~/images/iconprev.gif" OnClick="ibtnPrevSmilies_Click"
                                runat="server" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updatePanelSmilies" runat="server">
                                <ContentTemplate>
                                    <asp:DataList ID="dlSmilies" SkinID="Smilies" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server">
                                        <ItemTemplate>
                                            <a href="#" onclick="return insertSmiley('<%# Eval("Key") %>')">
                                                <img alt="smiley" src="<%# Eval("Image") %>" title="<%# Eval("Description") %>" border="0" /></a>
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
            <asp:TextBox ID="txtMessageBody" CssClass="multilinetextbox" runat="server" TextMode="MultiLine"></asp:TextBox><br />
        </div>
    </div>
    <div class="buttons">
        <asp:Button ID="btnSend" runat="server"></asp:Button>
    </div>
    <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
</div>
