<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SmallSearchBox.ascx.cs" Inherits="ezFixUp.Components.Search.SmallSearchBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="../SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="../SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<TABLE width="100%" align="center">
	<TR>
		<TD>
			<b><%= Lang.Trans("Gender") %></b></TD>
		<TD>
			<asp:DropDownList id="dropGender" Runat="server"></asp:DropDownList></TD>
	</TR>
	<TR>
		<TD>
			<b><%= Lang.Trans("Age Range") %></b></TD>
		<TD><NOBR>
				<asp:TextBox id="txtAgeFrom" Runat="server" Width="30px" Size="4" MaxLength="2"></asp:TextBox>
				<%= Lang.Trans("to") %>
				<asp:TextBox id="txtAgeTo" Runat="server" Width="30px" Size="4" MaxLength="2"></asp:TextBox></NOBR></TD>
	</TR>
	<TR>
		<TD>
			<b><%= Lang.Trans("Photo") %></b></TD>
		<TD>
			<asp:CheckBox id="cbPhotoReq" Runat="server" Checked="True"></asp:CheckBox></TD>
	</TR>
</TABLE>
<div class="buttons">
    <uc1:FlexButton ID="btnBasicSearchGo" runat="server" OnClick="btnBasicSearchGo_Click" 
    		        RenderAs="LinkButton" CssClass="medium blue zurbtn fr"/>
	<%--<asp:Button id="btnBasicSearchGo" Runat="server" onclick="btnBasicSearchGo_Click"></asp:Button>--%>
</div><br />