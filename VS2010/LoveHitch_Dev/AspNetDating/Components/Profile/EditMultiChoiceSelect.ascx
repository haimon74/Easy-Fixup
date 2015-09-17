<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Profile.EditMultiChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
    <div ID="pnlUser" Runat="server">
	<uc1:HeaderLine ID="hlName" runat="server" />
    </div>
    <div ID="pnlAdmin" Runat="server">
	<h3 class="SectionHeading"><%= hlName.Title %></h3>
    </div>
    <div class="note">
	<asp:Label ID="lblDescription" Runat="server" />
    </div>
    <div class="hint">
	<asp:Label ID="lblHint" Runat="server" />
    </div>
    <asp:listbox id=lbValues runat="server" CssClass="font" SelectionMode="Multiple" Width="100%"></asp:listbox>
    <INPUT id=hidQuestionId type=hidden runat="server"> 
    <div class="separator"></div>
</div>