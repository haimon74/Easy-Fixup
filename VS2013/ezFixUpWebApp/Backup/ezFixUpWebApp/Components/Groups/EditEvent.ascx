<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEvent.ascx.cs" Inherits="ezFixUp.Components.Groups.EditEvent" %>
<%@ Import namespace="ezFixUp"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
    TargetControlID="txtHoursMin" 
    Mask="99:99"
    AcceptAMPM="true"
    MessageValidatorTip="true"
    OnFocusCssClass="MaskedEditFocus" 
    OnInvalidCssClass="MaskedEditError"
    MaskType="Time" 
    InputDirection="LeftToRight" 
    ErrorTooltipEnabled="True"/>
    <ajaxToolkit:MaskedEditValidator runat="server"
    ControlExtender="MaskedEditExtender1"
    ControlToValidate="txtHoursMin" 
    IsValidEmpty="True" 
    EmptyValueBlurredText="*" 
    Display="Dynamic" 
/>
<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<table cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top">
			<div id="pnlEventImage" runat="server"><img class="photoframe" src='GroupEventImage.ashx?id=<%= EventID %>&width=120&height=120&diskCache=1' alt=""/>&nbsp;</div>
		</td>
		<td valign="top" align="left">
			<%= Lang.Trans("Image") %>&nbsp;<asp:FileUpload ID="fuImage" runat="server" /></td>
	</tr>
</table>
<div class="separator clear"></div>
<div class="separator"></div>
<%= Lang.Trans("Title") %><br>
<asp:TextBox ID="txtTitle" CssClass="titletextbox" runat="server"></asp:TextBox><br />
<div class="separator"></div>
<%= Lang.Trans("Description") %>
<div class="clear"></div>
<asp:TextBox ID="txtDescription" runat="server" CssClass="multilinetextbox" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox><br />
<div class="separator"></div>
<%= Lang.Trans("Location") %>
<asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
<div class="separator"></div>
<table cellspacing="0" cellpadding="0">
	<tr>
		<td align="left" valign="middle">
			<%= Lang.Trans("Date") %>:&nbsp;
		</td>
		<td colspan="2" align="left" valign="middle">
			<uc2:DatePicker ID="datePicker1" runat="server"></uc2:DatePicker>
		</td>
	</tr>
	<tr>
		<td colspan="3" class="separator"></td>
	</tr>
	<tr>
		<td align="left" valign="middle">
			<%= Lang.Trans("Time") %>:&nbsp;
		</td>
		<td align="left" valign="middle">
			<asp:TextBox ID="txtHoursMin" runat="server"></asp:TextBox>
		</td>
		<td class="italic" valign="middle">
			&nbsp;<%= Lang.Trans("* Type 'A' or 'P' to switch AM/PM  ") %>
		</td>
	</tr>
</table>
<div class="separator"></div>
<div class="buttons">
	<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" />
	<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>