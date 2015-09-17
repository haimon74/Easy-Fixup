<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEvents.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewEvents" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBox" runat="server"></uc1:LargeBoxStart>
<asp:UpdatePanel ID="pnlCalendar" runat="server">
    <ContentTemplate>
        <asp:Calendar SkinID="TitleStyle" ID="calendar1" runat="server" CssClass="calendar" DayHeaderStyle-CssClass="DayHeaderStyle" 
        DayStyle-CssClass="DayStyle" NextPrevStyle-CssClass="NextPrevStyle" OtherMonthDayStyle-CssClass="OtherMonthDayStyle" 
        SelectedDayStyle-CssClass="SelectedDayStyle" SelectorStyle-CssClass="SelectorStyle" TitleStyle-CssClass="TitleStyle" 
        TodayDayStyle-CssClass="TodayDayStyle" WeekendDayStyle-CssClass="WeekendDayStyle" CellPadding="0" CellSpacing="0" 
        OnSelectionChanged="calendar1_SelectionChanged"  
        OnDayRender="calendar1_DayRender"></asp:Calendar><br />
	    <div class="separator"></div>
	    <h3 class="SectionHeading">
		    <%= Lang.Trans("Group events for date") %>&nbsp;<asp:Label ID="lblDate" runat="server"></asp:Label>
        </h3>
        <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="false"></asp:Label></div>
        <asp:Repeater ID="rptEvents" runat="server">        
	   		<ItemTemplate>
		    	<table cellpadding="0" cellspacing="0" width="100%">
		    		<tr>
		    			<td valign="top" width="60">
					        <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>        
						        <img class="photoframe" src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' border="0" alt=""/>
						    </a>		
		    			</td>
		    			<td valign="top">
					 	    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a><div class="clear"></div>
						    <b><%= Lang.Trans("Event date:") %></b>&nbsp;<%# Eval("Date") %>
					 	    <div class="clear"></div>
						    <b><%= Lang.Trans("Created by:") %></b>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a>
						    <div class="clear"></div>
						    <asp:Label ID="lblAttenders" runat="server" Text='<%# "<b>" + Lang.Trans("Attending: ") + "</b>" + Eval("Attenders") %>' Visible='<%# (string) Eval("Attenders") != "0" %>'></asp:Label>
		    			</td>
		    		</tr>
		    	</table>    	
    		</ItemTemplate>
			<SeparatorTemplate>
				<div class="separator6"></div>
				<div class="line"></div>
				<div class="separator6"></div>
			</SeparatorTemplate>
		</asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>