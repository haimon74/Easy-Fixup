<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="BirthdayBox.ascx.cs" Inherits="AspNetDating.Components.BirthdayBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<%@ OutputCache Duration="600" VaryByParam="none" %>

<div id="birthdaybox">
	<div class="box-top7">
		<h4 class="box-head7">
			<%= Lang.Trans("birthdays") %>
		</h4>
	</div>
	<div class="SideMenuBoxContent">
		<asp:Repeater ID="rptBirthdays" Runat="server">
			<ItemTemplate>
			<div class="text">
				&nbsp;&nbsp;<%# ((DateTime)Eval("Date")).ToShortDateString() %>
				<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>' onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("Username") %></a><br>
			</div>			
			</ItemTemplate>
		</asp:Repeater>
	</div>
	<div class="clear"></div>
</div>