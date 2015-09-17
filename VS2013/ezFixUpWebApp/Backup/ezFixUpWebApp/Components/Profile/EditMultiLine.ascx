<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiLine.ascx.cs" Inherits="ezFixUp.Components.Profile.EditMultiLine" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
    <div ID="pnlUser" Runat="server">
	<uc1:HeaderLine ID="hlName" runat="server" />
    </div>
    <div id="tdAdmin" runat="server">
		<h3 class="SectionHeading"><%= hlName.Title %></h3>
    </div>
    <div class="note">
	<asp:Label ID="lblDescription" Runat="server" />
    </div>
    <div class="hint">
	<asp:Label ID="lblHint" Runat="server" />
    </div>
	<asp:TextBox ID="txtValue" TextMode="MultiLine" CssClass="multilinetextbox" Runat="server" /><br />
    <asp:TextBox ID="txtCharCount" ReadOnly="true" CssClass="charcount" runat="server" style="border:solid 1px grey;"></asp:TextBox>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId"/>
    <div class="separator"></div>
</div>