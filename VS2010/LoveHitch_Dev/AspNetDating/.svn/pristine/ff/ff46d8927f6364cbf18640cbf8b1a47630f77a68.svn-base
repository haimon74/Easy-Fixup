<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="BirthdayBoxWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.BirthdayBoxWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div id="birthdaybox">
	<div class="SideMenuBoxContent">
    	<div class="SideMenuBoxInfo">
            <asp:MultiView ID="mvBirthdays" ActiveViewIndex="0" runat="server">
            <asp:View ID="vBirthdays" runat="server">
                <asp:Repeater ID="rptBirthdays" Runat="server">
                    <ItemTemplate>
                    <div class="text">
                        <%# ((DateTime)Eval("Date")).ToShortDateString() %>&nbsp;
                        <a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>' onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("DisplayName")%></a><br>
                    </div>			
                    </ItemTemplate>
                </asp:Repeater>
            </asp:View>
            <asp:View ID="vNoBirthdays" runat="server">
                <div class="center">
                    <%= Lang.Trans("There are no birthdays today!") %></div>
            </asp:View>
            </asp:MultiView>
        </div>
	</div>
	<div class="clear"></div>
</div>
<div class="SideMenuWebpartBtm"></div>